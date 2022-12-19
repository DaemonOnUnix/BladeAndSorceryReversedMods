using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000070 RID: 112
	public class SpeechRecognizer : IDisposable
	{
		// Token: 0x06000351 RID: 849 RVA: 0x0000E91B File Offset: 0x0000CB1B
		public SpeechRecognizer()
		{
			this._sapiRecognizer = new SapiRecognizer(SapiRecognizer.RecognizerType.Shared);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000E92F File Offset: 0x0000CB2F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000E940 File Offset: 0x0000CB40
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000E993 File Offset: 0x0000CB93
		public RecognizerState State
		{
			get
			{
				return this.RecoBase.State;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000E9AD File Offset: 0x0000CBAD
		public bool Enabled
		{
			get
			{
				return this.RecoBase.Enabled;
			}
			set
			{
				this.RecoBase.Enabled = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000E9BB File Offset: 0x0000CBBB
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		public bool PauseRecognizerOnRecognition
		{
			get
			{
				return this.RecoBase.PauseRecognizerOnRecognition;
			}
			set
			{
				this.RecoBase.PauseRecognizerOnRecognition = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000E9D6 File Offset: 0x0000CBD6
		public ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this.RecoBase.Grammars;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000E9E3 File Offset: 0x0000CBE3
		public RecognizerInfo RecognizerInfo
		{
			get
			{
				return this.RecoBase.RecognizerInfo;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public AudioState AudioState
		{
			get
			{
				return this.RecoBase.AudioState;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000E9FD File Offset: 0x0000CBFD
		public int AudioLevel
		{
			get
			{
				return this.RecoBase.AudioLevel;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000EA0A File Offset: 0x0000CC0A
		public TimeSpan AudioPosition
		{
			get
			{
				return this.RecoBase.AudioPosition;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000EA17 File Offset: 0x0000CC17
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this.RecoBase.RecognizerAudioPosition;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				return this.RecoBase.AudioFormat;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000EA31 File Offset: 0x0000CC31
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000EA3E File Offset: 0x0000CC3E
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

		// Token: 0x06000362 RID: 866 RVA: 0x0000EA4C File Offset: 0x0000CC4C
		public void LoadGrammar(Grammar grammar)
		{
			this.RecoBase.LoadGrammar(grammar);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000EA5A File Offset: 0x0000CC5A
		public void LoadGrammarAsync(Grammar grammar)
		{
			this.RecoBase.LoadGrammarAsync(grammar);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000EA68 File Offset: 0x0000CC68
		public void UnloadGrammar(Grammar grammar)
		{
			this.RecoBase.UnloadGrammar(grammar);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000EA76 File Offset: 0x0000CC76
		public void UnloadAllGrammars()
		{
			this.RecoBase.UnloadAllGrammars();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000EA83 File Offset: 0x0000CC83
		public RecognitionResult EmulateRecognize(string inputText)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(inputText);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000EAAF File Offset: 0x0000CCAF
		public RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(inputText, compareOptions);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EADC File Offset: 0x0000CCDC
		public RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(wordUnits, compareOptions);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000EB09 File Offset: 0x0000CD09
		public void EmulateRecognizeAsync(string inputText)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(inputText);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000EB35 File Offset: 0x0000CD35
		public void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(inputText, compareOptions);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000EB62 File Offset: 0x0000CD62
		public void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(wordUnits, compareOptions);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000EB8F File Offset: 0x0000CD8F
		public void RequestRecognizerUpdate()
		{
			this.RecoBase.RequestRecognizerUpdate();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000EB9C File Offset: 0x0000CD9C
		public void RequestRecognizerUpdate(object userToken)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000EBAA File Offset: 0x0000CDAA
		public void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken, audioPositionAheadToRaiseUpdate);
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600036F RID: 879 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		// (remove) Token: 0x06000370 RID: 880 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
		public event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000371 RID: 881 RVA: 0x0000EC2C File Offset: 0x0000CE2C
		// (remove) Token: 0x06000372 RID: 882 RVA: 0x0000EC64 File Offset: 0x0000CE64
		public event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000373 RID: 883 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		// (remove) Token: 0x06000374 RID: 884 RVA: 0x0000ECD4 File Offset: 0x0000CED4
		public event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000375 RID: 885 RVA: 0x0000ED0C File Offset: 0x0000CF0C
		// (remove) Token: 0x06000376 RID: 886 RVA: 0x0000ED44 File Offset: 0x0000CF44
		public event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000377 RID: 887 RVA: 0x0000ED7C File Offset: 0x0000CF7C
		// (remove) Token: 0x06000378 RID: 888 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000379 RID: 889 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		// (remove) Token: 0x0600037A RID: 890 RVA: 0x0000EE24 File Offset: 0x0000D024
		public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x0600037B RID: 891 RVA: 0x0000EE5C File Offset: 0x0000D05C
		// (remove) Token: 0x0600037C RID: 892 RVA: 0x0000EE94 File Offset: 0x0000D094
		public event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600037D RID: 893 RVA: 0x0000EECC File Offset: 0x0000D0CC
		// (remove) Token: 0x0600037E RID: 894 RVA: 0x0000EF1C File Offset: 0x0000D11C
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

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600037F RID: 895 RVA: 0x0000EF6C File Offset: 0x0000D16C
		// (remove) Token: 0x06000380 RID: 896 RVA: 0x0000EFBC File Offset: 0x0000D1BC
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

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000381 RID: 897 RVA: 0x0000F00C File Offset: 0x0000D20C
		// (remove) Token: 0x06000382 RID: 898 RVA: 0x0000F05C File Offset: 0x0000D25C
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

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000383 RID: 899 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		// (remove) Token: 0x06000384 RID: 900 RVA: 0x0000F0FC File Offset: 0x0000D2FC
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

		// Token: 0x06000385 RID: 901 RVA: 0x0000F14C File Offset: 0x0000D34C
		private void StateChangedProxy(object sender, StateChangedEventArgs e)
		{
			EventHandler<StateChangedEventArgs> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this, e);
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F16C File Offset: 0x0000D36C
		private void EmulateRecognizeCompletedProxy(object sender, EmulateRecognizeCompletedEventArgs e)
		{
			EventHandler<EmulateRecognizeCompletedEventArgs> emulateRecognizeCompleted = this.EmulateRecognizeCompleted;
			if (emulateRecognizeCompleted != null)
			{
				emulateRecognizeCompleted(this, e);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F18C File Offset: 0x0000D38C
		private void LoadGrammarCompletedProxy(object sender, LoadGrammarCompletedEventArgs e)
		{
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted(this, e);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		private void SpeechDetectedProxy(object sender, SpeechDetectedEventArgs e)
		{
			EventHandler<SpeechDetectedEventArgs> speechDetected = this.SpeechDetected;
			if (speechDetected != null)
			{
				speechDetected(this, e);
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		private void SpeechRecognizedProxy(object sender, SpeechRecognizedEventArgs e)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized(this, e);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F1EC File Offset: 0x0000D3EC
		private void SpeechRecognitionRejectedProxy(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			if (speechRecognitionRejected != null)
			{
				speechRecognitionRejected(this, e);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F20C File Offset: 0x0000D40C
		private void RecognizerUpdateReachedProxy(object sender, RecognizerUpdateReachedEventArgs e)
		{
			EventHandler<RecognizerUpdateReachedEventArgs> recognizerUpdateReached = this.RecognizerUpdateReached;
			if (recognizerUpdateReached != null)
			{
				recognizerUpdateReached(this, e);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000F22C File Offset: 0x0000D42C
		private void SpeechHypothesizedProxy(object sender, SpeechHypothesizedEventArgs e)
		{
			EventHandler<SpeechHypothesizedEventArgs> speechHypothesizedDelegate = this._speechHypothesizedDelegate;
			if (speechHypothesizedDelegate != null)
			{
				speechHypothesizedDelegate(this, e);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000F24C File Offset: 0x0000D44C
		private void AudioSignalProblemOccurredProxy(object sender, AudioSignalProblemOccurredEventArgs e)
		{
			EventHandler<AudioSignalProblemOccurredEventArgs> audioSignalProblemOccurredDelegate = this._audioSignalProblemOccurredDelegate;
			if (audioSignalProblemOccurredDelegate != null)
			{
				audioSignalProblemOccurredDelegate(this, e);
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F26C File Offset: 0x0000D46C
		private void AudioLevelUpdatedProxy(object sender, AudioLevelUpdatedEventArgs e)
		{
			EventHandler<AudioLevelUpdatedEventArgs> audioLevelUpdatedDelegate = this._audioLevelUpdatedDelegate;
			if (audioLevelUpdatedDelegate != null)
			{
				audioLevelUpdatedDelegate(this, e);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F28C File Offset: 0x0000D48C
		private void AudioStateChangedProxy(object sender, AudioStateChangedEventArgs e)
		{
			EventHandler<AudioStateChangedEventArgs> audioStateChangedDelegate = this._audioStateChangedDelegate;
			if (audioStateChangedDelegate != null)
			{
				audioStateChangedDelegate(this, e);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000F2AC File Offset: 0x0000D4AC
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
					try
					{
						this._recognizerBase.Initialize(this._sapiRecognizer, false);
					}
					catch (COMException ex)
					{
						throw RecognizerBase.ExceptionFromSapiCreateRecognizerError(ex);
					}
					this.PauseRecognizerOnRecognition = false;
					this._recognizerBase._haveInputSource = true;
					if (this.AudioPosition != TimeSpan.Zero)
					{
						this._recognizerBase.AudioState = AudioState.Silence;
					}
					this._recognizerBase.StateChanged += this.StateChangedProxy;
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

		// Token: 0x040003B8 RID: 952
		private bool _disposed;

		// Token: 0x040003B9 RID: 953
		private RecognizerBase _recognizerBase;

		// Token: 0x040003BA RID: 954
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x040003BB RID: 955
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x040003BC RID: 956
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x040003BD RID: 957
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x040003BE RID: 958
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;
	}
}
