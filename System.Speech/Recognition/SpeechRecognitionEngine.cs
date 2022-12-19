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
	// Token: 0x020001A3 RID: 419
	public class SpeechRecognitionEngine : IDisposable
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x0002F933 File Offset: 0x0002E933
		public SpeechRecognitionEngine()
		{
			this.Initialize(null);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002F944 File Offset: 0x0002E944
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

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002FA3C File Offset: 0x0002EA3C
		public SpeechRecognitionEngine(string recognizerId)
		{
			Helpers.ThrowIfEmptyOrNull(recognizerId, "recognizerId");
			foreach (RecognizerInfo recognizerInfo in SpeechRecognitionEngine.InstalledRecognizers())
			{
				if (recognizerId.Equals(recognizerInfo.Id, 5))
				{
					this.Initialize(recognizerInfo);
					return;
				}
			}
			throw new ArgumentException(SR.Get(SRID.RecognizerNotFound, new object[0]), "recognizerId");
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002FAC8 File Offset: 0x0002EAC8
		public SpeechRecognitionEngine(RecognizerInfo recognizerInfo)
		{
			Helpers.ThrowIfNull(recognizerInfo, "recognizerInfo");
			this.Initialize(recognizerInfo);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002FAE2 File Offset: 0x0002EAE2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002FAF4 File Offset: 0x0002EAF4
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

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002FB48 File Offset: 0x0002EB48
		public static ReadOnlyCollection<RecognizerInfo> InstalledRecognizers()
		{
			List<RecognizerInfo> list = new List<RecognizerInfo>();
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Recognizers"))
			{
				if (objectTokenCategory != null)
				{
					foreach (ObjectToken objectToken in objectTokenCategory)
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0002FBD4 File Offset: 0x0002EBD4
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x0002FBE1 File Offset: 0x0002EBE1
		[EditorBrowsable(2)]
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

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002FBEF File Offset: 0x0002EBEF
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x0002FBFC File Offset: 0x0002EBFC
		[EditorBrowsable(2)]
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

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0002FC0A File Offset: 0x0002EC0A
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x0002FC24 File Offset: 0x0002EC24
		[EditorBrowsable(2)]
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0002FC88 File Offset: 0x0002EC88
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x0002FCA0 File Offset: 0x0002ECA0
		[EditorBrowsable(2)]
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

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0002FD04 File Offset: 0x0002ED04
		public ReadOnlyCollection<Grammar> Grammars
		{
			get
			{
				return this.RecoBase.Grammars;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0002FD11 File Offset: 0x0002ED11
		public RecognizerInfo RecognizerInfo
		{
			get
			{
				return this.RecoBase.RecognizerInfo;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002FD1E File Offset: 0x0002ED1E
		public AudioState AudioState
		{
			get
			{
				return this.RecoBase.AudioState;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0002FD2B File Offset: 0x0002ED2B
		public int AudioLevel
		{
			get
			{
				return this.RecoBase.AudioLevel;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0002FD38 File Offset: 0x0002ED38
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this.RecoBase.RecognizerAudioPosition;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0002FD45 File Offset: 0x0002ED45
		public TimeSpan AudioPosition
		{
			get
			{
				return this.RecoBase.AudioPosition;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0002FD52 File Offset: 0x0002ED52
		public SpeechAudioFormatInfo AudioFormat
		{
			get
			{
				return this.RecoBase.AudioFormat;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0002FD5F File Offset: 0x0002ED5F
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x0002FD6C File Offset: 0x0002ED6C
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

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002FD7A File Offset: 0x0002ED7A
		public void SetInputToWaveFile(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.RecoBase.SetInput(path);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002FD93 File Offset: 0x0002ED93
		public void SetInputToWaveStream(Stream audioSource)
		{
			this.RecoBase.SetInput(audioSource, null);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002FDA2 File Offset: 0x0002EDA2
		public void SetInputToAudioStream(Stream audioSource, SpeechAudioFormatInfo audioFormat)
		{
			Helpers.ThrowIfNull(audioSource, "audioSource");
			Helpers.ThrowIfNull(audioFormat, "audioFormat");
			this.RecoBase.SetInput(audioSource, audioFormat);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002FDC7 File Offset: 0x0002EDC7
		public void SetInputToNull()
		{
			this.RecoBase.SetInput(null, null);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002FDD6 File Offset: 0x0002EDD6
		public void SetInputToDefaultAudioDevice()
		{
			this.RecoBase.SetInputToDefaultAudioDevice();
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002FDE3 File Offset: 0x0002EDE3
		public RecognitionResult Recognize()
		{
			return this.RecoBase.Recognize(this.RecoBase.InitialSilenceTimeout);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002FDFB File Offset: 0x0002EDFB
		public RecognitionResult Recognize(TimeSpan initialSilenceTimeout)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.Recognize(initialSilenceTimeout);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002FE2C File Offset: 0x0002EE2C
		public void RecognizeAsync()
		{
			this.RecognizeAsync(RecognizeMode.Single);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002FE35 File Offset: 0x0002EE35
		public void RecognizeAsync(RecognizeMode mode)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.RecognizeAsync(mode);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0002FE66 File Offset: 0x0002EE66
		public void RecognizeAsyncCancel()
		{
			this.RecoBase.RecognizeAsyncCancel();
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002FE73 File Offset: 0x0002EE73
		public void RecognizeAsyncStop()
		{
			this.RecoBase.RecognizeAsyncStop();
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002FE80 File Offset: 0x0002EE80
		public object QueryRecognizerSetting(string settingName)
		{
			return this.RecoBase.QueryRecognizerSetting(settingName);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002FE8E File Offset: 0x0002EE8E
		public void UpdateRecognizerSetting(string settingName, string updatedValue)
		{
			this.RecoBase.UpdateRecognizerSetting(settingName, updatedValue);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002FE9D File Offset: 0x0002EE9D
		public void UpdateRecognizerSetting(string settingName, int updatedValue)
		{
			this.RecoBase.UpdateRecognizerSetting(settingName, updatedValue);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0002FEAC File Offset: 0x0002EEAC
		public void LoadGrammar(Grammar grammar)
		{
			this.RecoBase.LoadGrammar(grammar);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002FEBA File Offset: 0x0002EEBA
		public void LoadGrammarAsync(Grammar grammar)
		{
			this.RecoBase.LoadGrammarAsync(grammar);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002FEC8 File Offset: 0x0002EEC8
		public void UnloadGrammar(Grammar grammar)
		{
			this.RecoBase.UnloadGrammar(grammar);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002FED6 File Offset: 0x0002EED6
		public void UnloadAllGrammars()
		{
			this.RecoBase.UnloadAllGrammars();
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002FEE3 File Offset: 0x0002EEE3
		public RecognitionResult EmulateRecognize(string inputText)
		{
			return this.EmulateRecognize(inputText, 25);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002FEEE File Offset: 0x0002EEEE
		public RecognitionResult EmulateRecognize(string inputText, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.EmulateRecognize(inputText, compareOptions);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002FF20 File Offset: 0x0002EF20
		public RecognitionResult EmulateRecognize(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			return this.RecoBase.EmulateRecognize(wordUnits, compareOptions);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002FF52 File Offset: 0x0002EF52
		public void EmulateRecognizeAsync(string inputText)
		{
			this.EmulateRecognizeAsync(inputText, 25);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002FF5D File Offset: 0x0002EF5D
		public void EmulateRecognizeAsync(string inputText, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.EmulateRecognizeAsync(inputText, compareOptions);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002FF8F File Offset: 0x0002EF8F
		public void EmulateRecognizeAsync(RecognizedWordUnit[] wordUnits, CompareOptions compareOptions)
		{
			if (this.Grammars.Count == 0)
			{
				throw new InvalidOperationException(SR.Get(SRID.RecognizerHasNoGrammar, new object[0]));
			}
			this.RecoBase.EmulateRecognizeAsync(wordUnits, compareOptions);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002FFC1 File Offset: 0x0002EFC1
		public void RequestRecognizerUpdate()
		{
			this.RecoBase.RequestRecognizerUpdate();
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002FFCE File Offset: 0x0002EFCE
		public void RequestRecognizerUpdate(object userToken)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002FFDC File Offset: 0x0002EFDC
		public void RequestRecognizerUpdate(object userToken, TimeSpan audioPositionAheadToRaiseUpdate)
		{
			this.RecoBase.RequestRecognizerUpdate(userToken, audioPositionAheadToRaiseUpdate);
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000B21 RID: 2849 RVA: 0x0002FFEB File Offset: 0x0002EFEB
		// (remove) Token: 0x06000B22 RID: 2850 RVA: 0x00030004 File Offset: 0x0002F004
		public event EventHandler<RecognizeCompletedEventArgs> RecognizeCompleted;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000B23 RID: 2851 RVA: 0x0003001D File Offset: 0x0002F01D
		// (remove) Token: 0x06000B24 RID: 2852 RVA: 0x00030036 File Offset: 0x0002F036
		public event EventHandler<EmulateRecognizeCompletedEventArgs> EmulateRecognizeCompleted;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000B25 RID: 2853 RVA: 0x0003004F File Offset: 0x0002F04F
		// (remove) Token: 0x06000B26 RID: 2854 RVA: 0x00030068 File Offset: 0x0002F068
		public event EventHandler<LoadGrammarCompletedEventArgs> LoadGrammarCompleted;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000B27 RID: 2855 RVA: 0x00030081 File Offset: 0x0002F081
		// (remove) Token: 0x06000B28 RID: 2856 RVA: 0x0003009A File Offset: 0x0002F09A
		public event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000B29 RID: 2857 RVA: 0x000300B3 File Offset: 0x0002F0B3
		// (remove) Token: 0x06000B2A RID: 2858 RVA: 0x000300CC File Offset: 0x0002F0CC
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000B2B RID: 2859 RVA: 0x000300E5 File Offset: 0x0002F0E5
		// (remove) Token: 0x06000B2C RID: 2860 RVA: 0x000300FE File Offset: 0x0002F0FE
		public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRecognitionRejected;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000B2D RID: 2861 RVA: 0x00030117 File Offset: 0x0002F117
		// (remove) Token: 0x06000B2E RID: 2862 RVA: 0x00030130 File Offset: 0x0002F130
		public event EventHandler<RecognizerUpdateReachedEventArgs> RecognizerUpdateReached;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000B2F RID: 2863 RVA: 0x0003014C File Offset: 0x0002F14C
		// (remove) Token: 0x06000B30 RID: 2864 RVA: 0x0003019C File Offset: 0x0002F19C
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

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000B31 RID: 2865 RVA: 0x000301EC File Offset: 0x0002F1EC
		// (remove) Token: 0x06000B32 RID: 2866 RVA: 0x0003023C File Offset: 0x0002F23C
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

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000B33 RID: 2867 RVA: 0x0003028C File Offset: 0x0002F28C
		// (remove) Token: 0x06000B34 RID: 2868 RVA: 0x000302DC File Offset: 0x0002F2DC
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

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000B35 RID: 2869 RVA: 0x0003032C File Offset: 0x0002F32C
		// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x0003037C File Offset: 0x0002F37C
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

		// Token: 0x06000B37 RID: 2871 RVA: 0x000303CC File Offset: 0x0002F3CC
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

		// Token: 0x06000B38 RID: 2872 RVA: 0x0003046C File Offset: 0x0002F46C
		private void RecognizeCompletedProxy(object sender, RecognizeCompletedEventArgs e)
		{
			EventHandler<RecognizeCompletedEventArgs> recognizeCompleted = this.RecognizeCompleted;
			if (recognizeCompleted != null)
			{
				recognizeCompleted.Invoke(this, e);
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0003048C File Offset: 0x0002F48C
		private void EmulateRecognizeCompletedProxy(object sender, EmulateRecognizeCompletedEventArgs e)
		{
			EventHandler<EmulateRecognizeCompletedEventArgs> emulateRecognizeCompleted = this.EmulateRecognizeCompleted;
			if (emulateRecognizeCompleted != null)
			{
				emulateRecognizeCompleted.Invoke(this, e);
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000304AC File Offset: 0x0002F4AC
		private void LoadGrammarCompletedProxy(object sender, LoadGrammarCompletedEventArgs e)
		{
			EventHandler<LoadGrammarCompletedEventArgs> loadGrammarCompleted = this.LoadGrammarCompleted;
			if (loadGrammarCompleted != null)
			{
				loadGrammarCompleted.Invoke(this, e);
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000304CC File Offset: 0x0002F4CC
		private void SpeechDetectedProxy(object sender, SpeechDetectedEventArgs e)
		{
			EventHandler<SpeechDetectedEventArgs> speechDetected = this.SpeechDetected;
			if (speechDetected != null)
			{
				speechDetected.Invoke(this, e);
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000304EC File Offset: 0x0002F4EC
		private void SpeechRecognizedProxy(object sender, SpeechRecognizedEventArgs e)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized.Invoke(this, e);
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003050C File Offset: 0x0002F50C
		private void SpeechRecognitionRejectedProxy(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			EventHandler<SpeechRecognitionRejectedEventArgs> speechRecognitionRejected = this.SpeechRecognitionRejected;
			if (speechRecognitionRejected != null)
			{
				speechRecognitionRejected.Invoke(this, e);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003052C File Offset: 0x0002F52C
		private void RecognizerUpdateReachedProxy(object sender, RecognizerUpdateReachedEventArgs e)
		{
			EventHandler<RecognizerUpdateReachedEventArgs> recognizerUpdateReached = this.RecognizerUpdateReached;
			if (recognizerUpdateReached != null)
			{
				recognizerUpdateReached.Invoke(this, e);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003054C File Offset: 0x0002F54C
		private void SpeechHypothesizedProxy(object sender, SpeechHypothesizedEventArgs e)
		{
			EventHandler<SpeechHypothesizedEventArgs> speechHypothesizedDelegate = this._speechHypothesizedDelegate;
			if (speechHypothesizedDelegate != null)
			{
				speechHypothesizedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0003056C File Offset: 0x0002F56C
		private void AudioSignalProblemOccurredProxy(object sender, AudioSignalProblemOccurredEventArgs e)
		{
			EventHandler<AudioSignalProblemOccurredEventArgs> audioSignalProblemOccurredDelegate = this._audioSignalProblemOccurredDelegate;
			if (audioSignalProblemOccurredDelegate != null)
			{
				audioSignalProblemOccurredDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003058C File Offset: 0x0002F58C
		private void AudioLevelUpdatedProxy(object sender, AudioLevelUpdatedEventArgs e)
		{
			EventHandler<AudioLevelUpdatedEventArgs> audioLevelUpdatedDelegate = this._audioLevelUpdatedDelegate;
			if (audioLevelUpdatedDelegate != null)
			{
				audioLevelUpdatedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000305AC File Offset: 0x0002F5AC
		private void AudioStateChangedProxy(object sender, AudioStateChangedEventArgs e)
		{
			EventHandler<AudioStateChangedEventArgs> audioStateChangedDelegate = this._audioStateChangedDelegate;
			if (audioStateChangedDelegate != null)
			{
				audioStateChangedDelegate.Invoke(this, e);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000305CC File Offset: 0x0002F5CC
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
					this._recognizerBase.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(this.RecognizeCompletedProxy);
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

		// Token: 0x0400096E RID: 2414
		private bool _disposed;

		// Token: 0x0400096F RID: 2415
		private RecognizerBase _recognizerBase;

		// Token: 0x04000970 RID: 2416
		private SapiRecognizer _sapiRecognizer;

		// Token: 0x04000971 RID: 2417
		private EventHandler<AudioSignalProblemOccurredEventArgs> _audioSignalProblemOccurredDelegate;

		// Token: 0x04000972 RID: 2418
		private EventHandler<AudioLevelUpdatedEventArgs> _audioLevelUpdatedDelegate;

		// Token: 0x04000973 RID: 2419
		private EventHandler<AudioStateChangedEventArgs> _audioStateChangedDelegate;

		// Token: 0x04000974 RID: 2420
		private EventHandler<SpeechHypothesizedEventArgs> _speechHypothesizedDelegate;
	}
}
