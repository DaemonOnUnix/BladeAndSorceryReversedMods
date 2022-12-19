using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.SapiInterop;
using System.Threading;

namespace System.Speech.Recognition
{
	// Token: 0x02000195 RID: 405
	internal class RecognizerBase : IRecognizerInternal, IDisposable, ISpGrammarResourceLoader
	{
		// Token: 0x06000A26 RID: 2598 RVA: 0x0002BE7A File Offset: 0x0002AE7A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002BE8C File Offset: 0x0002AE8C
		~RecognizerBase()
		{
			this.Dispose(false);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002BEBC File Offset: 0x0002AEBC
		internal void LoadGrammar(Grammar grammar)
		{
			try
			{
				GrammarState[] array = new GrammarState[1];
				this.ValidateGrammar(grammar, array);
				if (!this._supportsSapi53)
				{
					RecognizerBase.CheckGrammarOptionsOnSapi51(grammar);
				}
				ulong num;
				SapiGrammar sapiGrammar = this.CreateNewSapiGrammar(out num);
				try
				{
					this.LoadSapiGrammar(grammar, sapiGrammar, grammar.Enabled, grammar.Weight, grammar.Priority);
				}
				catch
				{
					sapiGrammar.Dispose();
					grammar.State = GrammarState.Unloaded;
					grammar.InternalData = null;
					throw;
				}
				grammar.InternalData = new InternalGrammarData(num, sapiGrammar, grammar.Enabled, grammar.Weight, grammar.Priority);
				lock (this.SapiRecognizer)
				{
					this._grammars.Add(grammar);
				}
				grammar.Recognizer = this;
				grammar.State = GrammarState.Loaded;
			}
			catch (Exception ex)
			{
				this._loadException = ex;
				throw;
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0002BFA8 File Offset: 0x0002AFA8
		internal void LoadGrammarAsync(Grammar grammar)
		{
			if (!this._supportsSapi53)
			{
				RecognizerBase.CheckGrammarOptionsOnSapi51(grammar);
			}
			GrammarState[] array = new GrammarState[1];
			this.ValidateGrammar(grammar, array);
			ulong num;
			SapiGrammar sapiGrammar = this.CreateNewSapiGrammar(out num);
			grammar.InternalData = new InternalGrammarData(num, sapiGrammar, grammar.Enabled, grammar.Weight, grammar.Priority);
			lock (this.SapiRecognizer)
			{
				this._grammars.Add(grammar);
			}
			grammar.Recognizer = this;
			grammar.State = GrammarState.Loading;
			this._waitForGrammarsToLoad.StartOperation();
			if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadGrammarAsyncCallback), grammar))
			{
				throw new OperationCanceledException(SR.Get(SRID.OperationAborted, new object[0]));
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0002C06C File Offset: 0x0002B06C
		internal void UnloadGrammar(Grammar grammar)
		{
			this.ValidateGrammar(grammar, new GrammarState[]
			{
				GrammarState.Loaded,
				GrammarState.LoadFailed
			});
			InternalGrammarData internalData = grammar.InternalData;
			if (internalData != null)
			{
				internalData._sapiGrammar.Dispose();
			}
			lock (this.SapiRecognizer)
			{
				this._grammars.Remove(grammar);
			}
			grammar.State = GrammarState.Unloaded;
			grammar.InternalData = null;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002C0E8 File Offset: 0x0002B0E8
		internal void UnloadAllGrammars()
		{
			List<Grammar> list;
			lock (this.SapiRecognizer)
			{
				list = new List<Grammar>(this._grammars);
			}
			this._waitForGrammarsToLoad.WaitForOperationsToFinish();
			foreach (Grammar grammar in list)
			{
				this.UnloadGrammar(grammar);
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0002C170 File Offset: 0x0002B170
		void IRecognizerInternal.SetGrammarState(Grammar grammar, bool enabled)
		{
			InternalGrammarData internalData = grammar.InternalData;
			lock (this._grammarDataLock)
			{
				if (grammar.Loaded)
				{
					internalData._sapiGrammar.SetGrammarState(enabled ? SPGRAMMARSTATE.SPGS_ENABLED : SPGRAMMARSTATE.SPGS_DISABLED);
				}
				internalData._grammarEnabled = enabled;
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0002C1CC File Offset: 0x0002B1CC
		void IRecognizerInternal.SetGrammarWeight(Grammar grammar, float weight)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI2, new object[] { "Weight" }));
			}
			InternalGrammarData internalData = grammar.InternalData;
			lock (this._grammarDataLock)
			{
				if (grammar.Loaded)
				{
					if (grammar.IsDictation(grammar.Uri))
					{
						internalData._sapiGrammar.SetDictationWeight(weight);
					}
					else
					{
						internalData._sapiGrammar.SetRuleWeight(grammar.RuleName, 0U, weight);
					}
				}
				internalData._grammarWeight = weight;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0002C26C File Offset: 0x0002B26C
		void IRecognizerInternal.SetGrammarPriority(Grammar grammar, int priority)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI2, new object[] { "Priority" }));
			}
			InternalGrammarData internalData = grammar.InternalData;
			lock (this._grammarDataLock)
			{
				if (grammar.Loaded)
				{
					if (grammar.IsDictation(grammar.Uri))
					{
						throw new NotSupportedException(SR.Get(SRID.CannotSetPriorityOnDictation, new object[0]));
					}
					internalData._sapiGrammar.SetRulePriority(grammar.RuleName, 0U, priority);
				}
				internalData._grammarPriority = priority;
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002C314 File Offset: 0x0002B314
		Grammar IRecognizerInternal.GetGrammarFromId(ulong id)
		{
			lock (this.SapiRecognizer)
			{
				foreach (Grammar grammar in this._grammars)
				{
					InternalGrammarData internalData = grammar.InternalData;
					if (internalData._grammarId == id)
					{
						return grammar;
					}
				}
			}
			return null;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002C39C File Offset: 0x0002B39C
		void IRecognizerInternal.SetDictationContext(Grammar grammar, string precedingText, string subsequentText)
		{
			if (precedingText == null)
			{
				precedingText = string.Empty;
			}
			if (subsequentText == null)
			{
				subsequentText = string.Empty;
			}
			SPTEXTSELECTIONINFO sptextselectioninfo = new SPTEXTSELECTIONINFO(0U, 0U, (uint)precedingText.Length, 0U);
			string text = precedingText + subsequentText + "\0\0";
			SapiGrammar sapiGrammar = grammar.InternalData._sapiGrammar;
			sapiGrammar.SetWordSequenceData(text, sptextselectioninfo);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002C3EE File Offset: 0x0002B3EE
		internal RecognitionResult EmulateRecognize(string inputText)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			return this.InternalEmulateRecognize(inputText, SpeechEmulationCompareFlags.SECFDefault, false, null);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002C409 File Offset: 0x0002B409
		internal void EmulateRecognizeAsync(string inputText)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			this.InternalEmulateRecognizeAsync(inputText, SpeechEmulationCompareFlags.SECFDefault, false, null);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0002C424 File Offset: 0x0002B424
		internal RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			bool flag = compareOptions == 1 || compareOptions == 268435456;
			if (!this._supportsSapi53 && !flag)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			return this.InternalEmulateRecognize(inputText, RecognizerBase.ConvertCompareOptions(compareOptions), !flag, null);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0002C47C File Offset: 0x0002B47C
		internal void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			bool flag = compareOptions == 1 || compareOptions == 268435456;
			if (!this._supportsSapi53 && !flag)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			this.InternalEmulateRecognizeAsync(inputText, RecognizerBase.ConvertCompareOptions(compareOptions), !flag, null);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002C4D4 File Offset: 0x0002B4D4
		internal RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			Helpers.ThrowIfNull(wordUnits, "wordUnits");
			for (int i = 0; i < wordUnits.Length; i++)
			{
				if (wordUnits[i] == null)
				{
					throw new ArgumentException(SR.Get(SRID.ArrayOfNullIllegal, new object[0]), "wordUnits");
				}
			}
			return this.InternalEmulateRecognize(null, RecognizerBase.ConvertCompareOptions(compareOptions), true, wordUnits);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0002C544 File Offset: 0x0002B544
		internal void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			Helpers.ThrowIfNull(wordUnits, "wordUnits");
			for (int i = 0; i < wordUnits.Length; i++)
			{
				if (wordUnits[i] == null)
				{
					throw new ArgumentException(SR.Get(SRID.ArrayOfNullIllegal, new object[0]), "wordUnits");
				}
			}
			this.InternalEmulateRecognizeAsync(null, RecognizerBase.ConvertCompareOptions(compareOptions), true, wordUnits);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002C5B3 File Offset: 0x0002B5B3
		internal void RequestRecognizerUpdate()
		{
			this.RequestRecognizerUpdate(null);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0002C5BC File Offset: 0x0002B5BC
		internal void RequestRecognizerUpdate(object userToken)
		{
			uint num = this.AddBookmarkItem(userToken);
			this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_PAUSE, 0UL, new IntPtr((long)((ulong)num)));
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0002C5E8 File Offset: 0x0002B5E8
		internal void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
		{
			if (audioPositionAheadToRaiseUpdate < TimeSpan.Zero)
			{
				throw new NotSupportedException(SR.Get(SRID.NegativeTimesNotSupported, new object[0]));
			}
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			uint num = this.AddBookmarkItem(userToken);
			this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_PAUSE | SPBOOKMARKOPTIONS.SPBO_AHEAD | SPBOOKMARKOPTIONS.SPBO_TIME_UNITS, (ulong)audioPositionAheadToRaiseUpdate.Ticks, new IntPtr((long)((ulong)num)));
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0002C654 File Offset: 0x0002B654
		internal void Initialize(SapiRecognizer recognizer, bool inproc)
		{
			this._sapiRecognizer = recognizer;
			this._inproc = inproc;
			this._recoThunk = new RecognizerBase.RecognizerBaseThunk(this);
			try
			{
				this._sapiContext = this._sapiRecognizer.CreateRecoContext();
			}
			catch (COMException ex)
			{
				if (!this._supportsSapi53 && ex.ErrorCode == -2147200966)
				{
					throw new PlatformNotSupportedException(SR.Get(SRID.RecognitionNotSupported, new object[0]));
				}
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
			}
			this._supportsSapi53 = recognizer.IsSapi53;
			if (this._supportsSapi53)
			{
				this._sapiContext.SetGrammarOptions(SPGRAMMAROPTIONS.SPGO_ALL);
			}
			try
			{
				ISpPhoneticAlphabetSelection spPhoneticAlphabetSelection = this._sapiContext as ISpPhoneticAlphabetSelection;
				if (spPhoneticAlphabetSelection != null)
				{
					spPhoneticAlphabetSelection.SetAlphabetToUPS(true);
				}
			}
			catch (COMException)
			{
			}
			this._sapiContext.SetAudioOptions(SPAUDIOOPTIONS.SPAO_RETAIN_AUDIO, IntPtr.Zero, IntPtr.Zero);
			this.MaxAlternates = 10;
			this.ResetBookmarkTable();
			this._eventInterest = 854759695187968UL;
			this._sapiContext.SetInterest(this._eventInterest, this._eventInterest);
			this._asyncWorker = new AsyncSerializedWorker(new WaitCallback(this.DispatchEvents), null);
			this._asyncWorkerUI = new AsyncSerializedWorker(null, AsyncOperationManager.CreateOperation(null));
			this._asyncWorkerUI.WorkItemPending += new WaitCallback(this.SignalHandlerThread);
			this._eventNotify = this._sapiContext.CreateEventNotify(this._asyncWorker, this._supportsSapi53);
			this._grammars = new List<Grammar>();
			this._readOnlyGrammars = new ReadOnlyCollection<Grammar>(this._grammars);
			this.UpdateAudioFormat(null);
			this.InitialSilenceTimeout = TimeSpan.FromSeconds(30.0);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0002C7FC File Offset: 0x0002B7FC
		internal void RecognizeAsync(RecognizeMode mode)
		{
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
				}
				if (!this._haveInputSource)
				{
					throw new InvalidOperationException(SR.Get(SRID.RecognizerNoInputSource, new object[0]));
				}
				this._isRecognizing = true;
			}
			this._recognizeMode = mode;
			if (this._supportsSapi53)
			{
				if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.RecognizeAsyncWaitForGrammarsToLoad)))
				{
					throw new OperationCanceledException(SR.Get(SRID.OperationAborted, new object[0]));
				}
			}
			else
			{
				try
				{
					this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_ACTIVE_ALWAYS);
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiStreamError((SAPIErrorCodes)ex.ErrorCode);
				}
				catch
				{
					throw;
				}
			}
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0002C900 File Offset: 0x0002B900
		internal RecognitionResult Recognize(TimeSpan initialSilenceTimeout)
		{
			RecognitionResult result = null;
			bool completed = false;
			bool flag = false;
			EventHandler<RecognizeCompletedEventArgs> eventHandler = delegate(object sender, RecognizeCompletedEventArgs eventArgs)
			{
				result = eventArgs.Result;
				completed = true;
			};
			TimeSpan initialSilenceTimeout2 = this._initialSilenceTimeout;
			this.InitialSilenceTimeout = initialSilenceTimeout;
			this.RecognizeCompletedSync = (EventHandler<RecognizeCompletedEventArgs>)Delegate.Combine(this.RecognizeCompletedSync, eventHandler);
			TimeSpan timeSpan = TimeSpan.FromTicks(Math.Max(initialSilenceTimeout.Ticks, this._defaultTimeout.Ticks));
			try
			{
				this._asyncWorkerUI.AsyncMode = false;
				this.RecognizeAsync(RecognizeMode.Single);
				while (!completed && !this._disposed)
				{
					bool flag2;
					if (!flag)
					{
						flag2 = this._handlerWaitHandle.WaitOne(timeSpan, false);
						if (!flag2)
						{
							this.EndRecognitionWithTimeout();
							flag = true;
						}
					}
					else
					{
						flag2 = this._handlerWaitHandle.WaitOne(timeSpan, false);
					}
					if (flag2)
					{
						this._asyncWorkerUI.ConsumeQueue();
					}
				}
			}
			finally
			{
				this.RecognizeCompletedSync = (EventHandler<RecognizeCompletedEventArgs>)Delegate.Remove(this.RecognizeCompletedSync, eventHandler);
				this._initialSilenceTimeout = initialSilenceTimeout2;
				this._asyncWorkerUI.AsyncMode = true;
			}
			return result;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0002CA20 File Offset: 0x0002BA20
		internal void RecognizeAsyncCancel()
		{
			bool flag = false;
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					if (!this._isEmulateRecognition)
					{
						flag = true;
						this._isRecognizeCancelled = true;
					}
					else
					{
						this._isRecognizing = (this._isEmulateRecognition = false);
					}
				}
			}
			if (flag)
			{
				try
				{
					this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE_WITH_PURGE);
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
			}
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0002CAA8 File Offset: 0x0002BAA8
		internal void RecognizeAsyncStop()
		{
			bool flag = false;
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					flag = true;
					this._isRecognizeCancelled = true;
				}
			}
			if (flag)
			{
				try
				{
					this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE);
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002CB14 File Offset: 0x0002BB14
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0002CB1C File Offset: 0x0002BB1C
		internal bool PauseRecognizerOnRecognition
		{
			get
			{
				return this._pauseRecognizerOnRecognition;
			}
			set
			{
				if (value != this._pauseRecognizerOnRecognition)
				{
					this._pauseRecognizerOnRecognition = value;
					lock (this.SapiRecognizer)
					{
						foreach (Grammar grammar in this._grammars)
						{
							SapiGrammar sapiGrammar = grammar.InternalData._sapiGrammar;
							this.ActivateRule(sapiGrammar, grammar.Uri, grammar.RuleName);
						}
					}
				}
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002CBBC File Offset: 0x0002BBBC
		internal void SetInput(string path)
		{
			Stream stream = new FileStream(path, 3, 1, 1);
			this.SetInput(stream, null);
			this._inputStream = stream;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0002CBE4 File Offset: 0x0002BBE4
		internal void SetInput(Stream stream, SpeechAudioFormatInfo audioFormat)
		{
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
				}
				try
				{
					if (stream == null)
					{
						this.SapiRecognizer.SetInput(null, false);
						this._haveInputSource = false;
					}
					else
					{
						this.SapiRecognizer.SetInput(new SpAudioStreamWrapper(stream, audioFormat), false);
						this._haveInputSource = true;
					}
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
				this.CloseCachedInputStream();
				this.UpdateAudioFormat(audioFormat);
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002CC8C File Offset: 0x0002BC8C
		internal void SetInputToDefaultAudioDevice()
		{
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
				}
				ISpObjectTokenCategory spObjectTokenCategory = (ISpObjectTokenCategory)new SpObjectTokenCategory();
				try
				{
					spObjectTokenCategory.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\AudioInput", false);
					string text;
					spObjectTokenCategory.GetDefaultTokenId(out text);
					ISpObjectToken spObjectToken = (ISpObjectToken)new SpObjectToken();
					try
					{
						spObjectToken.SetId(null, text, false);
						this.SapiRecognizer.SetInput(spObjectToken, true);
					}
					catch (COMException ex)
					{
						throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
					}
					finally
					{
						Marshal.ReleaseComObject(spObjectToken);
					}
				}
				catch (COMException ex2)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex2);
				}
				finally
				{
					Marshal.ReleaseComObject(spObjectTokenCategory);
				}
				this.UpdateAudioFormat(null);
				this._haveInputSource = true;
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0002CD88 File Offset: 0x0002BD88
		internal int QueryRecognizerSettingAsInt(string settingName)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			return this.SapiRecognizer.GetPropertyNum(settingName);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0002CDA4 File Offset: 0x0002BDA4
		internal object QueryRecognizerSetting(string settingName)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			object obj;
			try
			{
				obj = this.SapiRecognizer.GetPropertyNum(settingName);
			}
			catch (Exception ex)
			{
				if (!(ex is COMException) && !(ex is InvalidOperationException) && !(ex is KeyNotFoundException))
				{
					throw;
				}
				obj = this.SapiRecognizer.GetPropertyString(settingName);
			}
			return obj;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002CE0C File Offset: 0x0002BE0C
		internal void UpdateRecognizerSetting(string settingName, string updatedValue)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			this.SapiRecognizer.SetPropertyString(settingName, updatedValue);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0002CE26 File Offset: 0x0002BE26
		internal void UpdateRecognizerSetting(string settingName, int updatedValue)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			this.SapiRecognizer.SetPropertyNum(settingName, updatedValue);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002CE40 File Offset: 0x0002BE40
		internal static Exception ExceptionFromSapiCreateRecognizerError(COMException e)
		{
			return RecognizerBase.ExceptionFromSapiCreateRecognizerError((SAPIErrorCodes)e.ErrorCode);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002CE50 File Offset: 0x0002BE50
		internal static Exception ExceptionFromSapiCreateRecognizerError(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (errorCode != SAPIErrorCodes.CLASS_E_CLASSNOTAVAILABLE && errorCode != SAPIErrorCodes.REGDB_E_CLASSNOTREG)
			{
				switch (errorCode)
				{
				case SAPIErrorCodes.SPERR_SHARED_ENGINE_DISABLED:
				case SAPIErrorCodes.SPERR_RECOGNIZER_NOT_FOUND:
					return new PlatformNotSupportedException(SR.Get(srid, new object[0]));
				default:
				{
					Exception ex = null;
					if (srid >= SRID.NullParamIllegal)
					{
						ex = new InvalidOperationException(SR.Get(srid, new object[0]));
					}
					else
					{
						try
						{
							Marshal.ThrowExceptionForHR((int)errorCode);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
					}
					return ex;
				}
				}
			}
			else
			{
				OperatingSystem osversion = Environment.OSVersion;
				if (IntPtr.Size == 8 && osversion.Platform == 2 && osversion.Version.Major == 5)
				{
					return new NotSupportedException(SR.Get(SRID.RecognitionNotSupportedOn64bit, new object[0]));
				}
				return new PlatformNotSupportedException(SR.Get(SRID.RecognitionNotSupported, new object[0]));
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002CF2C File Offset: 0x0002BF2C
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x0002CF68 File Offset: 0x0002BF68
		internal TimeSpan InitialSilenceTimeout
		{
			get
			{
				TimeSpan initialSilenceTimeout;
				lock (this.SapiRecognizer)
				{
					initialSilenceTimeout = this._initialSilenceTimeout;
				}
				return initialSilenceTimeout;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.NegativeTimesNotSupported, new object[0]));
				}
				lock (this.SapiRecognizer)
				{
					if (this._isRecognizing)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
					}
					this._initialSilenceTimeout = value;
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002CFE8 File Offset: 0x0002BFE8
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x0002D024 File Offset: 0x0002C024
		internal TimeSpan BabbleTimeout
		{
			get
			{
				TimeSpan babbleTimeout;
				lock (this.SapiRecognizer)
				{
					babbleTimeout = this._babbleTimeout;
				}
				return babbleTimeout;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.NegativeTimesNotSupported, new object[0]));
				}
				lock (this.SapiRecognizer)
				{
					if (this._isRecognizing)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
					}
					this._babbleTimeout = value;
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0002D0A4 File Offset: 0x0002C0A4
		internal RecognizerState State
		{
			get
			{
				RecognizerState recognizerState;
				try
				{
					SPRECOSTATE recoState = this.SapiRecognizer.GetRecoState();
					if (recoState == SPRECOSTATE.SPRST_ACTIVE || recoState == SPRECOSTATE.SPRST_ACTIVE_ALWAYS)
					{
						recognizerState = RecognizerState.Listening;
					}
					else
					{
						recognizerState = RecognizerState.Stopped;
					}
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
				return recognizerState;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0002D0E8 File Offset: 0x0002C0E8
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0002D124 File Offset: 0x0002C124
		internal bool Enabled
		{
			get
			{
				bool enabled;
				lock (this.SapiRecognizer)
				{
					enabled = this._enabled;
				}
				return enabled;
			}
			set
			{
				lock (this.SapiRecognizer)
				{
					if (value != this._enabled)
					{
						try
						{
							this.SapiContext.SetContextState(value ? SPCONTEXTSTATE.SPCS_ENABLED : SPCONTEXTSTATE.SPCS_DISABLED);
							this._enabled = value;
						}
						catch (COMException ex)
						{
							throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
						}
					}
				}
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0002D190 File Offset: 0x0002C190
		internal ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this._readOnlyGrammars;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002D198 File Offset: 0x0002C198
		internal RecognizerInfo RecognizerInfo
		{
			get
			{
				if (this._recognizerInfo == null)
				{
					try
					{
						this._recognizerInfo = this.SapiRecognizer.GetRecognizerInfo();
					}
					catch (COMException ex)
					{
						throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
					}
				}
				return this._recognizerInfo;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002D1E0 File Offset: 0x0002C1E0
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x0002D1F2 File Offset: 0x0002C1F2
		internal AudioState AudioState
		{
			get
			{
				if (!this._haveInputSource)
				{
					return AudioState.Stopped;
				}
				return this._audioState;
			}
			set
			{
				this._audioState = value;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002D1FC File Offset: 0x0002C1FC
		internal int AudioLevel
		{
			get
			{
				int num = 0;
				if (this._haveInputSource)
				{
					try
					{
						SPRECOGNIZERSTATUS status = this.SapiRecognizer.GetStatus();
						lock (this.SapiRecognizer)
						{
							if (this._supportsSapi53)
							{
								num = (int)status.AudioStatus.dwAudioLevel;
							}
							else
							{
								num = 0;
							}
						}
					}
					catch (COMException ex)
					{
						throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
					}
				}
				return num;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0002D278 File Offset: 0x0002C278
		internal TimeSpan AudioPosition
		{
			get
			{
				if (!this._haveInputSource)
				{
					return TimeSpan.Zero;
				}
				TimeSpan timeSpan;
				try
				{
					SPRECOGNIZERSTATUS status = this.SapiRecognizer.GetStatus();
					lock (this.SapiRecognizer)
					{
						SpeechAudioFormatInfo audioFormat = this.AudioFormat;
						timeSpan = ((audioFormat.AverageBytesPerSecond > 0) ? new TimeSpan((long)(status.AudioStatus.CurDevicePos * 10000000UL / (ulong)((long)audioFormat.AverageBytesPerSecond))) : TimeSpan.Zero);
					}
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
				return timeSpan;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002D314 File Offset: 0x0002C314
		internal TimeSpan RecognizerAudioPosition
		{
			get
			{
				if (!this._haveInputSource)
				{
					return TimeSpan.Zero;
				}
				TimeSpan timeSpan;
				try
				{
					SPRECOGNIZERSTATUS status = this.SapiRecognizer.GetStatus();
					lock (this.SapiRecognizer)
					{
						timeSpan = new TimeSpan((long)status.ullRecognitionStreamTime);
					}
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
				return timeSpan;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0002D384 File Offset: 0x0002C384
		internal SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				lock (this.SapiRecognizer)
				{
					if (!this._haveInputSource)
					{
						return null;
					}
					if (this._audioFormat == null)
					{
						this._audioFormat = this.GetSapiAudioFormat();
					}
				}
				return this._audioFormat;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0002D3E0 File Offset: 0x0002C3E0
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0002D3E8 File Offset: 0x0002C3E8
		internal int MaxAlternates
		{
			get
			{
				return this._maxAlternates;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.MaxAlternatesInvalid, new object[0]));
				}
				if (value != this._maxAlternates)
				{
					this.SapiContext.SetMaxAlternates((uint)value);
					this._maxAlternates = value;
				}
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000A5B RID: 2651 RVA: 0x0002D425 File Offset: 0x0002C425
		// (remove) Token: 0x06000A5C RID: 2652 RVA: 0x0002D43E File Offset: 0x0002C43E
		internal event EventHandler<RecognizeCompletedEventArgs> RecognizeCompleted;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000A5D RID: 2653 RVA: 0x0002D457 File Offset: 0x0002C457
		// (remove) Token: 0x06000A5E RID: 2654 RVA: 0x0002D470 File Offset: 0x0002C470
		internal event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000A5F RID: 2655 RVA: 0x0002D489 File Offset: 0x0002C489
		// (remove) Token: 0x06000A60 RID: 2656 RVA: 0x0002D4A2 File Offset: 0x0002C4A2
		internal event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000A61 RID: 2657 RVA: 0x0002D4BB File Offset: 0x0002C4BB
		// (remove) Token: 0x06000A62 RID: 2658 RVA: 0x0002D4D4 File Offset: 0x0002C4D4
		internal event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000A63 RID: 2659 RVA: 0x0002D4ED File Offset: 0x0002C4ED
		// (remove) Token: 0x06000A64 RID: 2660 RVA: 0x0002D506 File Offset: 0x0002C506
		internal event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000A65 RID: 2661 RVA: 0x0002D51F File Offset: 0x0002C51F
		// (remove) Token: 0x06000A66 RID: 2662 RVA: 0x0002D538 File Offset: 0x0002C538
		internal event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000A67 RID: 2663 RVA: 0x0002D551 File Offset: 0x0002C551
		// (remove) Token: 0x06000A68 RID: 2664 RVA: 0x0002D56A File Offset: 0x0002C56A
		internal event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000A69 RID: 2665 RVA: 0x0002D583 File Offset: 0x0002C583
		// (remove) Token: 0x06000A6A RID: 2666 RVA: 0x0002D5B3 File Offset: 0x0002C5B3
		internal event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized
		{
			[MethodImpl(32)]
			add
			{
				if (this._speechHypothesizedDelegate == null)
				{
					this.AddEventInterest(549755813888UL);
				}
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Combine(this._speechHypothesizedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Remove(this._speechHypothesizedDelegate, value);
				if (this._speechHypothesizedDelegate == null)
				{
					this.RemoveEventInterest(549755813888UL);
				}
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000A6B RID: 2667 RVA: 0x0002D5E3 File Offset: 0x0002C5E3
		// (remove) Token: 0x06000A6C RID: 2668 RVA: 0x0002D613 File Offset: 0x0002C613
		internal event EventHandler<AudioSignalProblemOccurredEventArgs> AudioSignalProblemOccurred
		{
			[MethodImpl(32)]
			add
			{
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.AddEventInterest(17592186044416UL);
				}
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Combine(this._audioSignalProblemOccurredDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Remove(this._audioSignalProblemOccurredDelegate, value);
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RemoveEventInterest(17592186044416UL);
				}
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000A6D RID: 2669 RVA: 0x0002D643 File Offset: 0x0002C643
		// (remove) Token: 0x06000A6E RID: 2670 RVA: 0x0002D673 File Offset: 0x0002C673
		internal event EventHandler<AudioLevelUpdatedEventArgs> AudioLevelUpdated
		{
			[MethodImpl(32)]
			add
			{
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.AddEventInterest(1125899906842624UL);
				}
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Combine(this._audioLevelUpdatedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Remove(this._audioLevelUpdatedDelegate, value);
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RemoveEventInterest(1125899906842624UL);
				}
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000A6F RID: 2671 RVA: 0x0002D6A3 File Offset: 0x0002C6A3
		// (remove) Token: 0x06000A70 RID: 2672 RVA: 0x0002D6BC File Offset: 0x0002C6BC
		internal event EventHandler<AudioStateChangedEventArgs> AudioStateChanged
		{
			[MethodImpl(32)]
			add
			{
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Combine(this._audioStateChangedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Remove(this._audioStateChangedDelegate, value);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000A71 RID: 2673 RVA: 0x0002D6D5 File Offset: 0x0002C6D5
		// (remove) Token: 0x06000A72 RID: 2674 RVA: 0x0002D6EE File Offset: 0x0002C6EE
		internal event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x06000A73 RID: 2675 RVA: 0x0002D708 File Offset: 0x0002C708
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed && disposing)
			{
				lock (this._thisObjectLock)
				{
					if (this._asyncWorkerUI != null)
					{
						this._asyncWorkerUI.Enabled = false;
						this._asyncWorkerUI.Purge();
						this._asyncWorker.Enabled = false;
						this._asyncWorker.Purge();
					}
					if (this._sapiContext != null)
					{
						this._sapiContext.DisposeEventNotify(this._eventNotify);
						this._handlerWaitHandle.Close();
						this.UnloadAllGrammars();
						this._waitForGrammarsToLoad.Dispose();
					}
					this.CloseCachedInputStream();
					if (this._sapiContext != null)
					{
						this._sapiContext.Dispose();
						this._sapiContext = null;
					}
					if (this._sapiRecognizer != null)
					{
						this._sapiRecognizer.Dispose();
						this._sapiRecognizer = null;
					}
					if (this._recognizerInfo != null)
					{
						this._recognizerInfo.Dispose();
						this._recognizerInfo = null;
					}
					this._disposed = true;
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002D814 File Offset: 0x0002C814
		private SapiRecoContext SapiContext
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("RecognizerBase");
				}
				return this._sapiContext;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0002D82F File Offset: 0x0002C82F
		private SapiRecognizer SapiRecognizer
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("RecognizerBase");
				}
				return this._sapiRecognizer;
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002D84C File Offset: 0x0002C84C
		private void LoadSapiGrammar(Grammar grammar, SapiGrammar sapiGrammar, bool enabled, float weight, int priority)
		{
			Uri uri = grammar.BaseUri;
			if (this._supportsSapi53 && uri == null && grammar.Uri != null)
			{
				string originalString = grammar.Uri.OriginalString;
				int num = originalString.LastIndexOfAny(new char[] { '\\', '/' });
				if (num >= 0)
				{
					uri = new Uri(originalString.Substring(0, num + 1), 0);
				}
			}
			if (grammar.IsDictation(grammar.Uri))
			{
				this.LoadSapiDictationGrammar(sapiGrammar, grammar.Uri, grammar.RuleName, enabled, weight, priority);
				return;
			}
			this.LoadSapiGrammarFromCfg(sapiGrammar, grammar, uri, enabled, weight, priority);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002D8F0 File Offset: 0x0002C8F0
		private void LoadSapiDictationGrammar(SapiGrammar sapiGrammar, Uri uri, string ruleName, bool enabled, float weight, int priority)
		{
			try
			{
				if (Grammar.IsDictationGrammar(uri))
				{
					string text = (string.IsNullOrEmpty(uri.Fragment) ? null : uri.Fragment.Substring(1, uri.Fragment.Length - 1));
					sapiGrammar.LoadDictation(text, SPLOADOPTIONS.SPLO_STATIC);
				}
			}
			catch (COMException ex)
			{
				SAPIErrorCodes errorCode = (SAPIErrorCodes)ex.ErrorCode;
				if (errorCode == SAPIErrorCodes.SPERR_NOT_FOUND)
				{
					throw new ArgumentException(SR.Get(SRID.DictationTopicNotFound, new object[] { uri }), ex);
				}
				RecognizerBase.ThrowIfSapiErrorCode((SAPIErrorCodes)ex.ErrorCode);
				throw;
			}
			this.SetSapiGrammarProperties(sapiGrammar, uri, ruleName, enabled, weight, priority);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0002D994 File Offset: 0x0002C994
		int ISpGrammarResourceLoader.LoadResource(string bstrResourceUri, bool fAlwaysReload, out IStream pStream, ref string pbstrMIMEType, ref short pfModified, ref string pbstrRedirectUrl)
		{
			int num4;
			try
			{
				int num = bstrResourceUri.IndexOf('>');
				string text = null;
				if (num > 0)
				{
					text = bstrResourceUri.Substring(num + 1);
					bstrResourceUri = bstrResourceUri.Substring(0, num);
				}
				string text2 = pbstrMIMEType;
				string[] array = pbstrRedirectUrl.Split(new char[] { ' ' }, 0);
				uint num2 = uint.Parse(array[0], CultureInfo.InvariantCulture);
				uint num3 = uint.Parse(array[1], CultureInfo.InvariantCulture);
				Uri uri;
				Grammar grammar = Grammar.Create(bstrResourceUri, text2, text, out uri);
				if (uri != null)
				{
					pbstrRedirectUrl = uri.ToString();
				}
				if (grammar == null)
				{
					throw new FormatException(SR.Get(SRID.SapiErrorRuleNotFound2, new object[] { text2, bstrResourceUri }));
				}
				grammar.SapiGrammarId = num3;
				Grammar grammar2 = this._topLevel.Find((long)((ulong)num2));
				if (grammar2 == null)
				{
					this._topLevel.AddRuleRef(grammar, num3);
				}
				else
				{
					grammar2.AddRuleRef(grammar, num3);
				}
				MemoryStream memoryStream = new MemoryStream(grammar.CfgData);
				SpStreamWrapper spStreamWrapper = new SpStreamWrapper(memoryStream);
				pStream = spStreamWrapper;
				pfModified = 0;
				num4 = 0;
			}
			catch (Exception ex)
			{
				pStream = null;
				this._loadException = ex;
				num4 = -2147200988;
			}
			return num4;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0002DADC File Offset: 0x0002CADC
		string ISpGrammarResourceLoader.GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl)
		{
			redirectUrl = null;
			mimeType = null;
			return null;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002DAE5 File Offset: 0x0002CAE5
		void ISpGrammarResourceLoader.ReleaseLocalCopy(string path)
		{
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002DAE8 File Offset: 0x0002CAE8
		private void LoadSapiGrammarFromCfg(SapiGrammar sapiGrammar, Grammar grammar, Uri baseUri, bool enabled, float weight, int priority)
		{
			byte[] cfgData = grammar.CfgData;
			GCHandle gchandle = GCHandle.Alloc(cfgData, 3);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			try
			{
				if (this._supportsSapi53)
				{
					this._loadException = null;
					this._topLevel = grammar;
					if (this._inproc)
					{
						sapiGrammar.SetGrammarLoader(this._recoThunk);
					}
					sapiGrammar.LoadCmdFromMemory2(intPtr, SPLOADOPTIONS.SPLO_STATIC, null, (baseUri == null) ? null : baseUri.ToString());
				}
				else
				{
					sapiGrammar.LoadCmdFromMemory(intPtr, SPLOADOPTIONS.SPLO_STATIC);
				}
			}
			catch (COMException ex)
			{
				SAPIErrorCodes errorCode = (SAPIErrorCodes)ex.ErrorCode;
				if (errorCode <= SAPIErrorCodes.SPERR_INVALID_IMPORT)
				{
					if (errorCode == SAPIErrorCodes.SPERR_UNSUPPORTED_FORMAT)
					{
						throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]), ex);
					}
					switch (errorCode)
					{
					case SAPIErrorCodes.SPERR_TOO_MANY_GRAMMARS:
						throw new NotSupportedException(SR.Get(SRID.SapiErrorTooManyGrammars, new object[0]), ex);
					case SAPIErrorCodes.SPERR_INVALID_IMPORT:
						throw new FormatException(SR.Get(SRID.SapiErrorInvalidImport, new object[0]), ex);
					}
				}
				else
				{
					if (errorCode == SAPIErrorCodes.SPERR_NOT_FOUND)
					{
						throw new FileNotFoundException(SR.Get(SRID.ReferencedGrammarNotFound, new object[0]), ex);
					}
					if (errorCode == (SAPIErrorCodes)(-1))
					{
						if (this._loadException != null)
						{
							throw this._loadException;
						}
						RecognizerBase.ThrowIfSapiErrorCode((SAPIErrorCodes)ex.ErrorCode);
						goto IL_137;
					}
				}
				RecognizerBase.ThrowIfSapiErrorCode((SAPIErrorCodes)ex.ErrorCode);
				IL_137:
				throw;
			}
			catch (ArgumentException ex2)
			{
				throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]), ex2);
			}
			finally
			{
				gchandle.Free();
			}
			this.SetSapiGrammarProperties(sapiGrammar, null, grammar.RuleName, enabled, weight, priority);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002DCB4 File Offset: 0x0002CCB4
		private void SetSapiGrammarProperties(SapiGrammar sapiGrammar, Uri uri, string ruleName, bool enabled, float weight, int priority)
		{
			if (!enabled)
			{
				sapiGrammar.SetGrammarState(SPGRAMMARSTATE.SPGS_DISABLED);
			}
			if (this._supportsSapi53)
			{
				if (priority != 0)
				{
					if (Grammar.IsDictationGrammar(uri))
					{
						throw new NotSupportedException(SR.Get(SRID.CannotSetPriorityOnDictation, new object[0]));
					}
					sapiGrammar.SetRulePriority(ruleName, 0U, priority);
				}
				if (!weight.Equals(1f))
				{
					if (Grammar.IsDictationGrammar(uri))
					{
						sapiGrammar.SetDictationWeight(weight);
					}
					else
					{
						sapiGrammar.SetRuleWeight(ruleName, 0U, weight);
					}
				}
			}
			else if (priority != 0 || !weight.Equals(1f))
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			this.ActivateRule(sapiGrammar, uri, ruleName);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002DD5C File Offset: 0x0002CD5C
		private void LoadGrammarAsyncCallback(object grammarObject)
		{
			Grammar grammar = (Grammar)grammarObject;
			InternalGrammarData internalData = grammar.InternalData;
			Exception ex = null;
			try
			{
				lock (this._grammarDataLock)
				{
					this.LoadSapiGrammar(grammar, internalData._sapiGrammar, internalData._grammarEnabled, internalData._grammarWeight, internalData._grammarPriority);
					grammar.State = GrammarState.Loaded;
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				if (ex != null)
				{
					grammar.State = GrammarState.LoadFailed;
					grammar.LoadException = ex;
				}
				this._waitForGrammarsToLoad.FinishOperation();
				this._asyncWorkerUI.PostOperation(new WaitCallback(this.LoadGrammarAsyncCompletedCallback), new object[] { grammarObject });
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0002DE2C File Offset: 0x0002CE2C
		private void LoadGrammarAsyncCompletedCallback(object grammarObject)
		{
			Grammar grammar = (Grammar)grammarObject;
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted.Invoke(this, new LoadGrammarCompletedEventArgs(grammar, grammar.LoadException, false, null));
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0002DE60 File Offset: 0x0002CE60
		private SapiGrammar CreateNewSapiGrammar(out ulong grammarId)
		{
			ulong currentGrammarId = this._currentGrammarId;
			for (;;)
			{
				this._currentGrammarId += 1UL;
				bool flag = false;
				lock (this.SapiRecognizer)
				{
					foreach (Grammar grammar in this._grammars)
					{
						if (this._currentGrammarId == grammar.InternalData._grammarId)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					break;
				}
				if (this._currentGrammarId == currentGrammarId)
				{
					goto Block_3;
				}
			}
			SapiGrammar sapiGrammar = this.SapiContext.CreateGrammar(this._currentGrammarId);
			grammarId = this._currentGrammarId;
			return sapiGrammar;
			Block_3:
			throw new InvalidOperationException(SR.Get(SRID.SapiErrorTooManyGrammars, new object[0]));
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002DF40 File Offset: 0x0002CF40
		private void ValidateGrammar(Grammar grammar, params GrammarState[] validStates)
		{
			Helpers.ThrowIfNull(grammar, "grammar");
			int i = 0;
			while (i < validStates.Length)
			{
				GrammarState grammarState = validStates[i];
				if (grammar.State == grammarState)
				{
					if (grammar.State != GrammarState.Unloaded && grammar.Recognizer != this)
					{
						throw new InvalidOperationException(SR.Get(SRID.GrammarWrongRecognizer, new object[0]));
					}
					return;
				}
				else
				{
					i++;
				}
			}
			switch (grammar.State)
			{
			case GrammarState.Unloaded:
				throw new InvalidOperationException(SR.Get(SRID.GrammarNotLoaded, new object[0]));
			case GrammarState.Loading:
				throw new InvalidOperationException(SR.Get(SRID.GrammarLoadingInProgress, new object[0]));
			case GrammarState.Loaded:
				throw new InvalidOperationException(SR.Get(SRID.GrammarAlreadyLoaded, new object[0]));
			case GrammarState.LoadFailed:
				throw new InvalidOperationException(SR.Get(SRID.GrammarLoadFailed, new object[0]));
			default:
				return;
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002E034 File Offset: 0x0002D034
		private RecognitionResult InternalEmulateRecognize(string phrase, SpeechEmulationCompareFlags flag, bool useReco2, RecognizedWordUnit[] wordUnits)
		{
			RecognitionResult result = null;
			bool completed = false;
			EventHandler<EmulateRecognizeCompletedEventArgs> eventHandler = delegate(object sender, EmulateRecognizeCompletedEventArgs eventArgs)
			{
				result = eventArgs.Result;
				completed = true;
			};
			this.EmulateRecognizeCompletedSync = (EventHandler<EmulateRecognizeCompletedEventArgs>)Delegate.Combine(this.EmulateRecognizeCompletedSync, eventHandler);
			try
			{
				this._asyncWorkerUI.AsyncMode = false;
				this.InternalEmulateRecognizeAsync(phrase, flag, useReco2, wordUnits);
				do
				{
					this._handlerWaitHandle.WaitOne();
					this._asyncWorkerUI.ConsumeQueue();
				}
				while (!completed && !this._disposed);
			}
			finally
			{
				this.EmulateRecognizeCompletedSync = (EventHandler<EmulateRecognizeCompletedEventArgs>)Delegate.Remove(this.EmulateRecognizeCompletedSync, eventHandler);
				this._asyncWorkerUI.AsyncMode = true;
			}
			return result;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002E0F4 File Offset: 0x0002D0F4
		private void InternalEmulateRecognizeAsync(string phrase, SpeechEmulationCompareFlags flag, bool useReco2, RecognizedWordUnit[] wordUnits)
		{
			lock (this.SapiRecognizer)
			{
				if (this._isRecognizing)
				{
					throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
				}
				this._isRecognizing = true;
				this._isEmulateRecognition = true;
			}
			if (useReco2 || this._supportsSapi53)
			{
				GCHandle[] array = null;
				IntPtr intPtr;
				ISpPhrase spPhrase;
				if (wordUnits == null)
				{
					spPhrase = SPPHRASE.CreatePhraseFromText(phrase.Trim(), this.RecognizerInfo.Culture, out array, out intPtr);
				}
				else
				{
					spPhrase = SPPHRASE.CreatePhraseFromWordUnits(wordUnits, this.RecognizerInfo.Culture, out array, out intPtr);
				}
				try
				{
					SAPIErrorCodes sapierrorCodes = this.SapiRecognizer.EmulateRecognition(spPhrase, (uint)flag);
					if (sapierrorCodes != SAPIErrorCodes.S_OK)
					{
						this.EmulateRecognizedFailReportError(sapierrorCodes);
					}
					return;
				}
				finally
				{
					foreach (GCHandle gchandle in array)
					{
						gchandle.Free();
					}
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			SAPIErrorCodes sapierrorCodes2 = this.SapiRecognizer.EmulateRecognition(phrase);
			if (sapierrorCodes2 != SAPIErrorCodes.S_OK)
			{
				this.EmulateRecognizedFailReportError(sapierrorCodes2);
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002E214 File Offset: 0x0002D214
		private void EmulateRecognizedFailReportError(SAPIErrorCodes hr)
		{
			this._lastException = RecognizerBase.ExceptionFromSapiCreateRecognizerError(hr);
			if (hr < SAPIErrorCodes.S_OK || hr == SAPIErrorCodes.SP_NO_RULE_ACTIVE)
			{
				this.FireEmulateRecognizeCompletedEvent(null, this._lastException, true);
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0002E23C File Offset: 0x0002D23C
		private void ActivateRule(SapiGrammar sapiGrammar, Uri uri, string ruleName)
		{
			SPRULESTATE sprulestate = (this._pauseRecognizerOnRecognition ? SPRULESTATE.SPRS_ACTIVE_WITH_AUTO_PAUSE : SPRULESTATE.SPRS_ACTIVE);
			SAPIErrorCodes sapierrorCodes;
			if (Grammar.IsDictationGrammar(uri))
			{
				sapierrorCodes = sapiGrammar.SetDictationState(sprulestate);
			}
			else
			{
				sapierrorCodes = sapiGrammar.SetRuleState(ruleName, sprulestate);
			}
			if (sapierrorCodes == SAPIErrorCodes.SPERR_NOT_TOPLEVEL_RULE || sapierrorCodes == SAPIErrorCodes.SP_NO_RULES_TO_ACTIVATE)
			{
				if (uri == null)
				{
					if (string.IsNullOrEmpty(ruleName))
					{
						throw new FormatException(SR.Get(SRID.RecognizerNoRootRuleToActivate, new object[0]));
					}
					throw new ArgumentException(SR.Get(SRID.RecognizerRuleNotFoundStream, new object[] { ruleName }), "ruleName");
				}
				else
				{
					if (string.IsNullOrEmpty(ruleName))
					{
						throw new FormatException(SR.Get(SRID.RecognizerNoRootRuleToActivate1, new object[] { uri }));
					}
					throw new ArgumentException(SR.Get(SRID.RecognizerRuleNotFound, new object[] { ruleName, uri }), "ruleName");
				}
			}
			else
			{
				if (sapierrorCodes != SAPIErrorCodes.SPERR_AUDIO_NOT_FOUND && sapierrorCodes < SAPIErrorCodes.S_OK)
				{
					RecognizerBase.ThrowIfSapiErrorCode(sapierrorCodes);
					throw new COMException(SR.Get(SRID.RecognizerRuleActivationFailed, new object[0]), (int)sapierrorCodes);
				}
				return;
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002E344 File Offset: 0x0002D344
		private void RecognizeAsyncWaitForGrammarsToLoad(object unused)
		{
			this._waitForGrammarsToLoad.WaitForOperationsToFinish();
			Exception ex = null;
			bool flag = false;
			lock (this.SapiRecognizer)
			{
				foreach (Grammar grammar in this._grammars)
				{
					if (grammar.State == GrammarState.LoadFailed)
					{
						ex = grammar.LoadException;
						break;
					}
				}
				if (this._isRecognizeCancelled)
				{
					flag = true;
				}
			}
			if (ex == null && !flag)
			{
				try
				{
					if (!this._isEmulateRecognition)
					{
						this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_ACTIVE_ALWAYS);
					}
				}
				catch (COMException ex2)
				{
					ex = RecognizerBase.ExceptionFromSapiStreamError((SAPIErrorCodes)ex2.ErrorCode);
				}
				catch (Exception ex3)
				{
					ex = ex3;
				}
			}
			if (ex != null || flag)
			{
				RecognizeCompletedEventArgs recognizeCompletedEventArgs = new RecognizeCompletedEventArgs(null, false, false, false, TimeSpan.Zero, ex, flag, null);
				this._asyncWorkerUI.PostOperation(new WaitCallback(this.RecognizeAsyncWaitForGrammarsToLoadFailed), new object[] { recognizeCompletedEventArgs });
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0002E46C File Offset: 0x0002D46C
		private void RecognizeAsyncWaitForGrammarsToLoadFailed(object eventArgs)
		{
			lock (this.SapiRecognizer)
			{
				this._isRecognizeCancelled = false;
				this._isRecognizing = false;
			}
			EventHandler<RecognizeCompletedEventArgs> recognizeCompleted = this.RecognizeCompleted;
			if (recognizeCompleted != null)
			{
				recognizeCompleted.Invoke(this, (RecognizeCompletedEventArgs)eventArgs);
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002E4C4 File Offset: 0x0002D4C4
		private void SignalHandlerThread(object ignored)
		{
			if (!this._asyncWorkerUI.AsyncMode)
			{
				this._handlerWaitHandle.Set();
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0002E4E0 File Offset: 0x0002D4E0
		private void DispatchEvents(object eventData)
		{
			lock (this._thisObjectLock)
			{
				SpeechEvent speechEvent = eventData as SpeechEvent;
				if (!this._disposed && eventData != null)
				{
					switch (speechEvent.EventId)
					{
					case SPEVENTENUM.SPEI_END_SR_STREAM:
						this.ProcessEndStreamEvent(speechEvent);
						break;
					case SPEVENTENUM.SPEI_PHRASE_START:
						this.ProcessPhraseStartEvent(speechEvent);
						break;
					case SPEVENTENUM.SPEI_RECOGNITION:
					case SPEVENTENUM.SPEI_FALSE_RECOGNITION:
						this.ProcessRecognitionEvent(speechEvent);
						break;
					case SPEVENTENUM.SPEI_HYPOTHESIS:
						this.ProcessHypothesisEvent(speechEvent);
						break;
					case SPEVENTENUM.SPEI_SR_BOOKMARK:
						this.ProcessBookmarkEvent(speechEvent);
						break;
					case SPEVENTENUM.SPEI_INTERFERENCE:
						this.ProcessInterferenceEvent((uint)speechEvent.LParam);
						break;
					case SPEVENTENUM.SPEI_START_SR_STREAM:
						this.ProcessStartStreamEvent();
						break;
					case SPEVENTENUM.SPEI_RECO_OTHER_CONTEXT:
						this.ProcessRecoOtherContextEvent();
						break;
					case SPEVENTENUM.SPEI_SR_AUDIO_LEVEL:
						this.ProcessAudioLevelEvent((int)speechEvent.WParam);
						break;
					}
				}
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0002E5DC File Offset: 0x0002D5DC
		private void ProcessStartStreamEvent()
		{
			lock (this.SapiRecognizer)
			{
				this._audioState = AudioState.Silence;
			}
			this.FireAudioStateChangedEvent(this._audioState);
			this.FireStateChangedEvent(RecognizerState.Listening);
			TimeSpan initialSilenceTimeout = this.InitialSilenceTimeout;
			if (this._recognizeMode == RecognizeMode.Single && initialSilenceTimeout != TimeSpan.Zero)
			{
				if (this._supportsSapi53)
				{
					this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_PAUSE | SPBOOKMARKOPTIONS.SPBO_TIME_UNITS, (ulong)initialSilenceTimeout.Ticks, new IntPtr(1));
				}
				else
				{
					this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_PAUSE, this.TimeSpanToStreamPosition(initialSilenceTimeout), new IntPtr(1));
				}
				this._detectingInitialSilenceTimeout = true;
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0002E688 File Offset: 0x0002D688
		private void ProcessPhraseStartEvent(SpeechEvent speechEvent)
		{
			this._isWaitingForRecognition = true;
			lock (this.SapiRecognizer)
			{
				this._audioState = AudioState.Speech;
			}
			this.FireAudioStateChangedEvent(this._audioState);
			this._detectingInitialSilenceTimeout = false;
			TimeSpan babbleTimeout = this.BabbleTimeout;
			if (this._recognizeMode == RecognizeMode.Single && babbleTimeout != TimeSpan.Zero)
			{
				if (this._supportsSapi53)
				{
					this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_TIME_UNITS, (ulong)(babbleTimeout + speechEvent.AudioPosition).Ticks, new IntPtr(2));
				}
				else
				{
					this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_NONE, this.TimeSpanToStreamPosition(babbleTimeout) + speechEvent.AudioStreamOffset, new IntPtr(2));
				}
				this._detectingBabbleTimeout = true;
			}
			this.FireSpeechDetectedEvent(speechEvent.AudioPosition);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002E75C File Offset: 0x0002D75C
		private void ProcessBookmarkEvent(SpeechEvent speechEvent)
		{
			uint num = (uint)speechEvent.LParam;
			try
			{
				if (num == 1U)
				{
					if (this._detectingInitialSilenceTimeout)
					{
						this.EndRecognitionWithTimeout();
					}
				}
				else if (num == 2U)
				{
					if (this._detectingBabbleTimeout && !this._initialSilenceTimeoutReached)
					{
						this._babbleTimeoutReached = true;
						this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE_WITH_PURGE);
					}
				}
				else
				{
					object bookmarkItemAndRemove = this.GetBookmarkItemAndRemove(num);
					EventHandler<RecognizerUpdateReachedEventArgs> recognizerUpdateReached = this.RecognizerUpdateReached;
					if (recognizerUpdateReached != null)
					{
						recognizerUpdateReached.Invoke(this, new RecognizerUpdateReachedEventArgs(bookmarkItemAndRemove, speechEvent.AudioPosition));
					}
				}
			}
			catch (COMException ex)
			{
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
			}
			finally
			{
				if (((int)speechEvent.WParam & 1) != 0)
				{
					this.SapiContext.Resume();
				}
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002E814 File Offset: 0x0002D814
		private void ProcessHypothesisEvent(SpeechEvent speechEvent)
		{
			RecognitionResult recognitionResult = this.CreateRecognitionResult(speechEvent);
			bool enabled;
			lock (this.SapiRecognizer)
			{
				enabled = this._enabled;
			}
			if (recognitionResult.Grammar != null && recognitionResult.Grammar.Enabled && enabled)
			{
				this.FireSpeechHypothesizedEvent(recognitionResult);
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0002E878 File Offset: 0x0002D878
		private void ProcessRecognitionEvent(SpeechEvent speechEvent)
		{
			this._detectingInitialSilenceTimeout = false;
			this._detectingBabbleTimeout = false;
			bool flag = true;
			bool flag2 = (speechEvent.WParam & 2UL) != 0UL;
			try
			{
				RecognitionResult recognitionResult = this.CreateRecognitionResult(speechEvent);
				bool enabled;
				lock (this.SapiRecognizer)
				{
					this._audioState = AudioState.Silence;
					flag = this._isRecognizeCancelled;
					enabled = this._enabled;
				}
				this.FireAudioStateChangedEvent(this._audioState);
				if (((recognitionResult.Grammar != null && recognitionResult.Grammar.Enabled) || (speechEvent.EventId == SPEVENTENUM.SPEI_FALSE_RECOGNITION && recognitionResult.GrammarId == 0UL)) && enabled)
				{
					if (speechEvent.EventId == SPEVENTENUM.SPEI_RECOGNITION)
					{
						this._lastResult = recognitionResult;
						SpeechRecognizedEventArgs speechRecognizedEventArgs = new SpeechRecognizedEventArgs(recognitionResult);
						recognitionResult.Grammar.OnRecognitionInternal(speechRecognizedEventArgs);
						this.FireSpeechRecognizedEvent(speechRecognizedEventArgs);
					}
					else
					{
						this._lastResult = null;
						if (recognitionResult.GrammarId != 0UL || (!this._babbleTimeoutReached && !flag))
						{
							this.FireSpeechRecognitionRejectedEvent(recognitionResult);
						}
					}
				}
			}
			finally
			{
				if (this._recognizeMode == RecognizeMode.Single)
				{
					try
					{
						this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE_WITH_PURGE);
					}
					catch (COMException ex)
					{
						throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
					}
				}
				if (((int)speechEvent.WParam & 1) != 0)
				{
					this.SapiContext.Resume();
				}
			}
			if (this._inproc || flag2)
			{
				this._isWaitingForRecognition = false;
			}
			if (flag2 && !this._inproc)
			{
				this.FireEmulateRecognizeCompletedEvent(this._lastResult, this._lastException, flag);
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002E9F8 File Offset: 0x0002D9F8
		private void ProcessRecoOtherContextEvent()
		{
			if (this._isEmulateRecognition && !this._inproc)
			{
				this.FireEmulateRecognizeCompletedEvent(this._lastResult, this._lastException, false);
			}
			lock (this.SapiRecognizer)
			{
				this._audioState = AudioState.Silence;
			}
			this.FireAudioStateChangedEvent(this._audioState);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002EA64 File Offset: 0x0002DA64
		private void ProcessEndStreamEvent(SpeechEvent speechEvent)
		{
			if (!this._supportsSapi53 && this._isEmulateRecognition && this._isWaitingForRecognition)
			{
				return;
			}
			if (((int)speechEvent.WParam & 2) == 0)
			{
				this.ResetBookmarkTable();
			}
			bool initialSilenceTimeoutReached = this._initialSilenceTimeoutReached;
			bool babbleTimeoutReached = this._babbleTimeoutReached;
			RecognitionResult lastResult = this._lastResult;
			Exception lastException = this._lastException;
			this._initialSilenceTimeoutReached = false;
			this._babbleTimeoutReached = false;
			this._detectingInitialSilenceTimeout = false;
			this._detectingBabbleTimeout = false;
			this._lastResult = null;
			this._lastException = null;
			bool flag = false;
			bool isRecognizeCancelled;
			lock (this.SapiRecognizer)
			{
				this._audioState = AudioState.Stopped;
				if (((int)speechEvent.WParam & 1) == 1)
				{
					flag = true;
					this._haveInputSource = false;
				}
				isRecognizeCancelled = this._isRecognizeCancelled;
				this._isRecognizeCancelled = false;
				this._isRecognizing = false;
			}
			this.FireAudioStateChangedEvent(this._audioState);
			if (!this._isEmulateRecognition)
			{
				this.FireRecognizeCompletedEvent(lastResult, initialSilenceTimeoutReached, babbleTimeoutReached, flag, speechEvent.AudioPosition, (speechEvent.LParam == 0UL) ? null : RecognizerBase.ExceptionFromSapiStreamError((SAPIErrorCodes)speechEvent.LParam), isRecognizeCancelled);
			}
			else
			{
				this.FireEmulateRecognizeCompletedEvent(lastResult, (speechEvent.LParam == 0UL) ? lastException : RecognizerBase.ExceptionFromSapiStreamError((SAPIErrorCodes)speechEvent.LParam), isRecognizeCancelled);
			}
			this.FireStateChangedEvent(RecognizerState.Stopped);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002EBAC File Offset: 0x0002DBAC
		private void ProcessInterferenceEvent(uint interference)
		{
			this.FireSignalProblemOccurredEvent((AudioSignalProblem)interference);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002EBB5 File Offset: 0x0002DBB5
		private void ProcessAudioLevelEvent(int audioLevel)
		{
			this.FireAudioLevelUpdatedEvent(audioLevel);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002EBBE File Offset: 0x0002DBBE
		private void EndRecognitionWithTimeout()
		{
			this._initialSilenceTimeoutReached = true;
			this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE_WITH_PURGE);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002EBD4 File Offset: 0x0002DBD4
		private RecognitionResult CreateRecognitionResult(SpeechEvent speechEvent)
		{
			ISpRecoResult spRecoResult = (ISpRecoResult)Marshal.GetObjectForIUnknown((IntPtr)((long)speechEvent.LParam));
			IntPtr intPtr;
			spRecoResult.Serialize(out intPtr);
			byte[] array = null;
			try
			{
				uint num = (uint)Marshal.ReadInt32(intPtr);
				array = new byte[num];
				Marshal.Copy(intPtr, array, 0, (int)num);
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return new RecognitionResult(this, spRecoResult, array, this.MaxAlternates);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002EC48 File Offset: 0x0002DC48
		private void UpdateAudioFormat(SpeechAudioFormatInfo audioFormat)
		{
			lock (this.SapiRecognizer)
			{
				try
				{
					this._audioFormat = this.GetSapiAudioFormat();
				}
				catch (ArgumentException)
				{
					this._audioFormat = audioFormat;
				}
				this._eventNotify.AudioFormat = this._audioFormat;
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002ECB0 File Offset: 0x0002DCB0
		private SpeechAudioFormatInfo GetSapiAudioFormat()
		{
			IntPtr intPtr = IntPtr.Zero;
			SpeechAudioFormatInfo speechAudioFormatInfo = null;
			bool flag = false;
			try
			{
				try
				{
					intPtr = this.SapiRecognizer.GetFormat(SPSTREAMFORMATTYPE.SPWF_SRENGINE);
					if (intPtr != IntPtr.Zero && (speechAudioFormatInfo = AudioFormatConverter.ToSpeechAudioFormatInfo(intPtr)) != null)
					{
						flag = true;
					}
				}
				catch (COMException)
				{
					flag = false;
				}
				if (!flag)
				{
					speechAudioFormatInfo = new SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return speechAudioFormatInfo;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002ED38 File Offset: 0x0002DD38
		private ulong TimeSpanToStreamPosition(TimeSpan time)
		{
			return (ulong)(time.Ticks * (long)this.AudioFormat.AverageBytesPerSecond / 10000000L);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002ED58 File Offset: 0x0002DD58
		private static void ThrowIfSapiErrorCode(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (srid >= SRID.NullParamIllegal)
			{
				throw new InvalidOperationException(SR.Get(srid, new object[0]));
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002ED84 File Offset: 0x0002DD84
		private static Exception ExceptionFromSapiStreamError(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (srid >= SRID.NullParamIllegal)
			{
				return new InvalidOperationException(SR.Get(srid, new object[0]));
			}
			return new COMException(SR.Get(SRID.AudioDeviceInternalError, new object[0]), (int)errorCode);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002EDC4 File Offset: 0x0002DDC4
		private static SpeechEmulationCompareFlags ConvertCompareOptions(CompareOptions compareOptions)
		{
			CompareOptions compareOptions2 = 1342177305;
			SpeechEmulationCompareFlags speechEmulationCompareFlags = (SpeechEmulationCompareFlags)0;
			if ((compareOptions & 1) != null || (compareOptions & 268435456) != null)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreCase;
			}
			if ((compareOptions & 8) != null)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreKanaType;
			}
			if ((compareOptions & 16) != null)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreWidth;
			}
			if ((compareOptions & ~(compareOptions2 != null)) != null)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			return speechEmulationCompareFlags;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002EE20 File Offset: 0x0002DE20
		internal void AddEventInterest(ulong interest)
		{
			if ((this._eventInterest & interest) != interest)
			{
				this._eventInterest |= interest;
				this.SapiContext.SetInterest(this._eventInterest, this._eventInterest);
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002EE52 File Offset: 0x0002DE52
		internal void RemoveEventInterest(ulong interest)
		{
			if ((this._eventInterest & interest) != 0UL)
			{
				this._eventInterest &= ~interest;
				this.SapiContext.SetInterest(this._eventInterest, this._eventInterest);
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002EE88 File Offset: 0x0002DE88
		private uint AddBookmarkItem(object userToken)
		{
			uint num = 0U;
			if (userToken != null)
			{
				lock (this._bookmarkTable)
				{
					num = this._nextBookmarkId++;
					if (this._nextBookmarkId == 0U)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerUpdateTableTooLarge, new object[0]));
					}
					this._bookmarkTable[(int)num] = userToken;
				}
			}
			return num;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002EF00 File Offset: 0x0002DF00
		private void ResetBookmarkTable()
		{
			lock (this._bookmarkTable)
			{
				if (this._bookmarkTable.Count > 0)
				{
					int[] array = new int[this._bookmarkTable.Count];
					this._bookmarkTable.Keys.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						if ((long)array[i] <= (long)((ulong)this._prevMaxBookmarkId))
						{
							this._bookmarkTable.Remove(array[i]);
						}
					}
				}
				if (this._bookmarkTable.Count == 0)
				{
					this._nextBookmarkId = 3U;
					this._prevMaxBookmarkId = 2U;
				}
				else
				{
					this._prevMaxBookmarkId = this._nextBookmarkId - 1U;
				}
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002EFBC File Offset: 0x0002DFBC
		private object GetBookmarkItemAndRemove(uint bookmarkId)
		{
			object obj = null;
			if (bookmarkId != 0U)
			{
				lock (this._bookmarkTable)
				{
					obj = this._bookmarkTable[(int)bookmarkId];
					this._bookmarkTable.Remove((int)bookmarkId);
				}
			}
			return obj;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0002F014 File Offset: 0x0002E014
		private void CloseCachedInputStream()
		{
			if (this._inputStream != null)
			{
				this._inputStream.Close();
				this._inputStream = null;
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002F030 File Offset: 0x0002E030
		private void FireAudioStateChangedEvent(AudioState audioState)
		{
			EventHandler<AudioStateChangedEventArgs> audioStateChangedDelegate = this._audioStateChangedDelegate;
			if (audioStateChangedDelegate != null)
			{
				this._asyncWorkerUI.PostOperation(audioStateChangedDelegate, new object[]
				{
					this,
					new AudioStateChangedEventArgs(audioState)
				});
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002F068 File Offset: 0x0002E068
		private void FireSignalProblemOccurredEvent(AudioSignalProblem audioSignalProblem)
		{
			EventHandler<AudioSignalProblemOccurredEventArgs> audioSignalProblemOccurredDelegate = this._audioSignalProblemOccurredDelegate;
			if (audioSignalProblemOccurredDelegate != null)
			{
				TimeSpan timeSpan = TimeSpan.Zero;
				TimeSpan timeSpan2 = TimeSpan.Zero;
				try
				{
					SPRECOGNIZERSTATUS status = this.SapiRecognizer.GetStatus();
					lock (this.SapiRecognizer)
					{
						SpeechAudioFormatInfo audioFormat = this.AudioFormat;
						timeSpan2 = ((audioFormat.AverageBytesPerSecond > 0) ? new TimeSpan((long)(status.AudioStatus.CurDevicePos * 10000000UL / (ulong)((long)audioFormat.AverageBytesPerSecond))) : TimeSpan.Zero);
						timeSpan = new TimeSpan((long)status.ullRecognitionStreamTime);
					}
				}
				catch (COMException ex)
				{
					throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
				}
				this._asyncWorkerUI.PostOperation(audioSignalProblemOccurredDelegate, new object[]
				{
					this,
					new AudioSignalProblemOccurredEventArgs(audioSignalProblem, this.AudioLevel, timeSpan2, timeSpan)
				});
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002F150 File Offset: 0x0002E150
		private void FireAudioLevelUpdatedEvent(int audioLevel)
		{
			EventHandler<AudioLevelUpdatedEventArgs> audioLevelUpdatedDelegate = this._audioLevelUpdatedDelegate;
			if (audioLevelUpdatedDelegate != null)
			{
				this._asyncWorkerUI.PostOperation(audioLevelUpdatedDelegate, new object[]
				{
					this,
					new AudioLevelUpdatedEventArgs(audioLevel)
				});
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002F188 File Offset: 0x0002E188
		private void FireStateChangedEvent(RecognizerState recognizerState)
		{
			EventHandler<StateChangedEventArgs> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				this._asyncWorkerUI.PostOperation(stateChanged, new object[]
				{
					this,
					new StateChangedEventArgs(recognizerState)
				});
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002F1C0 File Offset: 0x0002E1C0
		private void FireSpeechDetectedEvent(TimeSpan audioPosition)
		{
			EventHandler<SpeechDetectedEventArgs> speechDetected = this.SpeechDetected;
			if (speechDetected != null)
			{
				this._asyncWorkerUI.PostOperation(speechDetected, new object[]
				{
					this,
					new SpeechDetectedEventArgs(audioPosition)
				});
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002F1F8 File Offset: 0x0002E1F8
		private void FireSpeechHypothesizedEvent(RecognitionResult result)
		{
			EventHandler<SpeechHypothesizedEventArgs> speechHypothesizedDelegate = this._speechHypothesizedDelegate;
			if (speechHypothesizedDelegate != null)
			{
				this._asyncWorkerUI.PostOperation(speechHypothesizedDelegate, new object[]
				{
					this,
					new SpeechHypothesizedEventArgs(result)
				});
			}
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002F230 File Offset: 0x0002E230
		private void FireSpeechRecognitionRejectedEvent(RecognitionResult result)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			SpeechRecognitionRejectedEventArgs speechRecognitionRejectedEventArgs = new SpeechRecognitionRejectedEventArgs(result);
			if (speechRecognitionRejected != null)
			{
				this._asyncWorkerUI.PostOperation(speechRecognitionRejected, new object[] { this, speechRecognitionRejectedEventArgs });
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002F26C File Offset: 0x0002E26C
		private void FireSpeechRecognizedEvent(SpeechRecognizedEventArgs recognitionEventArgs)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				this._asyncWorkerUI.PostOperation(speechRecognized, new object[] { this, recognitionEventArgs });
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002F2A0 File Offset: 0x0002E2A0
		private void FireRecognizeCompletedEvent(RecognitionResult result, bool initialSilenceTimeoutReached, bool babbleTimeoutReached, bool isStreamReleased, TimeSpan audioPosition, Exception exception, bool isRecognizeCancelled)
		{
			EventHandler<RecognizeCompletedEventArgs> eventHandler = this.RecognizeCompletedSync;
			if (eventHandler == null)
			{
				eventHandler = this.RecognizeCompleted;
			}
			if (eventHandler != null)
			{
				this._asyncWorkerUI.PostOperation(eventHandler, new object[]
				{
					this,
					new RecognizeCompletedEventArgs(result, initialSilenceTimeoutReached, babbleTimeoutReached, isStreamReleased, audioPosition, exception, isRecognizeCancelled, null)
				});
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002F2F0 File Offset: 0x0002E2F0
		private void FireEmulateRecognizeCompletedEvent(RecognitionResult result, Exception exception, bool isRecognizeCancelled)
		{
			EventHandler<EmulateRecognizeCompletedEventArgs> eventHandler;
			lock (this.SapiRecognizer)
			{
				eventHandler = this.EmulateRecognizeCompletedSync;
				if (eventHandler == null)
				{
					eventHandler = this.EmulateRecognizeCompleted;
				}
				this._lastResult = null;
				this._lastException = null;
				this._isEmulateRecognition = false;
				this._isRecognizing = false;
				this._isWaitingForRecognition = false;
			}
			if (eventHandler != null)
			{
				this._asyncWorkerUI.PostOperation(eventHandler, new object[]
				{
					this,
					new EmulateRecognizeCompletedEventArgs(result, exception, isRecognizeCancelled, null)
				});
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002F380 File Offset: 0x0002E380
		private static void CheckGrammarOptionsOnSapi51(Grammar grammar)
		{
			SRID srid = (SRID)(-1);
			if (grammar.BaseUri != null && !grammar.IsSrgsDocument)
			{
				srid = SRID.NotSupportedWithThisVersionOfSAPIBaseUri;
			}
			else if (grammar.IsStg || grammar.Sapi53Only)
			{
				srid = SRID.NotSupportedWithThisVersionOfSAPITagFormat;
			}
			if (srid != (SRID)(-1))
			{
				throw new NotSupportedException(SR.Get(srid, new object[0]));
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000AAB RID: 2731 RVA: 0x0002F3D3 File Offset: 0x0002E3D3
		// (remove) Token: 0x06000AAC RID: 2732 RVA: 0x0002F3EC File Offset: 0x0002E3EC
		private event EventHandler<RecognizeCompletedEventArgs> RecognizeCompletedSync;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000AAD RID: 2733 RVA: 0x0002F405 File Offset: 0x0002E405
		// (remove) Token: 0x06000AAE RID: 2734 RVA: 0x0002F41E File Offset: 0x0002E41E
		private event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompletedSync;

		// Token: 0x04000911 RID: 2321
		private const uint _nullBookmarkId = 0U;

		// Token: 0x04000912 RID: 2322
		private const uint _initialSilenceBookmarkId = 1U;

		// Token: 0x04000913 RID: 2323
		private const uint _babbleBookmarkId = 2U;

		// Token: 0x04000914 RID: 2324
		private const uint _firstUnusedBookmarkId = 3U;

		// Token: 0x0400091D RID: 2333
		private List<Grammar> _grammars;

		// Token: 0x0400091E RID: 2334
		private ReadOnlyCollection<Grammar> _readOnlyGrammars;

		// Token: 0x0400091F RID: 2335
		private RecognizerInfo _recognizerInfo;

		// Token: 0x04000920 RID: 2336
		private bool _disposed;

		// Token: 0x04000921 RID: 2337
		private ulong _currentGrammarId;

		// Token: 0x04000922 RID: 2338
		private SapiRecoContext _sapiContext;

		// Token: 0x04000923 RID: 2339
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x04000924 RID: 2340
		private bool _supportsSapi53;

		// Token: 0x04000925 RID: 2341
		private EventNotify _eventNotify;

		// Token: 0x04000926 RID: 2342
		private ulong _eventInterest;

		// Token: 0x04000927 RID: 2343
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x04000928 RID: 2344
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x04000929 RID: 2345
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x0400092A RID: 2346
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;

		// Token: 0x0400092B RID: 2347
		private bool _enabled = true;

		// Token: 0x0400092C RID: 2348
		private int _maxAlternates;

		// Token: 0x0400092D RID: 2349
		internal AudioState _audioState;

		// Token: 0x0400092E RID: 2350
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x0400092F RID: 2351
		private RecognizeMode _recognizeMode = RecognizeMode.Multiple;

		// Token: 0x04000930 RID: 2352
		private bool _isRecognizeCancelled;

		// Token: 0x04000931 RID: 2353
		private bool _isRecognizing;

		// Token: 0x04000932 RID: 2354
		private bool _isEmulateRecognition;

		// Token: 0x04000933 RID: 2355
		private bool _isWaitingForRecognition;

		// Token: 0x04000934 RID: 2356
		private RecognitionResult _lastResult;

		// Token: 0x04000935 RID: 2357
		private Exception _lastException;

		// Token: 0x04000936 RID: 2358
		private bool _pauseRecognizerOnRecognition = true;

		// Token: 0x04000937 RID: 2359
		private bool _detectingInitialSilenceTimeout;

		// Token: 0x04000938 RID: 2360
		private bool _detectingBabbleTimeout;

		// Token: 0x04000939 RID: 2361
		private bool _initialSilenceTimeoutReached;

		// Token: 0x0400093A RID: 2362
		private bool _babbleTimeoutReached;

		// Token: 0x0400093B RID: 2363
		private TimeSpan _initialSilenceTimeout;

		// Token: 0x0400093C RID: 2364
		private TimeSpan _babbleTimeout;

		// Token: 0x0400093D RID: 2365
		internal bool _haveInputSource;

		// Token: 0x0400093E RID: 2366
		private Stream _inputStream;

		// Token: 0x0400093F RID: 2367
		private Dictionary<int, object> _bookmarkTable = new Dictionary<int, object>();

		// Token: 0x04000940 RID: 2368
		private uint _nextBookmarkId = 3U;

		// Token: 0x04000941 RID: 2369
		private uint _prevMaxBookmarkId = 2U;

		// Token: 0x04000942 RID: 2370
		private OperationLock _waitForGrammarsToLoad = new OperationLock();

		// Token: 0x04000943 RID: 2371
		private object _grammarDataLock = new object();

		// Token: 0x04000944 RID: 2372
		private AsyncSerializedWorker _asyncWorker;

		// Token: 0x04000945 RID: 2373
		private AsyncSerializedWorker _asyncWorkerUI;

		// Token: 0x04000946 RID: 2374
		private AutoResetEvent _handlerWaitHandle = new AutoResetEvent(false);

		// Token: 0x04000947 RID: 2375
		private object _thisObjectLock = new object();

		// Token: 0x04000948 RID: 2376
		private Exception _loadException;

		// Token: 0x04000949 RID: 2377
		private Grammar _topLevel;

		// Token: 0x0400094A RID: 2378
		private bool _inproc;

		// Token: 0x0400094D RID: 2381
		private TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30.0);

		// Token: 0x0400094E RID: 2382
		private RecognizerBase.RecognizerBaseThunk _recoThunk;

		// Token: 0x02000196 RID: 406
		private class RecognizerBaseThunk : ISpGrammarResourceLoader
		{
			// Token: 0x06000AB0 RID: 2736 RVA: 0x0002F4BA File Offset: 0x0002E4BA
			internal RecognizerBaseThunk(RecognizerBase recognizer)
			{
				this._recognizerRef = new WeakReference(recognizer);
			}

			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0002F4CE File Offset: 0x0002E4CE
			internal RecognizerBase Recognizer
			{
				get
				{
					return (RecognizerBase)this._recognizerRef.Target;
				}
			}

			// Token: 0x06000AB2 RID: 2738 RVA: 0x0002F4E0 File Offset: 0x0002E4E0
			int ISpGrammarResourceLoader.LoadResource(string bstrResourceUri, bool fAlwaysReload, out IStream pStream, ref string pbstrMIMEType, ref short pfModified, ref string pbstrRedirectUrl)
			{
				return ((ISpGrammarResourceLoader)this.Recognizer).LoadResource(bstrResourceUri, fAlwaysReload, out pStream, ref pbstrMIMEType, ref pfModified, ref pbstrRedirectUrl);
			}

			// Token: 0x06000AB3 RID: 2739 RVA: 0x0002F4F6 File Offset: 0x0002E4F6
			string ISpGrammarResourceLoader.GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl)
			{
				return ((ISpGrammarResourceLoader)this.Recognizer).GetLocalCopy(resourcePath, out mimeType, out redirectUrl);
			}

			// Token: 0x06000AB4 RID: 2740 RVA: 0x0002F506 File Offset: 0x0002E506
			void ISpGrammarResourceLoader.ReleaseLocalCopy(string path)
			{
				((ISpGrammarResourceLoader)this.Recognizer).ReleaseLocalCopy(path);
			}

			// Token: 0x0400094F RID: 2383
			private WeakReference _recognizerRef;
		}
	}
}
