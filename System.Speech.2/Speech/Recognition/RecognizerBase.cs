using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
	// Token: 0x02000062 RID: 98
	internal class RecognizerBase : IRecognizerInternal, IDisposable, ISpGrammarResourceLoader
	{
		// Token: 0x06000239 RID: 569 RVA: 0x00009CB6 File Offset: 0x00007EB6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009CC8 File Offset: 0x00007EC8
		~RecognizerBase()
		{
			this.Dispose(false);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009CF8 File Offset: 0x00007EF8
		internal void LoadGrammar(Grammar grammar)
		{
			try
			{
				this.ValidateGrammar(grammar, new GrammarState[1]);
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
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
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

		// Token: 0x0600023C RID: 572 RVA: 0x00009DE8 File Offset: 0x00007FE8
		internal void LoadGrammarAsync(Grammar grammar)
		{
			if (!this._supportsSapi53)
			{
				RecognizerBase.CheckGrammarOptionsOnSapi51(grammar);
			}
			this.ValidateGrammar(grammar, new GrammarState[1]);
			ulong num;
			SapiGrammar sapiGrammar = this.CreateNewSapiGrammar(out num);
			grammar.InternalData = new InternalGrammarData(num, sapiGrammar, grammar.Enabled, grammar.Weight, grammar.Priority);
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x0600023D RID: 573 RVA: 0x00009EB4 File Offset: 0x000080B4
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
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
			{
				this._grammars.Remove(grammar);
			}
			grammar.State = GrammarState.Unloaded;
			grammar.InternalData = null;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009F34 File Offset: 0x00008134
		internal void UnloadAllGrammars()
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			List<Grammar> list;
			lock (sapiRecognizer)
			{
				list = new List<Grammar>(this._grammars);
			}
			this._waitForGrammarsToLoad.WaitForOperationsToFinish();
			foreach (Grammar grammar in list)
			{
				this.UnloadGrammar(grammar);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009FC4 File Offset: 0x000081C4
		void IRecognizerInternal.SetGrammarState(Grammar grammar, bool enabled)
		{
			InternalGrammarData internalData = grammar.InternalData;
			object grammarDataLock = this._grammarDataLock;
			lock (grammarDataLock)
			{
				if (grammar.Loaded)
				{
					internalData._sapiGrammar.SetGrammarState(enabled ? SPGRAMMARSTATE.SPGS_ENABLED : SPGRAMMARSTATE.SPGS_DISABLED);
				}
				internalData._grammarEnabled = enabled;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000A028 File Offset: 0x00008228
		void IRecognizerInternal.SetGrammarWeight(Grammar grammar, float weight)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI2, new object[] { "Weight" }));
			}
			InternalGrammarData internalData = grammar.InternalData;
			object grammarDataLock = this._grammarDataLock;
			lock (grammarDataLock)
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

		// Token: 0x06000241 RID: 577 RVA: 0x0000A0CC File Offset: 0x000082CC
		void IRecognizerInternal.SetGrammarPriority(Grammar grammar, int priority)
		{
			if (!this._supportsSapi53)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI2, new object[] { "Priority" }));
			}
			InternalGrammarData internalData = grammar.InternalData;
			object grammarDataLock = this._grammarDataLock;
			lock (grammarDataLock)
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

		// Token: 0x06000242 RID: 578 RVA: 0x0000A178 File Offset: 0x00008378
		Grammar IRecognizerInternal.GetGrammarFromId(ulong id)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000243 RID: 579 RVA: 0x0000A208 File Offset: 0x00008408
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

		// Token: 0x06000244 RID: 580 RVA: 0x0000A25A File Offset: 0x0000845A
		internal RecognitionResult EmulateRecognize(string inputText)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			return this.InternalEmulateRecognize(inputText, SpeechEmulationCompareFlags.SECFDefault, false, null);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A275 File Offset: 0x00008475
		internal void EmulateRecognizeAsync(string inputText)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			this.InternalEmulateRecognizeAsync(inputText, SpeechEmulationCompareFlags.SECFDefault, false, null);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000A290 File Offset: 0x00008490
		internal RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			bool flag = compareOptions == CompareOptions.IgnoreCase || compareOptions == CompareOptions.OrdinalIgnoreCase;
			if (!this._supportsSapi53 && !flag)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			return this.InternalEmulateRecognize(inputText, RecognizerBase.ConvertCompareOptions(compareOptions), !flag, null);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000A2E8 File Offset: 0x000084E8
		internal void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			Helpers.ThrowIfEmptyOrNull(inputText, "inputText");
			bool flag = compareOptions == CompareOptions.IgnoreCase || compareOptions == CompareOptions.OrdinalIgnoreCase;
			if (!this._supportsSapi53 && !flag)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			this.InternalEmulateRecognizeAsync(inputText, RecognizerBase.ConvertCompareOptions(compareOptions), !flag, null);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000A340 File Offset: 0x00008540
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

		// Token: 0x06000249 RID: 585 RVA: 0x0000A3B0 File Offset: 0x000085B0
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

		// Token: 0x0600024A RID: 586 RVA: 0x0000A41F File Offset: 0x0000861F
		internal void RequestRecognizerUpdate()
		{
			this.RequestRecognizerUpdate(null);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A428 File Offset: 0x00008628
		internal void RequestRecognizerUpdate(object userToken)
		{
			uint num = this.AddBookmarkItem(userToken);
			this.SapiContext.Bookmark(SPBOOKMARKOPTIONS.SPBO_PAUSE, 0UL, new IntPtr((long)((ulong)num)));
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A454 File Offset: 0x00008654
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

		// Token: 0x0600024D RID: 589 RVA: 0x0000A4C0 File Offset: 0x000086C0
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
				else
				{
					Trace.TraceInformation("SAPI does not implement phonetic alphabet selection.");
				}
			}
			catch (COMException)
			{
				Trace.TraceError("Cannot force SAPI to set the alphabet to UPS");
			}
			this._sapiContext.SetAudioOptions(SPAUDIOOPTIONS.SPAO_RETAIN_AUDIO, IntPtr.Zero, IntPtr.Zero);
			this.MaxAlternates = 10;
			this.ResetBookmarkTable();
			this._eventInterest = 854759695187968UL;
			this._sapiContext.SetInterest(this._eventInterest, this._eventInterest);
			this._asyncWorker = new AsyncSerializedWorker(new WaitCallback(this.DispatchEvents), null);
			this._asyncWorkerUI = new AsyncSerializedWorker(null, AsyncOperationManager.CreateOperation(null));
			this._asyncWorkerUI.WorkItemPending += this.SignalHandlerThread;
			this._eventNotify = this._sapiContext.CreateEventNotify(this._asyncWorker, this._supportsSapi53);
			this._grammars = new List<Grammar>();
			this._readOnlyGrammars = new ReadOnlyCollection<Grammar>(this._grammars);
			this.UpdateAudioFormat(null);
			this.InitialSilenceTimeout = TimeSpan.FromSeconds(30.0);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A680 File Offset: 0x00008880
		internal void RecognizeAsync(RecognizeMode mode)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x0600024F RID: 591 RVA: 0x0000A76C File Offset: 0x0000896C
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
			this.RecognizeCompletedSync += eventHandler;
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
				this.RecognizeCompletedSync -= eventHandler;
				this._initialSilenceTimeout = initialSilenceTimeout2;
				this._asyncWorkerUI.AsyncMode = true;
			}
			return result;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000A868 File Offset: 0x00008A68
		internal void RecognizeAsyncCancel()
		{
			bool flag = false;
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000251 RID: 593 RVA: 0x0000A8F8 File Offset: 0x00008AF8
		internal void RecognizeAsyncStop()
		{
			bool flag = false;
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000A96C File Offset: 0x00008B6C
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000A974 File Offset: 0x00008B74
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
					SapiRecognizer sapiRecognizer = this.SapiRecognizer;
					lock (sapiRecognizer)
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

		// Token: 0x06000254 RID: 596 RVA: 0x0000AA18 File Offset: 0x00008C18
		internal void SetInput(string path)
		{
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.SetInput(stream, null);
			this._inputStream = stream;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AA40 File Offset: 0x00008C40
		internal void SetInput(Stream stream, SpeechAudioFormatInfo audioFormat)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000256 RID: 598 RVA: 0x0000AAF0 File Offset: 0x00008CF0
		internal void SetInputToDefaultAudioDevice()
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000257 RID: 599 RVA: 0x0000ABF4 File Offset: 0x00008DF4
		internal int QueryRecognizerSettingAsInt(string settingName)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			return this.SapiRecognizer.GetPropertyNum(settingName);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AC10 File Offset: 0x00008E10
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

		// Token: 0x06000259 RID: 601 RVA: 0x0000AC78 File Offset: 0x00008E78
		internal void UpdateRecognizerSetting(string settingName, string updatedValue)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			this.SapiRecognizer.SetPropertyString(settingName, updatedValue);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AC92 File Offset: 0x00008E92
		internal void UpdateRecognizerSetting(string settingName, int updatedValue)
		{
			Helpers.ThrowIfEmptyOrNull(settingName, "settingName");
			this.SapiRecognizer.SetPropertyNum(settingName, updatedValue);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000ACAC File Offset: 0x00008EAC
		internal static Exception ExceptionFromSapiCreateRecognizerError(COMException e)
		{
			return RecognizerBase.ExceptionFromSapiCreateRecognizerError((SAPIErrorCodes)e.ErrorCode);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000ACBC File Offset: 0x00008EBC
		internal static Exception ExceptionFromSapiCreateRecognizerError(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (errorCode != SAPIErrorCodes.CLASS_E_CLASSNOTAVAILABLE && errorCode != SAPIErrorCodes.REGDB_E_CLASSNOTREG)
			{
				if (errorCode - SAPIErrorCodes.SPERR_SHARED_ENGINE_DISABLED > 1)
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
				return new PlatformNotSupportedException(SR.Get(srid, new object[0]));
			}
			else
			{
				OperatingSystem osversion = Environment.OSVersion;
				if (IntPtr.Size == 8 && osversion.Platform == PlatformID.Win32NT && osversion.Version.Major == 5)
				{
					return new NotSupportedException(SR.Get(SRID.RecognitionNotSupportedOn64bit, new object[0]));
				}
				return new PlatformNotSupportedException(SR.Get(SRID.RecognitionNotSupported, new object[0]));
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000AD88 File Offset: 0x00008F88
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000ADCC File Offset: 0x00008FCC
		internal TimeSpan InitialSilenceTimeout
		{
			get
			{
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				TimeSpan initialSilenceTimeout;
				lock (sapiRecognizer)
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
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
				{
					if (this._isRecognizing)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
					}
					this._initialSilenceTimeout = value;
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000AE54 File Offset: 0x00009054
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000AE98 File Offset: 0x00009098
		internal TimeSpan BabbleTimeout
		{
			get
			{
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				TimeSpan babbleTimeout;
				lock (sapiRecognizer)
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
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
				{
					if (this._isRecognizing)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerAlreadyRecognizing, new object[0]));
					}
					this._babbleTimeout = value;
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000AF20 File Offset: 0x00009120
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000AF64 File Offset: 0x00009164
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000AFA8 File Offset: 0x000091A8
		internal bool Enabled
		{
			get
			{
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				bool enabled;
				lock (sapiRecognizer)
				{
					enabled = this._enabled;
				}
				return enabled;
			}
			set
			{
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000B01C File Offset: 0x0000921C
		internal ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this._readOnlyGrammars;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000B024 File Offset: 0x00009224
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000B06C File Offset: 0x0000926C
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000B07E File Offset: 0x0000927E
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000B088 File Offset: 0x00009288
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
						SapiRecognizer sapiRecognizer = this.SapiRecognizer;
						lock (sapiRecognizer)
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000B10C File Offset: 0x0000930C
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
					SapiRecognizer sapiRecognizer = this.SapiRecognizer;
					lock (sapiRecognizer)
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000B1B4 File Offset: 0x000093B4
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
					SapiRecognizer sapiRecognizer = this.SapiRecognizer;
					lock (sapiRecognizer)
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000B22C File Offset: 0x0000942C
		internal SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000B290 File Offset: 0x00009490
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000B298 File Offset: 0x00009498
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

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600026E RID: 622 RVA: 0x0000B2D8 File Offset: 0x000094D8
		// (remove) Token: 0x0600026F RID: 623 RVA: 0x0000B310 File Offset: 0x00009510
		internal event EventHandler<RecognizeCompletedEventArgs> RecognizeCompleted;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000270 RID: 624 RVA: 0x0000B348 File Offset: 0x00009548
		// (remove) Token: 0x06000271 RID: 625 RVA: 0x0000B380 File Offset: 0x00009580
		internal event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000272 RID: 626 RVA: 0x0000B3B8 File Offset: 0x000095B8
		// (remove) Token: 0x06000273 RID: 627 RVA: 0x0000B3F0 File Offset: 0x000095F0
		internal event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000274 RID: 628 RVA: 0x0000B428 File Offset: 0x00009628
		// (remove) Token: 0x06000275 RID: 629 RVA: 0x0000B460 File Offset: 0x00009660
		internal event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000276 RID: 630 RVA: 0x0000B498 File Offset: 0x00009698
		// (remove) Token: 0x06000277 RID: 631 RVA: 0x0000B4D0 File Offset: 0x000096D0
		internal event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000278 RID: 632 RVA: 0x0000B508 File Offset: 0x00009708
		// (remove) Token: 0x06000279 RID: 633 RVA: 0x0000B540 File Offset: 0x00009740
		internal event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600027A RID: 634 RVA: 0x0000B578 File Offset: 0x00009778
		// (remove) Token: 0x0600027B RID: 635 RVA: 0x0000B5B0 File Offset: 0x000097B0
		internal event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600027C RID: 636 RVA: 0x0000B5E5 File Offset: 0x000097E5
		// (remove) Token: 0x0600027D RID: 637 RVA: 0x0000B615 File Offset: 0x00009815
		internal event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				if (this._speechHypothesizedDelegate == null)
				{
					this.AddEventInterest(549755813888UL);
				}
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Combine(this._speechHypothesizedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Remove(this._speechHypothesizedDelegate, value);
				if (this._speechHypothesizedDelegate == null)
				{
					this.RemoveEventInterest(549755813888UL);
				}
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600027E RID: 638 RVA: 0x0000B645 File Offset: 0x00009845
		// (remove) Token: 0x0600027F RID: 639 RVA: 0x0000B675 File Offset: 0x00009875
		internal event EventHandler<AudioSignalProblemOccurredEventArgs> AudioSignalProblemOccurred
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.AddEventInterest(17592186044416UL);
				}
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Combine(this._audioSignalProblemOccurredDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Remove(this._audioSignalProblemOccurredDelegate, value);
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RemoveEventInterest(17592186044416UL);
				}
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000280 RID: 640 RVA: 0x0000B6A5 File Offset: 0x000098A5
		// (remove) Token: 0x06000281 RID: 641 RVA: 0x0000B6D5 File Offset: 0x000098D5
		internal event EventHandler<AudioLevelUpdatedEventArgs> AudioLevelUpdated
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.AddEventInterest(1125899906842624UL);
				}
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Combine(this._audioLevelUpdatedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Remove(this._audioLevelUpdatedDelegate, value);
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RemoveEventInterest(1125899906842624UL);
				}
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000282 RID: 642 RVA: 0x0000B705 File Offset: 0x00009905
		// (remove) Token: 0x06000283 RID: 643 RVA: 0x0000B71E File Offset: 0x0000991E
		internal event EventHandler<AudioStateChangedEventArgs> AudioStateChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Combine(this._audioStateChangedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Remove(this._audioStateChangedDelegate, value);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000284 RID: 644 RVA: 0x0000B738 File Offset: 0x00009938
		// (remove) Token: 0x06000285 RID: 645 RVA: 0x0000B770 File Offset: 0x00009970
		internal event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x06000286 RID: 646 RVA: 0x0000B7A8 File Offset: 0x000099A8
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed && disposing)
			{
				object thisObjectLock = this._thisObjectLock;
				lock (thisObjectLock)
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000B8BC File Offset: 0x00009ABC
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000B8D7 File Offset: 0x00009AD7
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

		// Token: 0x06000289 RID: 649 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		private void LoadSapiGrammar(Grammar grammar, SapiGrammar sapiGrammar, bool enabled, float weight, int priority)
		{
			Uri uri = grammar.BaseUri;
			if (this._supportsSapi53 && uri == null && grammar.Uri != null)
			{
				string originalString = grammar.Uri.OriginalString;
				int num = originalString.LastIndexOfAny(new char[] { '\\', '/' });
				if (num >= 0)
				{
					uri = new Uri(originalString.Substring(0, num + 1), UriKind.RelativeOrAbsolute);
				}
			}
			if (grammar.IsDictation(grammar.Uri))
			{
				this.LoadSapiDictationGrammar(sapiGrammar, grammar.Uri, grammar.RuleName, enabled, weight, priority);
				return;
			}
			this.LoadSapiGrammarFromCfg(sapiGrammar, grammar, uri, enabled, weight, priority);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B994 File Offset: 0x00009B94
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

		// Token: 0x0600028B RID: 651 RVA: 0x0000BA34 File Offset: 0x00009C34
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
				string[] array = pbstrRedirectUrl.Split(new char[] { ' ' }, StringSplitOptions.None);
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

		// Token: 0x0600028C RID: 652 RVA: 0x0000BB64 File Offset: 0x00009D64
		string ISpGrammarResourceLoader.GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl)
		{
			redirectUrl = null;
			mimeType = null;
			return null;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void ISpGrammarResourceLoader.ReleaseLocalCopy(string path)
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000BB70 File Offset: 0x00009D70
		private void LoadSapiGrammarFromCfg(SapiGrammar sapiGrammar, Grammar grammar, Uri baseUri, bool enabled, float weight, int priority)
		{
			byte[] cfgData = grammar.CfgData;
			GCHandle gchandle = GCHandle.Alloc(cfgData, GCHandleType.Pinned);
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
				if (errorCode <= SAPIErrorCodes.SPERR_TOO_MANY_GRAMMARS)
				{
					if (errorCode == SAPIErrorCodes.SPERR_UNSUPPORTED_FORMAT)
					{
						throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]), ex);
					}
					if (errorCode == SAPIErrorCodes.SPERR_TOO_MANY_GRAMMARS)
					{
						throw new NotSupportedException(SR.Get(SRID.SapiErrorTooManyGrammars, new object[0]), ex);
					}
				}
				else
				{
					if (errorCode == SAPIErrorCodes.SPERR_INVALID_IMPORT)
					{
						throw new FormatException(SR.Get(SRID.SapiErrorInvalidImport, new object[0]), ex);
					}
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
						goto IL_130;
					}
				}
				RecognizerBase.ThrowIfSapiErrorCode((SAPIErrorCodes)ex.ErrorCode);
				IL_130:
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

		// Token: 0x0600028F RID: 655 RVA: 0x0000BD34 File Offset: 0x00009F34
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

		// Token: 0x06000290 RID: 656 RVA: 0x0000BDDC File Offset: 0x00009FDC
		private void LoadGrammarAsyncCallback(object grammarObject)
		{
			Grammar grammar = (Grammar)grammarObject;
			InternalGrammarData internalData = grammar.InternalData;
			Exception ex = null;
			try
			{
				object grammarDataLock = this._grammarDataLock;
				lock (grammarDataLock)
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

		// Token: 0x06000291 RID: 657 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
		private void LoadGrammarAsyncCompletedCallback(object grammarObject)
		{
			Grammar grammar = (Grammar)grammarObject;
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted(this, new LoadGrammarCompletedEventArgs(grammar, grammar.LoadException, false, null));
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
		private SapiGrammar CreateNewSapiGrammar(out ulong grammarId)
		{
			ulong currentGrammarId = this._currentGrammarId;
			for (;;)
			{
				this._currentGrammarId += 1UL;
				bool flag = false;
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				lock (sapiRecognizer)
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

		// Token: 0x06000293 RID: 659 RVA: 0x0000BFCC File Offset: 0x0000A1CC
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

		// Token: 0x06000294 RID: 660 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		private RecognitionResult InternalEmulateRecognize(string phrase, SpeechEmulationCompareFlags flag, bool useReco2, RecognizedWordUnit[] wordUnits)
		{
			RecognitionResult result = null;
			bool completed = false;
			EventHandler<EmulateRecognizeCompletedEventArgs> eventHandler = delegate(object sender, EmulateRecognizeCompletedEventArgs eventArgs)
			{
				result = eventArgs.Result;
				completed = true;
			};
			this.EmulateRecognizeCompletedSync += eventHandler;
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
				this.EmulateRecognizeCompletedSync -= eventHandler;
				this._asyncWorkerUI.AsyncMode = true;
			}
			return result;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000C140 File Offset: 0x0000A340
		private void InternalEmulateRecognizeAsync(string phrase, SpeechEmulationCompareFlags flag, bool useReco2, RecognizedWordUnit[] wordUnits)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000296 RID: 662 RVA: 0x0000C268 File Offset: 0x0000A468
		private void EmulateRecognizedFailReportError(SAPIErrorCodes hr)
		{
			this._lastException = RecognizerBase.ExceptionFromSapiCreateRecognizerError(hr);
			if (hr < SAPIErrorCodes.S_OK || hr == SAPIErrorCodes.SP_NO_RULE_ACTIVE)
			{
				this.FireEmulateRecognizeCompletedEvent(null, this._lastException, true);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000C290 File Offset: 0x0000A490
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

		// Token: 0x06000298 RID: 664 RVA: 0x0000C38C File Offset: 0x0000A58C
		private void RecognizeAsyncWaitForGrammarsToLoad(object unused)
		{
			this._waitForGrammarsToLoad.WaitForOperationsToFinish();
			Exception ex = null;
			bool flag = false;
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x06000299 RID: 665 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		private void RecognizeAsyncWaitForGrammarsToLoadFailed(object eventArgs)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
			{
				this._isRecognizeCancelled = false;
				this._isRecognizing = false;
			}
			EventHandler<RecognizeCompletedEventArgs> recognizeCompleted = this.RecognizeCompleted;
			if (recognizeCompleted != null)
			{
				recognizeCompleted(this, (RecognizeCompletedEventArgs)eventArgs);
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000C51C File Offset: 0x0000A71C
		private void SignalHandlerThread(object ignored)
		{
			if (!this._asyncWorkerUI.AsyncMode)
			{
				this._handlerWaitHandle.Set();
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000C538 File Offset: 0x0000A738
		private void DispatchEvents(object eventData)
		{
			object thisObjectLock = this._thisObjectLock;
			lock (thisObjectLock)
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000C63C File Offset: 0x0000A83C
		private void ProcessStartStreamEvent()
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x0600029D RID: 669 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		private void ProcessPhraseStartEvent(SpeechEvent speechEvent)
		{
			this._isWaitingForRecognition = true;
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x0600029E RID: 670 RVA: 0x0000C7CC File Offset: 0x0000A9CC
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
						recognizerUpdateReached(this, new RecognizerUpdateReachedEventArgs(bookmarkItemAndRemove, speechEvent.AudioPosition));
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

		// Token: 0x0600029F RID: 671 RVA: 0x0000C880 File Offset: 0x0000AA80
		private void ProcessHypothesisEvent(SpeechEvent speechEvent)
		{
			RecognitionResult recognitionResult = this.CreateRecognitionResult(speechEvent);
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			bool enabled;
			lock (sapiRecognizer)
			{
				enabled = this._enabled;
			}
			if (recognitionResult.Grammar != null && recognitionResult.Grammar.Enabled && enabled)
			{
				this.FireSpeechHypothesizedEvent(recognitionResult);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		private void ProcessRecognitionEvent(SpeechEvent speechEvent)
		{
			this._detectingInitialSilenceTimeout = false;
			this._detectingBabbleTimeout = false;
			bool flag = true;
			bool flag2 = (speechEvent.WParam & 2UL) > 0UL;
			try
			{
				RecognitionResult recognitionResult = this.CreateRecognitionResult(speechEvent);
				SapiRecognizer sapiRecognizer = this.SapiRecognizer;
				bool enabled;
				lock (sapiRecognizer)
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

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CA78 File Offset: 0x0000AC78
		private void ProcessRecoOtherContextEvent()
		{
			if (this._isEmulateRecognition && !this._inproc)
			{
				this.FireEmulateRecognizeCompletedEvent(this._lastResult, this._lastException, false);
			}
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
			{
				this._audioState = AudioState.Silence;
			}
			this.FireAudioStateChangedEvent(this._audioState);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
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
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			bool isRecognizeCancelled;
			lock (sapiRecognizer)
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

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CC34 File Offset: 0x0000AE34
		private void ProcessInterferenceEvent(uint interference)
		{
			this.FireSignalProblemOccurredEvent((AudioSignalProblem)interference);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000CC3D File Offset: 0x0000AE3D
		private void ProcessAudioLevelEvent(int audioLevel)
		{
			this.FireAudioLevelUpdatedEvent(audioLevel);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000CC46 File Offset: 0x0000AE46
		private void EndRecognitionWithTimeout()
		{
			this._initialSilenceTimeoutReached = true;
			this.SapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE_WITH_PURGE);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000CC5C File Offset: 0x0000AE5C
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

		// Token: 0x060002A7 RID: 679 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		private void UpdateAudioFormat(SpeechAudioFormatInfo audioFormat)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			lock (sapiRecognizer)
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

		// Token: 0x060002A8 RID: 680 RVA: 0x0000CD40 File Offset: 0x0000AF40
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

		// Token: 0x060002A9 RID: 681 RVA: 0x0000CDC8 File Offset: 0x0000AFC8
		private ulong TimeSpanToStreamPosition(TimeSpan time)
		{
			return (ulong)(time.Ticks * (long)this.AudioFormat.AverageBytesPerSecond / 10000000L);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		private static void ThrowIfSapiErrorCode(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (srid >= SRID.NullParamIllegal)
			{
				throw new InvalidOperationException(SR.Get(srid, new object[0]));
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000CE14 File Offset: 0x0000B014
		private static Exception ExceptionFromSapiStreamError(SAPIErrorCodes errorCode)
		{
			SRID srid = SapiConstants.SapiErrorCode2SRID(errorCode);
			if (srid >= SRID.NullParamIllegal)
			{
				return new InvalidOperationException(SR.Get(srid, new object[0]));
			}
			return new COMException(SR.Get(SRID.AudioDeviceInternalError, new object[0]), (int)errorCode);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000CE54 File Offset: 0x0000B054
		private static SpeechEmulationCompareFlags ConvertCompareOptions(CompareOptions compareOptions)
		{
			CompareOptions compareOptions2 = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.OrdinalIgnoreCase | CompareOptions.Ordinal;
			SpeechEmulationCompareFlags speechEmulationCompareFlags = (SpeechEmulationCompareFlags)0;
			if ((compareOptions & CompareOptions.IgnoreCase) != CompareOptions.None || (compareOptions & CompareOptions.OrdinalIgnoreCase) != CompareOptions.None)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreCase;
			}
			if ((compareOptions & CompareOptions.IgnoreKanaType) != CompareOptions.None)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreKanaType;
			}
			if ((compareOptions & CompareOptions.IgnoreWidth) != CompareOptions.None)
			{
				speechEmulationCompareFlags |= SpeechEmulationCompareFlags.SECFIgnoreWidth;
			}
			if ((compareOptions & ~(compareOptions2 != CompareOptions.None)) != CompareOptions.None)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPICompareOption, new object[0]));
			}
			return speechEmulationCompareFlags;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
		internal void AddEventInterest(ulong interest)
		{
			if ((this._eventInterest & interest) != interest)
			{
				this._eventInterest |= interest;
				this.SapiContext.SetInterest(this._eventInterest, this._eventInterest);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000CEE2 File Offset: 0x0000B0E2
		internal void RemoveEventInterest(ulong interest)
		{
			if ((this._eventInterest & interest) != 0UL)
			{
				this._eventInterest &= ~interest;
				this.SapiContext.SetInterest(this._eventInterest, this._eventInterest);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000CF14 File Offset: 0x0000B114
		private uint AddBookmarkItem(object userToken)
		{
			uint num = 0U;
			if (userToken != null)
			{
				Dictionary<int, object> bookmarkTable = this._bookmarkTable;
				lock (bookmarkTable)
				{
					uint nextBookmarkId = this._nextBookmarkId;
					this._nextBookmarkId = nextBookmarkId + 1U;
					num = nextBookmarkId;
					if (this._nextBookmarkId == 0U)
					{
						throw new InvalidOperationException(SR.Get(SRID.RecognizerUpdateTableTooLarge, new object[0]));
					}
					this._bookmarkTable[(int)num] = userToken;
				}
			}
			return num;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000CF94 File Offset: 0x0000B194
		private void ResetBookmarkTable()
		{
			Dictionary<int, object> bookmarkTable = this._bookmarkTable;
			lock (bookmarkTable)
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

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D054 File Offset: 0x0000B254
		private object GetBookmarkItemAndRemove(uint bookmarkId)
		{
			object obj = null;
			if (bookmarkId != 0U)
			{
				Dictionary<int, object> bookmarkTable = this._bookmarkTable;
				lock (bookmarkTable)
				{
					obj = this._bookmarkTable[(int)bookmarkId];
					this._bookmarkTable.Remove((int)bookmarkId);
				}
			}
			return obj;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		private void CloseCachedInputStream()
		{
			if (this._inputStream != null)
			{
				this._inputStream.Close();
				this._inputStream = null;
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D0CC File Offset: 0x0000B2CC
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

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D104 File Offset: 0x0000B304
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
					SapiRecognizer sapiRecognizer = this.SapiRecognizer;
					lock (sapiRecognizer)
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

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
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

		// Token: 0x060002B6 RID: 694 RVA: 0x0000D228 File Offset: 0x0000B428
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

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D260 File Offset: 0x0000B460
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

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D298 File Offset: 0x0000B498
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

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
		private void FireSpeechRecognitionRejectedEvent(RecognitionResult result)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			SpeechRecognitionRejectedEventArgs speechRecognitionRejectedEventArgs = new SpeechRecognitionRejectedEventArgs(result);
			if (speechRecognitionRejected != null)
			{
				this._asyncWorkerUI.PostOperation(speechRecognitionRejected, new object[] { this, speechRecognitionRejectedEventArgs });
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000D308 File Offset: 0x0000B508
		private void FireSpeechRecognizedEvent(SpeechRecognizedEventArgs recognitionEventArgs)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				this._asyncWorkerUI.PostOperation(speechRecognized, new object[] { this, recognitionEventArgs });
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000D33C File Offset: 0x0000B53C
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

		// Token: 0x060002BC RID: 700 RVA: 0x0000D388 File Offset: 0x0000B588
		private void FireEmulateRecognizeCompletedEvent(RecognitionResult result, Exception exception, bool isRecognizeCancelled)
		{
			SapiRecognizer sapiRecognizer = this.SapiRecognizer;
			EventHandler<EmulateRecognizeCompletedEventArgs> eventHandler;
			lock (sapiRecognizer)
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

		// Token: 0x060002BD RID: 701 RVA: 0x0000D41C File Offset: 0x0000B61C
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

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060002BE RID: 702 RVA: 0x0000D470 File Offset: 0x0000B670
		// (remove) Token: 0x060002BF RID: 703 RVA: 0x0000D4A8 File Offset: 0x0000B6A8
		private event EventHandler<RecognizeCompletedEventArgs> RecognizeCompletedSync;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060002C0 RID: 704 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		// (remove) Token: 0x060002C1 RID: 705 RVA: 0x0000D518 File Offset: 0x0000B718
		private event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompletedSync;

		// Token: 0x04000356 RID: 854
		private List<Grammar> _grammars;

		// Token: 0x04000357 RID: 855
		private ReadOnlyCollection<Grammar> _readOnlyGrammars;

		// Token: 0x04000358 RID: 856
		private RecognizerInfo _recognizerInfo;

		// Token: 0x04000359 RID: 857
		private bool _disposed;

		// Token: 0x0400035A RID: 858
		private ulong _currentGrammarId;

		// Token: 0x0400035B RID: 859
		private SapiRecoContext _sapiContext;

		// Token: 0x0400035C RID: 860
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x0400035D RID: 861
		private bool _supportsSapi53;

		// Token: 0x0400035E RID: 862
		private EventNotify _eventNotify;

		// Token: 0x0400035F RID: 863
		private ulong _eventInterest;

		// Token: 0x04000360 RID: 864
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x04000361 RID: 865
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x04000362 RID: 866
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x04000363 RID: 867
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;

		// Token: 0x04000364 RID: 868
		private bool _enabled = true;

		// Token: 0x04000365 RID: 869
		private int _maxAlternates;

		// Token: 0x04000366 RID: 870
		internal AudioState _audioState;

		// Token: 0x04000367 RID: 871
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x04000368 RID: 872
		private RecognizeMode _recognizeMode = RecognizeMode.Multiple;

		// Token: 0x04000369 RID: 873
		private bool _isRecognizeCancelled;

		// Token: 0x0400036A RID: 874
		private bool _isRecognizing;

		// Token: 0x0400036B RID: 875
		private bool _isEmulateRecognition;

		// Token: 0x0400036C RID: 876
		private bool _isWaitingForRecognition;

		// Token: 0x0400036D RID: 877
		private RecognitionResult _lastResult;

		// Token: 0x0400036E RID: 878
		private Exception _lastException;

		// Token: 0x0400036F RID: 879
		private bool _pauseRecognizerOnRecognition = true;

		// Token: 0x04000370 RID: 880
		private bool _detectingInitialSilenceTimeout;

		// Token: 0x04000371 RID: 881
		private bool _detectingBabbleTimeout;

		// Token: 0x04000372 RID: 882
		private bool _initialSilenceTimeoutReached;

		// Token: 0x04000373 RID: 883
		private bool _babbleTimeoutReached;

		// Token: 0x04000374 RID: 884
		private TimeSpan _initialSilenceTimeout;

		// Token: 0x04000375 RID: 885
		private TimeSpan _babbleTimeout;

		// Token: 0x04000376 RID: 886
		internal bool _haveInputSource;

		// Token: 0x04000377 RID: 887
		private Stream _inputStream;

		// Token: 0x04000378 RID: 888
		private Dictionary<int, object> _bookmarkTable = new Dictionary<int, object>();

		// Token: 0x04000379 RID: 889
		private uint _nextBookmarkId = 3U;

		// Token: 0x0400037A RID: 890
		private uint _prevMaxBookmarkId = 2U;

		// Token: 0x0400037B RID: 891
		private OperationLock _waitForGrammarsToLoad = new OperationLock();

		// Token: 0x0400037C RID: 892
		private object _grammarDataLock = new object();

		// Token: 0x0400037D RID: 893
		private const uint _nullBookmarkId = 0U;

		// Token: 0x0400037E RID: 894
		private const uint _initialSilenceBookmarkId = 1U;

		// Token: 0x0400037F RID: 895
		private const uint _babbleBookmarkId = 2U;

		// Token: 0x04000380 RID: 896
		private const uint _firstUnusedBookmarkId = 3U;

		// Token: 0x04000381 RID: 897
		private AsyncSerializedWorker _asyncWorker;

		// Token: 0x04000382 RID: 898
		private AsyncSerializedWorker _asyncWorkerUI;

		// Token: 0x04000383 RID: 899
		private AutoResetEvent _handlerWaitHandle = new AutoResetEvent(false);

		// Token: 0x04000384 RID: 900
		private object _thisObjectLock = new object();

		// Token: 0x04000385 RID: 901
		private Exception _loadException;

		// Token: 0x04000386 RID: 902
		private Grammar _topLevel;

		// Token: 0x04000387 RID: 903
		private bool _inproc;

		// Token: 0x0400038A RID: 906
		private TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30.0);

		// Token: 0x0400038B RID: 907
		private RecognizerBase.RecognizerBaseThunk _recoThunk;

		// Token: 0x0200017B RID: 379
		private class RecognizerBaseThunk : ISpGrammarResourceLoader
		{
			// Token: 0x06000B4B RID: 2891 RVA: 0x0002D57C File Offset: 0x0002B77C
			internal RecognizerBaseThunk(RecognizerBase recognizer)
			{
				this._recognizerRef = new WeakReference(recognizer);
			}

			// Token: 0x17000203 RID: 515
			// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0002D590 File Offset: 0x0002B790
			internal RecognizerBase Recognizer
			{
				get
				{
					return (RecognizerBase)this._recognizerRef.Target;
				}
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x0002D5A2 File Offset: 0x0002B7A2
			int ISpGrammarResourceLoader.LoadResource(string bstrResourceUri, bool fAlwaysReload, out IStream pStream, ref string pbstrMIMEType, ref short pfModified, ref string pbstrRedirectUrl)
			{
				return ((ISpGrammarResourceLoader)this.Recognizer).LoadResource(bstrResourceUri, fAlwaysReload, out pStream, ref pbstrMIMEType, ref pfModified, ref pbstrRedirectUrl);
			}

			// Token: 0x06000B4E RID: 2894 RVA: 0x0002D5B8 File Offset: 0x0002B7B8
			string ISpGrammarResourceLoader.GetLocalCopy(Uri resourcePath, out string mimeType, out Uri redirectUrl)
			{
				return ((ISpGrammarResourceLoader)this.Recognizer).GetLocalCopy(resourcePath, out mimeType, out redirectUrl);
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x0002D5C8 File Offset: 0x0002B7C8
			void ISpGrammarResourceLoader.ReleaseLocalCopy(string path)
			{
				((ISpGrammarResourceLoader)this.Recognizer).ReleaseLocalCopy(path);
			}

			// Token: 0x040008AC RID: 2220
			private WeakReference _recognizerRef;
		}
	}
}
