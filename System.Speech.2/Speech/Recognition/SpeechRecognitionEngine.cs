using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.ObjectTokens;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x0200006F RID: 111
	public class SpeechRecognitionEngine : IDisposable
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000D9E7 File Offset: 0x0000BBE7
		public SpeechRecognitionEngine()
		{
			this.Initialize(null);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D9F8 File Offset: 0x0000BBF8
		public SpeechRecognitionEngine(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			foreach (RecognizerInfo recognizerInfo in SpeechRecognitionEngine.InstalledRecognizers())
			{
				if (culture.Equals(recognizerInfo.Culture))
				{
					this.Initialize(recognizerInfo);
					return;
				}
			}
			foreach (RecognizerInfo recognizerInfo2 in SpeechRecognitionEngine.InstalledRecognizers())
			{
				if (Helpers.CompareInvariantCulture(recognizerInfo2.Culture, culture))
				{
					this.Initialize(recognizerInfo2);
					return;
				}
			}
			throw new ArgumentException(SR.Get(SRID.RecognizerNotFound, new object[0]), "culture");
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public SpeechRecognitionEngine(string recognizerId)
		{
			Helpers.ThrowIfEmptyOrNull(recognizerId, "recognizerId");
			foreach (RecognizerInfo recognizerInfo in SpeechRecognitionEngine.InstalledRecognizers())
			{
				if (recognizerId.Equals(recognizerInfo.Id, StringComparison.OrdinalIgnoreCase))
				{
					this.Initialize(recognizerInfo);
					return;
				}
			}
			throw new ArgumentException(SR.Get(SRID.RecognizerNotFound, new object[0]), "recognizerId");
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public SpeechRecognitionEngine(RecognizerInfo recognizerInfo)
		{
			Helpers.ThrowIfNull(recognizerInfo, "recognizerInfo");
			this.Initialize(recognizerInfo);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DB96 File Offset: 0x0000BD96
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DBA8 File Offset: 0x0000BDA8
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this._disposed)
			{
				if (this._recognizerBase != null)
				{
					this._recognizerBase.Dispose();
					this._recognizerBase = null;
				}
				if (this._sapiRecognizer != null)
				{
					this._sapiRecognizer.Dispose();
					this._sapiRecognizer = null;
				}
				this._disposed = true;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DBFC File Offset: 0x0000BDFC
		public static ReadOnlyCollection<RecognizerInfo> InstalledRecognizers()
		{
			List<RecognizerInfo> list = new List<RecognizerInfo>();
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Recognizers"))
			{
				if (objectTokenCategory != null)
				{
					foreach (ObjectToken objectToken in ((IEnumerable<ObjectToken>)objectTokenCategory))
					{
						RecognizerInfo recognizerInfo = RecognizerInfo.Create(objectToken);
						if (recognizerInfo != null)
						{
							list.Add(recognizerInfo);
						}
					}
				}
			}
			return new ReadOnlyCollection<RecognizerInfo>(list);
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000DC84 File Offset: 0x0000BE84
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000DC91 File Offset: 0x0000BE91
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TimeSpan InitialSilenceTimeout
		{
			get
			{
				return this.RecoBase.InitialSilenceTimeout;
			}
			set
			{
				this.RecoBase.InitialSilenceTimeout = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000DC9F File Offset: 0x0000BE9F
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TimeSpan BabbleTimeout
		{
			get
			{
				return this.RecoBase.BabbleTimeout;
			}
			set
			{
				this.RecoBase.BabbleTimeout = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000DCBA File Offset: 0x0000BEBA
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TimeSpan EndSilenceTimeout
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)this.RecoBase.QueryRecognizerSettingAsInt("ResponseSpeed"));
			}
			set
			{
				if (value.TotalMilliseconds < 0.0 || value.TotalMilliseconds > 10000.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.EndSilenceOutOfRange, new object[0]));
				}
				this.RecoBase.UpdateRecognizerSetting("ResponseSpeed", (int)value.TotalMilliseconds);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000DD38 File Offset: 0x0000BF38
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000DD50 File Offset: 0x0000BF50
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TimeSpan EndSilenceTimeoutAmbiguous
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)this.RecoBase.QueryRecognizerSettingAsInt("ComplexResponseSpeed"));
			}
			set
			{
				if (value.TotalMilliseconds < 0.0 || value.TotalMilliseconds > 10000.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.EndSilenceOutOfRange, new object[0]));
				}
				this.RecoBase.UpdateRecognizerSetting("ComplexResponseSpeed", (int)value.TotalMilliseconds);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000DDB4 File Offset: 0x0000BFB4
		public ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this.RecoBase.Grammars;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000DDC1 File Offset: 0x0000BFC1
		public RecognizerInfo RecognizerInfo
		{
			get
			{
				return this.RecoBase.RecognizerInfo;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000DDCE File Offset: 0x0000BFCE
		public AudioState AudioState
		{
			get
			{
				return this.RecoBase.AudioState;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000DDDB File Offset: 0x0000BFDB
		public int AudioLevel
		{
			get
			{
				return this.RecoBase.AudioLevel;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000DDE8 File Offset: 0x0000BFE8
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this.RecoBase.RecognizerAudioPosition;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000DDF5 File Offset: 0x0000BFF5
		public TimeSpan AudioPosition
		{
			get
			{
				return this.RecoBase.AudioPosition;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000DE02 File Offset: 0x0000C002
		public SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				return this.RecoBase.AudioFormat;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000DE0F File Offset: 0x0000C00F
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000DE1C File Offset: 0x0000C01C
		public int MaxAlternates
		{
			get
			{
				return this.RecoBase.MaxAlternates;
			}
			set
			{
				this.RecoBase.MaxAlternates = value;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000DE2A File Offset: 0x0000C02A
		public void SetInputToWaveFile(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.RecoBase.SetInput(path);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000DE43 File Offset: 0x0000C043
		public void SetInputToWaveStream(Stream audioSource)
		{
			this.RecoBase.SetInput(audioSource, null);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000DE52 File Offset: 0x0000C052
		public void SetInputToAudioStream(Stream audioSource, SpeechAudioFormatInfo audioFormat)
		{
			Helpers.ThrowIfNull(audioSource, "audioSource");
			Helpers.ThrowIfNull(audioFormat, "audioFormat");
			this.RecoBase.SetInput(audioSource, audioFormat);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000DE77 File Offset: 0x0000C077
		public void SetInputToNull()
		{
			this.RecoBase.SetInput(null, null);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000DE86 File Offset: 0x0000C086
		public void SetInputToDefaultAudioDevice()
		{
			this.RecoBase.SetInputToDefaultAudioDevice();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000DE93 File Offset: 0x0000C093
		public RecognitionResult Recognize()
		{
			return this.RecoBase.Recognize(this.RecoBase.InitialSilenceTimeout);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000DEAB File Offset: 0x0000C0AB
		public RecognitionResult Recognize(TimeSpan initialSilenceTimeout)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.Recognize(initialSilenceTimeout);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public void RecognizeAsync()
		{
			this.RecognizeAsync(RecognizeMode.Single);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000DEE5 File Offset: 0x0000C0E5
		public void RecognizeAsync(RecognizeMode mode)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.RecognizeAsync(mode);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000DF16 File Offset: 0x0000C116
		public void RecognizeAsyncCancel()
		{
			this.RecoBase.RecognizeAsyncCancel();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000DF23 File Offset: 0x0000C123
		public void RecognizeAsyncStop()
		{
			this.RecoBase.RecognizeAsyncStop();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000DF30 File Offset: 0x0000C130
		public object QueryRecognizerSetting(string settingName)
		{
			return this.RecoBase.QueryRecognizerSetting(settingName);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000DF3E File Offset: 0x0000C13E
		public void UpdateRecognizerSetting(string settingName, string updatedValue)
		{
			this.RecoBase.UpdateRecognizerSetting(settingName, updatedValue);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000DF4D File Offset: 0x0000C14D
		public void UpdateRecognizerSetting(string settingName, int updatedValue)
		{
			this.RecoBase.UpdateRecognizerSetting(settingName, updatedValue);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000DF5C File Offset: 0x0000C15C
		public void LoadGrammar(Grammar grammar)
		{
			this.RecoBase.LoadGrammar(grammar);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000DF6A File Offset: 0x0000C16A
		public void LoadGrammarAsync(Grammar grammar)
		{
			this.RecoBase.LoadGrammarAsync(grammar);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000DF78 File Offset: 0x0000C178
		public void UnloadGrammar(Grammar grammar)
		{
			this.RecoBase.UnloadGrammar(grammar);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000DF86 File Offset: 0x0000C186
		public void UnloadAllGrammars()
		{
			this.RecoBase.UnloadAllGrammars();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000DF93 File Offset: 0x0000C193
		public RecognitionResult EmulateRecognize(string inputText)
		{
			return this.EmulateRecognize(inputText, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000DF9E File Offset: 0x0000C19E
		public RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.EmulateRecognize(inputText, compareOptions);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		public RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.EmulateRecognize(wordUnits, compareOptions);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E002 File Offset: 0x0000C202
		public void EmulateRecognizeAsync(string inputText)
		{
			this.EmulateRecognizeAsync(inputText, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E00D File Offset: 0x0000C20D
		public void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.EmulateRecognizeAsync(inputText, compareOptions);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E03F File Offset: 0x0000C23F
		public void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.EmulateRecognizeAsync(wordUnits, compareOptions);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E071 File Offset: 0x0000C271
		public void RequestRecognizerUpdate()
		{
			this.RecoBase.RequestRecognizerUpdate();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E07E File Offset: 0x0000C27E
		public void RequestRecognizerUpdate(object userToken)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E08C File Offset: 0x0000C28C
		public void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken, audioPositionAheadToRaiseUpdate);
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600032E RID: 814 RVA: 0x0000E09C File Offset: 0x0000C29C
		// (remove) Token: 0x0600032F RID: 815 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		public event EventHandler<RecognizeCompletedEventArgs> RecognizeCompleted;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000330 RID: 816 RVA: 0x0000E10C File Offset: 0x0000C30C
		// (remove) Token: 0x06000331 RID: 817 RVA: 0x0000E144 File Offset: 0x0000C344
		public event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000332 RID: 818 RVA: 0x0000E17C File Offset: 0x0000C37C
		// (remove) Token: 0x06000333 RID: 819 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
		public event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000334 RID: 820 RVA: 0x0000E1EC File Offset: 0x0000C3EC
		// (remove) Token: 0x06000335 RID: 821 RVA: 0x0000E224 File Offset: 0x0000C424
		public event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000336 RID: 822 RVA: 0x0000E25C File Offset: 0x0000C45C
		// (remove) Token: 0x06000337 RID: 823 RVA: 0x0000E294 File Offset: 0x0000C494
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000338 RID: 824 RVA: 0x0000E2CC File Offset: 0x0000C4CC
		// (remove) Token: 0x06000339 RID: 825 RVA: 0x0000E304 File Offset: 0x0000C504
		public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600033A RID: 826 RVA: 0x0000E33C File Offset: 0x0000C53C
		// (remove) Token: 0x0600033B RID: 827 RVA: 0x0000E374 File Offset: 0x0000C574
		public event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600033C RID: 828 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		// (remove) Token: 0x0600033D RID: 829 RVA: 0x0000E3FC File Offset: 0x0000C5FC
		public event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._speechHypothesizedDelegate == null)
				{
					this.RecoBase.SpeechHypothesized += this.SpeechHypothesizedProxy;
				}
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Combine(this._speechHypothesizedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Remove(this._speechHypothesizedDelegate, value);
				if (this._speechHypothesizedDelegate == null)
				{
					this.RecoBase.SpeechHypothesized -= this.SpeechHypothesizedProxy;
				}
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x0600033E RID: 830 RVA: 0x0000E44C File Offset: 0x0000C64C
		// (remove) Token: 0x0600033F RID: 831 RVA: 0x0000E49C File Offset: 0x0000C69C
		public event EventHandler<AudioSignalProblemOccurredEventArgs> AudioSignalProblemOccurred
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RecoBase.AudioSignalProblemOccurred += this.AudioSignalProblemOccurredProxy;
				}
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Combine(this._audioSignalProblemOccurredDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Remove(this._audioSignalProblemOccurredDelegate, value);
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RecoBase.AudioSignalProblemOccurred -= this.AudioSignalProblemOccurredProxy;
				}
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000340 RID: 832 RVA: 0x0000E4EC File Offset: 0x0000C6EC
		// (remove) Token: 0x06000341 RID: 833 RVA: 0x0000E53C File Offset: 0x0000C73C
		public event EventHandler<AudioLevelUpdatedEventArgs> AudioLevelUpdated
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RecoBase.AudioLevelUpdated += this.AudioLevelUpdatedProxy;
				}
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Combine(this._audioLevelUpdatedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Remove(this._audioLevelUpdatedDelegate, value);
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RecoBase.AudioLevelUpdated -= this.AudioLevelUpdatedProxy;
				}
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000342 RID: 834 RVA: 0x0000E58C File Offset: 0x0000C78C
		// (remove) Token: 0x06000343 RID: 835 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public event EventHandler<AudioStateChangedEventArgs> AudioStateChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioStateChangedDelegate == null)
				{
					this.RecoBase.AudioStateChanged += this.AudioStateChangedProxy;
				}
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Combine(this._audioStateChangedDelegate, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Remove(this._audioStateChangedDelegate, value);
				if (this._audioStateChangedDelegate == null)
				{
					this.RecoBase.AudioStateChanged -= this.AudioStateChangedProxy;
				}
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E62C File Offset: 0x0000C82C
		private void Initialize(RecognizerInfo recognizerInfo)
		{
			try
			{
				this._sapiRecognizer = new SapiRecognizer(SapiRecognizer.RecognizerType.InProc);
			}
			catch (COMException ex)
			{
				throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
			}
			if (recognizerInfo != null)
			{
				ObjectToken objectToken = recognizerInfo.GetObjectToken();
				if (objectToken == null)
				{
					throw new ArgumentException(SR.Get(SRID.NullParamIllegal, new object[0]), "recognizerInfo");
				}
				try
				{
					this._sapiRecognizer.SetRecognizer(objectToken.SAPIToken);
				}
				catch (COMException ex2)
				{
					throw new ArgumentException(SR.Get(SRID.RecognizerNotFound, new object[0]), RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex2));
				}
			}
			this._sapiRecognizer.SetRecoState(SPRECOSTATE.SPRST_INACTIVE);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		private void RecognizeCompletedProxy(object sender, RecognizeCompletedEventArgs e)
		{
			EventHandler<RecognizeCompletedEventArgs> recognizeCompleted = this.RecognizeCompleted;
			if (recognizeCompleted != null)
			{
				recognizeCompleted(this, e);
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000E6EC File Offset: 0x0000C8EC
		private void EmulateRecognizeCompletedProxy(object sender, EmulateRecognizeCompletedEventArgs e)
		{
			EventHandler<EmulateRecognizeCompletedEventArgs> emulateRecognizeCompleted = this.EmulateRecognizeCompleted;
			if (emulateRecognizeCompleted != null)
			{
				emulateRecognizeCompleted(this, e);
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E70C File Offset: 0x0000C90C
		private void LoadGrammarCompletedProxy(object sender, LoadGrammarCompletedEventArgs e)
		{
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted(this, e);
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000E72C File Offset: 0x0000C92C
		private void SpeechDetectedProxy(object sender, SpeechDetectedEventArgs e)
		{
			EventHandler<SpeechDetectedEventArgs> speechDetected = this.SpeechDetected;
			if (speechDetected != null)
			{
				speechDetected(this, e);
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000E74C File Offset: 0x0000C94C
		private void SpeechRecognizedProxy(object sender, SpeechRecognizedEventArgs e)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized(this, e);
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E76C File Offset: 0x0000C96C
		private void SpeechRecognitionRejectedProxy(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			if (speechRecognitionRejected != null)
			{
				speechRecognitionRejected(this, e);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000E78C File Offset: 0x0000C98C
		private void RecognizerUpdateReachedProxy(object sender, RecognizerUpdateReachedEventArgs e)
		{
			EventHandler<RecognizerUpdateReachedEventArgs> recognizerUpdateReached = this.RecognizerUpdateReached;
			if (recognizerUpdateReached != null)
			{
				recognizerUpdateReached(this, e);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		private void SpeechHypothesizedProxy(object sender, SpeechHypothesizedEventArgs e)
		{
			EventHandler<SpeechHypothesizedEventArgs> speechHypothesizedDelegate = this._speechHypothesizedDelegate;
			if (speechHypothesizedDelegate != null)
			{
				speechHypothesizedDelegate(this, e);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E7CC File Offset: 0x0000C9CC
		private void AudioSignalProblemOccurredProxy(object sender, AudioSignalProblemOccurredEventArgs e)
		{
			EventHandler<AudioSignalProblemOccurredEventArgs> audioSignalProblemOccurredDelegate = this._audioSignalProblemOccurredDelegate;
			if (audioSignalProblemOccurredDelegate != null)
			{
				audioSignalProblemOccurredDelegate(this, e);
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E7EC File Offset: 0x0000C9EC
		private void AudioLevelUpdatedProxy(object sender, AudioLevelUpdatedEventArgs e)
		{
			EventHandler<AudioLevelUpdatedEventArgs> audioLevelUpdatedDelegate = this._audioLevelUpdatedDelegate;
			if (audioLevelUpdatedDelegate != null)
			{
				audioLevelUpdatedDelegate(this, e);
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E80C File Offset: 0x0000CA0C
		private void AudioStateChangedProxy(object sender, AudioStateChangedEventArgs e)
		{
			EventHandler<AudioStateChangedEventArgs> audioStateChangedDelegate = this._audioStateChangedDelegate;
			if (audioStateChangedDelegate != null)
			{
				audioStateChangedDelegate(this, e);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000E82C File Offset: 0x0000CA2C
		private RecognizerBase RecoBase
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("SpeechRecognitionEngine");
				}
				if (this._recognizerBase == null)
				{
					this._recognizerBase = new RecognizerBase();
					this._recognizerBase.Initialize(this._sapiRecognizer, true);
					this._recognizerBase.RecognizeCompleted += this.RecognizeCompletedProxy;
					this._recognizerBase.EmulateRecognizeCompleted += this.EmulateRecognizeCompletedProxy;
					this._recognizerBase.LoadGrammarCompleted += this.LoadGrammarCompletedProxy;
					this._recognizerBase.SpeechDetected += this.SpeechDetectedProxy;
					this._recognizerBase.SpeechRecognized += this.SpeechRecognizedProxy;
					this._recognizerBase.SpeechRecognitionRejected += this.SpeechRecognitionRejectedProxy;
					this._recognizerBase.RecognizerUpdateReached += this.RecognizerUpdateReachedProxy;
				}
				return this._recognizerBase;
			}
		}

		// Token: 0x040003AA RID: 938
		private bool _disposed;

		// Token: 0x040003AB RID: 939
		private RecognizerBase _recognizerBase;

		// Token: 0x040003AC RID: 940
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x040003AD RID: 941
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x040003AE RID: 942
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x040003AF RID: 943
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x040003B0 RID: 944
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;
	}
}
