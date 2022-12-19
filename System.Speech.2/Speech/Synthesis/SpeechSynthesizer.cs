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
	// Token: 0x02000017 RID: 23
	public sealed class SpeechSynthesizer : IDisposable
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003C60 File Offset: 0x00001E60
		~SpeechSynthesizer()
		{
			this.Dispose(false);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003C90 File Offset: 0x00001E90
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003CA0 File Offset: 0x00001EA0
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

		// Token: 0x0600005A RID: 90 RVA: 0x00003D06 File Offset: 0x00001F06
		public void SelectVoiceByHints(VoiceGender gender)
		{
			this.SelectVoiceByHints(gender, VoiceAge.NotSet, 1, CultureInfo.CurrentUICulture);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003D16 File Offset: 0x00001F16
		public void SelectVoiceByHints(VoiceGender gender, VoiceAge age)
		{
			this.SelectVoiceByHints(gender, age, 1, CultureInfo.CurrentUICulture);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003D26 File Offset: 0x00001F26
		public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, int voiceAlternate)
		{
			this.SelectVoiceByHints(gender, age, voiceAlternate, CultureInfo.CurrentUICulture);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003D38 File Offset: 0x00001F38
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

		// Token: 0x0600005E RID: 94 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public Prompt SpeakAsync(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Prompt prompt = new Prompt(textToSpeak, SynthesisTextFormat.Text);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003E20 File Offset: 0x00002020
		public void SpeakAsync(Prompt prompt)
		{
			Helpers.ThrowIfNull(prompt, "prompt");
			prompt.Synthesizer = this;
			this.VoiceSynthesizer.SpeakAsync(prompt);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003E40 File Offset: 0x00002040
		public Prompt SpeakSsmlAsync(string textToSpeak)
		{
			Helpers.ThrowIfNull(textToSpeak, "textToSpeak");
			Prompt prompt = new Prompt(textToSpeak, SynthesisTextFormat.Ssml);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003E68 File Offset: 0x00002068
		public Prompt SpeakAsync(PromptBuilder promptBuilder)
		{
			Helpers.ThrowIfNull(promptBuilder, "promptBuilder");
			Prompt prompt = new Prompt(promptBuilder);
			this.SpeakAsync(prompt);
			return prompt;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003E8F File Offset: 0x0000208F
		public void Speak(string textToSpeak)
		{
			this.Speak(new Prompt(textToSpeak, SynthesisTextFormat.Text));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003EA0 File Offset: 0x000020A0
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

		// Token: 0x06000064 RID: 100 RVA: 0x00003EF1 File Offset: 0x000020F1
		public void Speak(PromptBuilder promptBuilder)
		{
			this.Speak(new Prompt(promptBuilder));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003EFF File Offset: 0x000020FF
		public void SpeakSsml(string textToSpeak)
		{
			this.Speak(new Prompt(textToSpeak, SynthesisTextFormat.Ssml));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003F0E File Offset: 0x0000210E
		public void Pause()
		{
			if (!this.paused)
			{
				this.VoiceSynthesizer.Pause();
				this.paused = true;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F2A File Offset: 0x0000212A
		public void Resume()
		{
			if (this.paused)
			{
				this.VoiceSynthesizer.Resume();
				this.paused = false;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003F46 File Offset: 0x00002146
		public void SpeakAsyncCancel(Prompt prompt)
		{
			Helpers.ThrowIfNull(prompt, "prompt");
			this.VoiceSynthesizer.Abort(prompt);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003F5F File Offset: 0x0000215F
		public void SpeakAsyncCancelAll()
		{
			this.VoiceSynthesizer.Abort();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003F6C File Offset: 0x0000216C
		public void SetOutputToWaveFile(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			this.SetOutputToNull();
			this.SetOutputStream(new FileStream(path, FileMode.Create, FileAccess.Write), null, true, true);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003F90 File Offset: 0x00002190
		public void SetOutputToWaveFile(string path, SpeechAudioFormatInfo formatInfo)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Helpers.ThrowIfNull(formatInfo, "formatInfo");
			this.SetOutputToNull();
			this.SetOutputStream(new FileStream(path, FileMode.Create, FileAccess.Write), formatInfo, true, true);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003FBF File Offset: 0x000021BF
		public void SetOutputToWaveStream(Stream audioDestination)
		{
			Helpers.ThrowIfNull(audioDestination, "audioDestination");
			this.SetOutputStream(audioDestination, null, true, false);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003FD6 File Offset: 0x000021D6
		public void SetOutputToAudioStream(Stream audioDestination, SpeechAudioFormatInfo formatInfo)
		{
			Helpers.ThrowIfNull(audioDestination, "audioDestination");
			Helpers.ThrowIfNull(formatInfo, "formatInfo");
			this.SetOutputStream(audioDestination, formatInfo, false, false);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003FF8 File Offset: 0x000021F8
		public void SetOutputToDefaultAudioDevice()
		{
			this.SetOutputStream(null, null, true, false);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004004 File Offset: 0x00002204
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

		// Token: 0x06000070 RID: 112 RVA: 0x00004056 File Offset: 0x00002256
		public Prompt GetCurrentlySpokenPrompt()
		{
			return this.VoiceSynthesizer.Prompt;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004063 File Offset: 0x00002263
		public ReadOnlyCollection<InstalledVoice> GetInstalledVoices()
		{
			return this.VoiceSynthesizer.GetInstalledVoices(null);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004071 File Offset: 0x00002271
		public ReadOnlyCollection<InstalledVoice> GetInstalledVoices(CultureInfo culture)
		{
			Helpers.ThrowIfNull(culture, "culture");
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				throw new ArgumentException(SR.Get(SRID.InvariantCultureInfo, new object[0]), "culture");
			}
			return this.VoiceSynthesizer.GetInstalledVoices(culture);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000040AF File Offset: 0x000022AF
		public void AddLexicon(Uri uri, string mediaType)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this.VoiceSynthesizer.AddLexicon(uri, mediaType);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000040C9 File Offset: 0x000022C9
		public void RemoveLexicon(Uri uri)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this.VoiceSynthesizer.RemoveLexicon(uri);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000075 RID: 117 RVA: 0x000040E2 File Offset: 0x000022E2
		// (remove) Token: 0x06000076 RID: 118 RVA: 0x0000410B File Offset: 0x0000230B
		public event EventHandler<SpeakStartedEventArgs> SpeakStarted
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakStarted = (EventHandler<SpeakStartedEventArgs>)Delegate.Combine(voiceSynthesizer._speakStarted, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakStarted = (EventHandler<SpeakStartedEventArgs>)Delegate.Remove(voiceSynthesizer._speakStarted, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000077 RID: 119 RVA: 0x00004134 File Offset: 0x00002334
		// (remove) Token: 0x06000078 RID: 120 RVA: 0x0000415D File Offset: 0x0000235D
		public event EventHandler<SpeakCompletedEventArgs> SpeakCompleted
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakCompleted = (EventHandler<SpeakCompletedEventArgs>)Delegate.Combine(voiceSynthesizer._speakCompleted, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._speakCompleted = (EventHandler<SpeakCompletedEventArgs>)Delegate.Remove(voiceSynthesizer._speakCompleted, value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000079 RID: 121 RVA: 0x00004186 File Offset: 0x00002386
		// (remove) Token: 0x0600007A RID: 122 RVA: 0x000041AB File Offset: 0x000023AB
		public event EventHandler<SpeakProgressEventArgs> SpeakProgress
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<SpeakProgressEventArgs>(TtsEventId.WordBoundary, ref this.VoiceSynthesizer._speakProgress, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<SpeakProgressEventArgs>(TtsEventId.WordBoundary, ref this.VoiceSynthesizer._speakProgress, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600007B RID: 123 RVA: 0x000041D0 File Offset: 0x000023D0
		// (remove) Token: 0x0600007C RID: 124 RVA: 0x000041F5 File Offset: 0x000023F5
		public event EventHandler<BookmarkReachedEventArgs> BookmarkReached
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<BookmarkReachedEventArgs>(TtsEventId.Bookmark, ref this.VoiceSynthesizer._bookmarkReached, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<BookmarkReachedEventArgs>(TtsEventId.Bookmark, ref this.VoiceSynthesizer._bookmarkReached, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600007D RID: 125 RVA: 0x0000421A File Offset: 0x0000241A
		// (remove) Token: 0x0600007E RID: 126 RVA: 0x0000423F File Offset: 0x0000243F
		public event EventHandler<VoiceChangeEventArgs> VoiceChange
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<VoiceChangeEventArgs>(TtsEventId.VoiceChange, ref this.VoiceSynthesizer._voiceChange, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<VoiceChangeEventArgs>(TtsEventId.VoiceChange, ref this.VoiceSynthesizer._voiceChange, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600007F RID: 127 RVA: 0x00004264 File Offset: 0x00002464
		// (remove) Token: 0x06000080 RID: 128 RVA: 0x00004289 File Offset: 0x00002489
		public event EventHandler<PhonemeReachedEventArgs> PhonemeReached
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<PhonemeReachedEventArgs>(TtsEventId.Phoneme, ref this.VoiceSynthesizer._phonemeReached, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<PhonemeReachedEventArgs>(TtsEventId.Phoneme, ref this.VoiceSynthesizer._phonemeReached, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000081 RID: 129 RVA: 0x000042AE File Offset: 0x000024AE
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x000042D3 File Offset: 0x000024D3
		public event EventHandler<VisemeReachedEventArgs> VisemeReached
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.AddEvent<VisemeReachedEventArgs>(TtsEventId.Viseme, ref this.VoiceSynthesizer._visemeReached, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				this.VoiceSynthesizer.RemoveEvent<VisemeReachedEventArgs>(TtsEventId.Viseme, ref this.VoiceSynthesizer._visemeReached, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000083 RID: 131 RVA: 0x000042F8 File Offset: 0x000024F8
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x00004321 File Offset: 0x00002521
		public event EventHandler<StateChangedEventArgs> StateChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Combine(voiceSynthesizer._stateChanged, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Helpers.ThrowIfNull(value, "value");
				VoiceSynthesis voiceSynthesizer = this.VoiceSynthesizer;
				voiceSynthesizer._stateChanged = (EventHandler<StateChangedEventArgs>)Delegate.Remove(voiceSynthesizer._stateChanged, value);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000434A File Offset: 0x0000254A
		public SynthesizerState State
		{
			get
			{
				return this.VoiceSynthesizer.State;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000438A File Offset: 0x0000258A
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00004357 File Offset: 0x00002557
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000043C9 File Offset: 0x000025C9
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00004397 File Offset: 0x00002597
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000043D6 File Offset: 0x000025D6
		public VoiceInfo Voice
		{
			get
			{
				return this.VoiceSynthesizer.CurrentVoice(true).VoiceInfo;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000043E9 File Offset: 0x000025E9
		private void SetOutputStream(Stream stream, SpeechAudioFormatInfo formatInfo, bool headerInfo, bool closeStreamOnExit)
		{
			this.SetOutputToNull();
			this._outputStream = stream;
			this._closeStreamOnExit = closeStreamOnExit;
			this.VoiceSynthesizer.SetOutput(stream, formatInfo, headerInfo);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004410 File Offset: 0x00002610
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004490 File Offset: 0x00002690
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

		// Token: 0x040001E5 RID: 485
		private VoiceSynthesis _voiceSynthesis;

		// Token: 0x040001E6 RID: 486
		private bool _isDisposed;

		// Token: 0x040001E7 RID: 487
		private bool paused;

		// Token: 0x040001E8 RID: 488
		private Stream _outputStream;

		// Token: 0x040001E9 RID: 489
		private bool _closeStreamOnExit;
	}
}
