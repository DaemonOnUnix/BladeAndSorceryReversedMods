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
	// Token: 0x020001A4 RID: 420
	public class SpeechRecognizer : IDisposable
	{
		// Token: 0x06000B44 RID: 2884 RVA: 0x000306BB File Offset: 0x0002F6BB
		public SpeechRecognizer()
		{
			this._sapiRecognizer = new SapiRecognizer(SapiRecognizer.RecognizerType.Shared);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000306CF File Offset: 0x0002F6CF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000306E0 File Offset: 0x0002F6E0
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00030733 File Offset: 0x0002F733
		public RecognizerState State
		{
			get
			{
				return this.RecoBase.State;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00030740 File Offset: 0x0002F740
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0003074D File Offset: 0x0002F74D
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

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0003075B File Offset: 0x0002F75B
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x00030768 File Offset: 0x0002F768
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00030776 File Offset: 0x0002F776
		public ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this.RecoBase.Grammars;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00030783 File Offset: 0x0002F783
		public RecognizerInfo RecognizerInfo
		{
			get
			{
				return this.RecoBase.RecognizerInfo;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00030790 File Offset: 0x0002F790
		public AudioState AudioState
		{
			get
			{
				return this.RecoBase.AudioState;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0003079D File Offset: 0x0002F79D
		public int AudioLevel
		{
			get
			{
				return this.RecoBase.AudioLevel;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x000307AA File Offset: 0x0002F7AA
		public TimeSpan AudioPosition
		{
			get
			{
				return this.RecoBase.AudioPosition;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x000307B7 File Offset: 0x0002F7B7
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this.RecoBase.RecognizerAudioPosition;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000307C4 File Offset: 0x0002F7C4
		public SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				return this.RecoBase.AudioFormat;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x000307D1 File Offset: 0x0002F7D1
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x000307DE File Offset: 0x0002F7DE
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

		// Token: 0x06000B55 RID: 2901 RVA: 0x000307EC File Offset: 0x0002F7EC
		public void LoadGrammar(Grammar grammar)
		{
			this.RecoBase.LoadGrammar(grammar);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x000307FA File Offset: 0x0002F7FA
		public void LoadGrammarAsync(Grammar grammar)
		{
			this.RecoBase.LoadGrammarAsync(grammar);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00030808 File Offset: 0x0002F808
		public void UnloadGrammar(Grammar grammar)
		{
			this.RecoBase.UnloadGrammar(grammar);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00030816 File Offset: 0x0002F816
		public void UnloadAllGrammars()
		{
			this.RecoBase.UnloadAllGrammars();
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00030823 File Offset: 0x0002F823
		public RecognitionResult EmulateRecognize(string inputText)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(inputText);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0003084F File Offset: 0x0002F84F
		public RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(inputText, compareOptions);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0003087C File Offset: 0x0002F87C
		public RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				return this.RecoBase.EmulateRecognize(wordUnits, compareOptions);
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000308A9 File Offset: 0x0002F8A9
		public void EmulateRecognizeAsync(string inputText)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(inputText);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000308D5 File Offset: 0x0002F8D5
		public void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(inputText, compareOptions);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00030902 File Offset: 0x0002F902
		public void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Enabled)
			{
				this.RecoBase.EmulateRecognizeAsync(wordUnits, compareOptions);
				return;
			}
			throw new InvalidOperationException(SR.Get(SRID.RecognizerNotEnabled, new object[0]));
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0003092F File Offset: 0x0002F92F
		public void RequestRecognizerUpdate()
		{
			this.RecoBase.RequestRecognizerUpdate();
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0003093C File Offset: 0x0002F93C
		public void RequestRecognizerUpdate(object userToken)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0003094A File Offset: 0x0002F94A
		public void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken, audioPositionAheadToRaiseUpdate);
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000B62 RID: 2914 RVA: 0x00030959 File Offset: 0x0002F959
		// (remove) Token: 0x06000B63 RID: 2915 RVA: 0x00030972 File Offset: 0x0002F972
		public event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000B64 RID: 2916 RVA: 0x0003098B File Offset: 0x0002F98B
		// (remove) Token: 0x06000B65 RID: 2917 RVA: 0x000309A4 File Offset: 0x0002F9A4
		public event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000B66 RID: 2918 RVA: 0x000309BD File Offset: 0x0002F9BD
		// (remove) Token: 0x06000B67 RID: 2919 RVA: 0x000309D6 File Offset: 0x0002F9D6
		public event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000B68 RID: 2920 RVA: 0x000309EF File Offset: 0x0002F9EF
		// (remove) Token: 0x06000B69 RID: 2921 RVA: 0x00030A08 File Offset: 0x0002FA08
		public event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000B6A RID: 2922 RVA: 0x00030A21 File Offset: 0x0002FA21
		// (remove) Token: 0x06000B6B RID: 2923 RVA: 0x00030A3A File Offset: 0x0002FA3A
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000B6C RID: 2924 RVA: 0x00030A53 File Offset: 0x0002FA53
		// (remove) Token: 0x06000B6D RID: 2925 RVA: 0x00030A6C File Offset: 0x0002FA6C
		public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000B6E RID: 2926 RVA: 0x00030A85 File Offset: 0x0002FA85
		// (remove) Token: 0x06000B6F RID: 2927 RVA: 0x00030A9E File Offset: 0x0002FA9E
		public event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000B70 RID: 2928 RVA: 0x00030AB8 File Offset: 0x0002FAB8
		// (remove) Token: 0x06000B71 RID: 2929 RVA: 0x00030B08 File Offset: 0x0002FB08
		public event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._speechHypothesizedDelegate == null)
				{
					this.RecoBase.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(this.SpeechHypothesizedProxy);
				}
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Combine(this._speechHypothesizedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._speechHypothesizedDelegate = (EventHandler<SpeechHypothesizedEventArgs>)Delegate.Remove(this._speechHypothesizedDelegate, value);
				if (this._speechHypothesizedDelegate == null)
				{
					this.RecoBase.SpeechHypothesized -= new EventHandler<SpeechHypothesizedEventArgs>(this.SpeechHypothesizedProxy);
				}
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000B72 RID: 2930 RVA: 0x00030B58 File Offset: 0x0002FB58
		// (remove) Token: 0x06000B73 RID: 2931 RVA: 0x00030BA8 File Offset: 0x0002FBA8
		public event EventHandler<AudioSignalProblemOccurredEventArgs> AudioSignalProblemOccurred
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RecoBase.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(this.AudioSignalProblemOccurredProxy);
				}
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Combine(this._audioSignalProblemOccurredDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioSignalProblemOccurredDelegate = (EventHandler<AudioSignalProblemOccurredEventArgs>)Delegate.Remove(this._audioSignalProblemOccurredDelegate, value);
				if (this._audioSignalProblemOccurredDelegate == null)
				{
					this.RecoBase.AudioSignalProblemOccurred -= new EventHandler<AudioSignalProblemOccurredEventArgs>(this.AudioSignalProblemOccurredProxy);
				}
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000B74 RID: 2932 RVA: 0x00030BF8 File Offset: 0x0002FBF8
		// (remove) Token: 0x06000B75 RID: 2933 RVA: 0x00030C48 File Offset: 0x0002FC48
		public event EventHandler<AudioLevelUpdatedEventArgs> AudioLevelUpdated
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RecoBase.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(this.AudioLevelUpdatedProxy);
				}
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Combine(this._audioLevelUpdatedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioLevelUpdatedDelegate = (EventHandler<AudioLevelUpdatedEventArgs>)Delegate.Remove(this._audioLevelUpdatedDelegate, value);
				if (this._audioLevelUpdatedDelegate == null)
				{
					this.RecoBase.AudioLevelUpdated -= new EventHandler<AudioLevelUpdatedEventArgs>(this.AudioLevelUpdatedProxy);
				}
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000B76 RID: 2934 RVA: 0x00030C98 File Offset: 0x0002FC98
		// (remove) Token: 0x06000B77 RID: 2935 RVA: 0x00030CE8 File Offset: 0x0002FCE8
		public event EventHandler<AudioStateChangedEventArgs> AudioStateChanged
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				if (this._audioStateChangedDelegate == null)
				{
					this.RecoBase.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(this.AudioStateChangedProxy);
				}
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Combine(this._audioStateChangedDelegate, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this._audioStateChangedDelegate = (EventHandler<AudioStateChangedEventArgs>)Delegate.Remove(this._audioStateChangedDelegate, value);
				if (this._audioStateChangedDelegate == null)
				{
					this.RecoBase.AudioStateChanged -= new EventHandler<AudioStateChangedEventArgs>(this.AudioStateChangedProxy);
				}
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00030D38 File Offset: 0x0002FD38
		private void StateChangedProxy(object sender, StateChangedEventArgs e)
		{
			EventHandler<StateChangedEventArgs> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged.Invoke(this, e);
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00030D58 File Offset: 0x0002FD58
		private void EmulateRecognizeCompletedProxy(object sender, EmulateRecognizeCompletedEventArgs e)
		{
			EventHandler<EmulateRecognizeCompletedEventArgs> emulateRecognizeCompleted = this.EmulateRecognizeCompleted;
			if (emulateRecognizeCompleted != null)
			{
				emulateRecognizeCompleted.Invoke(this, e);
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00030D78 File Offset: 0x0002FD78
		private void LoadGrammarCompletedProxy(object sender, LoadGrammarCompletedEventArgs e)
		{
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted.Invoke(this, e);
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00030D98 File Offset: 0x0002FD98
		private void SpeechDetectedProxy(object sender, SpeechDetectedEventArgs e)
		{
			EventHandler<SpeechDetectedEventArgs> speechDetected = this.SpeechDetected;
			if (speechDetected != null)
			{
				speechDetected.Invoke(this, e);
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00030DB8 File Offset: 0x0002FDB8
		private void SpeechRecognizedProxy(object sender, SpeechRecognizedEventArgs e)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized.Invoke(this, e);
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00030DD8 File Offset: 0x0002FDD8
		private void SpeechRecognitionRejectedProxy(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			if (speechRecognitionRejected != null)
			{
				speechRecognitionRejected.Invoke(this, e);
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00030DF8 File Offset: 0x0002FDF8
		private void RecognizerUpdateReachedProxy(object sender, RecognizerUpdateReachedEventArgs e)
		{
			EventHandler<RecognizerUpdateReachedEventArgs> recognizerUpdateReached = this.RecognizerUpdateReached;
			if (recognizerUpdateReached != null)
			{
				recognizerUpdateReached.Invoke(this, e);
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00030E18 File Offset: 0x0002FE18
		private void SpeechHypothesizedProxy(object sender, SpeechHypothesizedEventArgs e)
		{
			EventHandler<SpeechHypothesizedEventArgs> speechHypothesizedDelegate = this._speechHypothesizedDelegate;
			if (speechHypothesizedDelegate != null)
			{
				speechHypothesizedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00030E38 File Offset: 0x0002FE38
		private void AudioSignalProblemOccurredProxy(object sender, AudioSignalProblemOccurredEventArgs e)
		{
			EventHandler<AudioSignalProblemOccurredEventArgs> audioSignalProblemOccurredDelegate = this._audioSignalProblemOccurredDelegate;
			if (audioSignalProblemOccurredDelegate != null)
			{
				audioSignalProblemOccurredDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00030E58 File Offset: 0x0002FE58
		private void AudioLevelUpdatedProxy(object sender, AudioLevelUpdatedEventArgs e)
		{
			EventHandler<AudioLevelUpdatedEventArgs> audioLevelUpdatedDelegate = this._audioLevelUpdatedDelegate;
			if (audioLevelUpdatedDelegate != null)
			{
				audioLevelUpdatedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00030E78 File Offset: 0x0002FE78
		private void AudioStateChangedProxy(object sender, AudioStateChangedEventArgs e)
		{
			EventHandler<AudioStateChangedEventArgs> audioStateChangedDelegate = this._audioStateChangedDelegate;
			if (audioStateChangedDelegate != null)
			{
				audioStateChangedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00030E98 File Offset: 0x0002FE98
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
					this._recognizerBase.StateChanged += new EventHandler<StateChangedEventArgs>(this.StateChangedProxy);
					this._recognizerBase.EmulateRecognizeCompleted += new EventHandler<EmulateRecognizeCompletedEventArgs>(this.EmulateRecognizeCompletedProxy);
					this._recognizerBase.LoadGrammarCompleted += new EventHandler<LoadGrammarCompletedEventArgs>(this.LoadGrammarCompletedProxy);
					this._recognizerBase.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(this.SpeechDetectedProxy);
					this._recognizerBase.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(this.SpeechRecognizedProxy);
					this._recognizerBase.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(this.SpeechRecognitionRejectedProxy);
					this._recognizerBase.RecognizerUpdateReached += new EventHandler<RecognizerUpdateReachedEventArgs>(this.RecognizerUpdateReachedProxy);
				}
				return this._recognizerBase;
			}
		}

		// Token: 0x0400097C RID: 2428
		private bool _disposed;

		// Token: 0x0400097D RID: 2429
		private RecognizerBase _recognizerBase;

		// Token: 0x0400097E RID: 2430
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x0400097F RID: 2431
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x04000980 RID: 2432
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x04000981 RID: 2433
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x04000982 RID: 2434
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;
	}
}
