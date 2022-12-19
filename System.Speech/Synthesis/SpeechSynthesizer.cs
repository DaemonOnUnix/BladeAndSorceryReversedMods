using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Speech.AudioFormat;
using System.Speech.Internal;
using System.Speech.Internal.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Synthesis
{
	// Token: 0x0200014F RID: 335
	public sealed class SpeechSynthesizer : IDisposable
	{
		// Token: 0x060008AD RID: 2221 RVA: 0x0002747C File Offset: 0x0002647C
		~SpeechSynthesizer()
		{
			this.Dispose(false);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000274AC File Offset: 0x000264AC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000274BC File Offset: 0x000264BC
		public void SelectVoice(string name)
		{
			Helpers.ThrowIfEmptyOrNull(name, "name");
			TTSVoice engine = this.VoiceSynthesizer.GetEngine(name, CultureInfo.CurrentUICulture, VoiceGender.NotSet, VoiceAge.NotSet, 1, true);
			if (engine == null || name != engine.VoiceInfo.Name)
			{
				throw new ArgumentException(SR.Get(SRID.SynthesizerSetVoiceNoMatch, new object[0]));
			}
			this.VoiceSynthesizer.Voice = engine;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00027522 File Offset: 0x00026522
		public void SelectVoiceByHints(VoiceGender gender)
		{
			this.SelectVoiceByHints(gender, VoiceAge.NotSet, 1, CultureInfo.CurrentUICulture);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00027532 File Offset: 0x00026532
		public void SelectVoiceByHints(VoiceGender gender, VoiceAge age)
		{
			this.SelectVoiceByHints(gender, age, 1, CultureInfo.CurrentUICulture);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00027542 File Offset: 0x00026542
		public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, int voiceAlternate)
		{
			this.SelectVoiceByHints(gender, age, voiceAlternate, CultureInfo.CurrentUICulture);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00027554 File Offset: 0x00026554
		public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, int voiceAlternate, CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (voiceAlternate < 0)
			{
				throw new ArgumentOutOfRangeException("voiceAlternate", SR.Get(SRID.PromptBuilderInvalidVariant, new object[0]));
			}
			if (!VoiceInfo.ValidateGender(gender))
			{
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "VoiceGender" }), "gender");
			}
			if (!VoiceInfo.ValidateAge(age))
			{
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "VoiceAge" }), "age");
			}
			TTSVoice engine = this.VoiceSynthesizer.GetEngine(null, culture, gender, age, voiceAlternate, true);
			if (engine == null)
			{
				throw new InvalidOperationException(SR.Get(SRID.SynthesizerSetVoiceNoMatch, new object[0]));
			}
			this.VoiceSynthesizer.Voice = engine;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00027618 File Offset: 0x00026618
		public Prompt SpeakAsync(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Prompt prompt = new Prompt(textToSpeak, SynthesisTextFormat.Text);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00027640 File Offset: 0x00026640
		public void SpeakAsync(Prompt prompt)
		{
			Helpers.ThrowIfNull(prompt, "prompt");
			prompt.Synthesizer = this;
			this.VoiceSynthesizer.SpeakAsync(prompt);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00027660 File Offset: 0x00026660
		public Prompt SpeakSsmlAsync(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Prompt prompt = new Prompt(textToSpeak, SynthesisTextFormat.Ssml);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00027688 File Offset: 0x00026688
		public Prompt SpeakAsync(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			Prompt prompt = new Prompt(promptBuilder);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000276AF File Offset: 0x000266AF
		public void Speak(string textToSpeak)
		{
			this.Speak(new Prompt(textToSpeak, SynthesisTextFormat.Text));
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000276C0 File Offset: 0x000266C0
		public void Speak(Prompt prompt)
		{
			Helpers.ThrowIfNull(prompt, "prompt");
			if (this.State == SynthesizerState.Paused)
			{
				throw new InvalidOperationException(SR.Get(SRID.SynthesizerSyncSpeakWhilePaused, new object[0]));
			}
			prompt.Synthesizer = this;
			prompt._syncSpeak = true;
			this.VoiceSynthesizer.Speak(prompt);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00027711 File Offset: 0x00026711
		public void Speak(PromptBuilder promptBuilder)
		{
			this.Speak(new Prompt(promptBuilder));
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002771F File Offset: 0x0002671F
		public void SpeakSsml(string textToSpeak)
		{
			this.Speak(new Prompt(textToSpeak, SynthesisTextFormat.Ssml));
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002772E File Offset: 0x0002672E
		public void Pause()
		{
			if (!this.paused)
			{
				this.VoiceSynthesizer.Pause();
				this.paused = true;
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002774A File Offset: 0x0002674A
		public void Resume()
		{
			if (this.paused)
			{
				this.VoiceSynthesizer.Resume();
				this.paused = false;
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00027766 File Offset: 0x00026766
		public void SpeakAsyncCancel(Prompt prompt)
		{
			Helpers.ThrowIfNull(prompt, "prompt");
			this.VoiceSynthesizer.Abort(prompt);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002777F File Offset: 0x0002677F
		public void SpeakAsyncCancelAll()
		{
			this.VoiceSynthesizer.Abort();
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002778C File Offset: 0x0002678C
		public void SetOutputToWaveFile(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.SetOutputToNull();
			this.SetOutputStream(new FileStream(path, 2, 2), null, true, true);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x000277B0 File Offset: 0x000267B0
		public void SetOutputToWaveFile(string path, SpeechAudioFormatInfo formatInfo)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Helpers.ThrowIfNull(formatInfo, "formatInfo");
			this.SetOutputToNull();
			this.SetOutputStream(new FileStream(path, 2, 2), formatInfo, true, true);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000277DF File Offset: 0x000267DF
		public void SetOutputToWaveStream(Stream audioDestination)
		{
			Helpers.ThrowIfNull(audioDestination, "audioDestination");
			this.SetOutputStream(audioDestination, null, true, false);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000277F6 File Offset: 0x000267F6
		public void SetOutputToAudioStream(Stream audioDestination, SpeechAudioFormatInfo formatInfo)
		{
			Helpers.ThrowIfNull(audioDestination, "audioDestination");
			Helpers.ThrowIfNull(formatInfo, "formatInfo");
			this.SetOutputStream(audioDestination, formatInfo, false, false);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00027818 File Offset: 0x00026818
		public void SetOutputToDefaultAudioDevice()
		{
			this.SetOutputStream(null, null, true, false);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00027824 File Offset: 0x00026824
		public void SetOutputToNull()
		{
			if (this._outputStream != Stream.Null)
			{
				this.VoiceSynthesizer.SetOutput(Stream.Null, null, true);
			}
			if (this._outputStream != null && this._closeStreamOnExit)
			{
				this._outputStream.Close();
			}
			this._outputStream = Stream.Null;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00027876 File Offset: 0x00026876
		public Prompt GetCurrentlySpokenPrompt()
		{
			return this.VoiceSynthesizer.Prompt;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00027883 File Offset: 0x00026883
		public ReadOnlyCollection<InstalledVoice> GetInstalledVoices()
		{
			return this.VoiceSynthesizer.GetInstalledVoices(null);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00027891 File Offset: 0x00026891
		public ReadOnlyCollection<InstalledVoice> GetInstalledVoices(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			return this.VoiceSynthesizer.GetInstalledVoices(culture);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000278CF File Offset: 0x000268CF
		public void AddLexicon(Uri uri, string mediaType)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this.VoiceSynthesizer.AddLexicon(uri, mediaType);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000278E9 File Offset: 0x000268E9
		public void RemoveLexicon(Uri uri)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this.VoiceSynthesizer.RemoveLexicon(uri);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060008CB RID: 2251 RVA: 0x00027902 File Offset: 0x00026902
		// (remove) Token: 0x060008CC RID: 2252 RVA: 0x0002792B File Offset: 0x0002692B
		public event EventHandler<SpeakStartedEventArgs> SpeakStarted
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakStarted = (EventHandler<SpeakStartedEventArgs>)Delegate.Combine(voiceSynthesizer._speakStarted, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakStarted = (EventHandler<SpeakStartedEventArgs>)Delegate.Remove(voiceSynthesizer._speakStarted, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060008CD RID: 2253 RVA: 0x00027954 File Offset: 0x00026954
		// (remove) Token: 0x060008CE RID: 2254 RVA: 0x0002797D File Offset: 0x0002697D
		public event EventHandler<SpeakCompletedEventArgs> SpeakCompleted
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakCompleted = (EventHandler<SpeakCompletedEventArgs>)Delegate.Combine(voiceSynthesizer._speakCompleted, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakCompleted = (EventHandler<SpeakCompletedEventArgs>)Delegate.Remove(voiceSynthesizer._speakCompleted, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060008CF RID: 2255 RVA: 0x000279A6 File Offset: 0x000269A6
		// (remove) Token: 0x060008D0 RID: 2256 RVA: 0x000279CB File Offset: 0x000269CB
		public event EventHandler<SpeakProgressEventArgs> SpeakProgress
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<SpeakProgressEventArgs>(TtsEventId.WordBoundary, ref this.VoiceSynthesizer._speakProgress, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<SpeakProgressEventArgs>(TtsEventId.WordBoundary, ref this.VoiceSynthesizer._speakProgress, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060008D1 RID: 2257 RVA: 0x000279F0 File Offset: 0x000269F0
		// (remove) Token: 0x060008D2 RID: 2258 RVA: 0x00027A15 File Offset: 0x00026A15
		public event EventHandler<BookmarkReachedEventArgs> BookmarkReached
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<BookmarkReachedEventArgs>(TtsEventId.Bookmark, ref this.VoiceSynthesizer._bookmarkReached, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<BookmarkReachedEventArgs>(TtsEventId.Bookmark, ref this.VoiceSynthesizer._bookmarkReached, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060008D3 RID: 2259 RVA: 0x00027A3A File Offset: 0x00026A3A
		// (remove) Token: 0x060008D4 RID: 2260 RVA: 0x00027A5F File Offset: 0x00026A5F
		public event EventHandler<VoiceChangeEventArgs> VoiceChange
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<VoiceChangeEventArgs>(TtsEventId.VoiceChange, ref this.VoiceSynthesizer._voiceChange, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<VoiceChangeEventArgs>(TtsEventId.VoiceChange, ref this.VoiceSynthesizer._voiceChange, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060008D5 RID: 2261 RVA: 0x00027A84 File Offset: 0x00026A84
		// (remove) Token: 0x060008D6 RID: 2262 RVA: 0x00027AA9 File Offset: 0x00026AA9
		public event EventHandler<PhonemeReachedEventArgs> PhonemeReached
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<PhonemeReachedEventArgs>(TtsEventId.Phoneme, ref this.VoiceSynthesizer._phonemeReached, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<PhonemeReachedEventArgs>(TtsEventId.Phoneme, ref this.VoiceSynthesizer._phonemeReached, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060008D7 RID: 2263 RVA: 0x00027ACE File Offset: 0x00026ACE
		// (remove) Token: 0x060008D8 RID: 2264 RVA: 0x00027AF3 File Offset: 0x00026AF3
		public event EventHandler<VisemeReachedEventArgs> VisemeReached
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<VisemeReachedEventArgs>(TtsEventId.Viseme, ref this.VoiceSynthesizer._visemeReached, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<VisemeReachedEventArgs>(TtsEventId.Viseme, ref this.VoiceSynthesizer._visemeReached, value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060008D9 RID: 2265 RVA: 0x00027B18 File Offset: 0x00026B18
		// (remove) Token: 0x060008DA RID: 2266 RVA: 0x00027B41 File Offset: 0x00026B41
		public event EventHandler<StateChangedEventArgs> StateChanged
		{
			[MethodImpl(32)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Combine(voiceSynthesizer._stateChanged, value);
			}
			[MethodImpl(32)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Remove(voiceSynthesizer._stateChanged, value);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00027B6A File Offset: 0x00026B6A
		public SynthesizerState State
		{
			get
			{
				return this.VoiceSynthesizer.State;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00027BAA File Offset: 0x00026BAA
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x00027B77 File Offset: 0x00026B77
		public int Rate
		{
			get
			{
				return this.VoiceSynthesizer.Rate;
			}
			set
			{
				if (value < -10 || value > 10)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.RateOutOfRange, new object[0]));
				}
				this.VoiceSynthesizer.Rate = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00027BE9 File Offset: 0x00026BE9
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x00027BB7 File Offset: 0x00026BB7
		public int Volume
		{
			get
			{
				return this.VoiceSynthesizer.Volume;
			}
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.ResourceUsageOutOfRange, new object[0]));
				}
				this.VoiceSynthesizer.Volume = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00027BF6 File Offset: 0x00026BF6
		public VoiceInfo Voice
		{
			get
			{
				return this.VoiceSynthesizer.CurrentVoice(true).VoiceInfo;
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00027C09 File Offset: 0x00026C09
		private void SetOutputStream(Stream stream, SpeechAudioFormatInfo formatInfo, bool headerInfo, bool closeStreamOnExit)
		{
			this.SetOutputToNull();
			this._outputStream = stream;
			this._closeStreamOnExit = closeStreamOnExit;
			this.VoiceSynthesizer.SetOutput(stream, formatInfo, headerInfo);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00027C30 File Offset: 0x00026C30
		private void Dispose(bool disposing)
		{
			if (!this._isDisposed && disposing && this._voiceSynthesis != null)
			{
				this._isDisposed = true;
				this.SpeakAsyncCancelAll();
				if (this._outputStream != null)
				{
					if (this._closeStreamOnExit)
					{
						this._outputStream.Close();
					}
					else
					{
						this._outputStream.Flush();
					}
					this._outputStream = null;
				}
			}
			if (this._voiceSynthesis != null)
			{
				this._voiceSynthesis.Dispose();
				this._voiceSynthesis = null;
			}
			this._isDisposed = true;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00027CB0 File Offset: 0x00026CB0
		private VoiceSynthesis VoiceSynthesizer
		{
			get
			{
				if (this._voiceSynthesis == null && this._isDisposed)
				{
					throw new ObjectDisposedException("SpeechSynthesizer");
				}
				if (this._voiceSynthesis == null)
				{
					WeakReference weakReference = new WeakReference(this);
					this._voiceSynthesis = new VoiceSynthesis(weakReference);
				}
				return this._voiceSynthesis;
			}
		}

		// Token: 0x04000669 RID: 1641
		private VoiceSynthesis _voiceSynthesis;

		// Token: 0x0400066A RID: 1642
		private bool _isDisposed;

		// Token: 0x0400066B RID: 1643
		private bool paused;

		// Token: 0x0400066C RID: 1644
		private Stream _outputStream;

		// Token: 0x0400066D RID: 1645
		private bool _closeStreamOnExit;
	}
}
