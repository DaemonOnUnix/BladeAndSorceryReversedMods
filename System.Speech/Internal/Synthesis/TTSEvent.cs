using System;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x02000103 RID: 259
	internal class TTSEvent
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x0001BDD9 File Offset: 0x0001ADD9
		internal TTSEvent(TtsEventId id, Prompt prompt, Exception exception, VoiceInfo voice)
		{
			this._evtId = id;
			this._prompt = prompt;
			this._exception = exception;
			this._voice = voice;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001BDFE File Offset: 0x0001ADFE
		internal TTSEvent(TtsEventId id, Prompt prompt, Exception exception, VoiceInfo voice, TimeSpan audioPosition, long streamPosition, string bookmark, uint wParam, IntPtr lParam)
			: this(id, prompt, exception, voice)
		{
			this._audioPosition = audioPosition;
			this._bookmark = bookmark;
			this._wParam = wParam;
			this._lParam = lParam;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001BE2B File Offset: 0x0001AE2B
		private TTSEvent()
		{
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001BE34 File Offset: 0x0001AE34
		internal static TTSEvent CreatePhonemeEvent(string phoneme, string nextPhoneme, TimeSpan duration, SynthesizerEmphasis emphasis, Prompt prompt, TimeSpan audioPosition)
		{
			return new TTSEvent
			{
				_evtId = TtsEventId.Phoneme,
				_audioPosition = audioPosition,
				_prompt = prompt,
				_phoneme = phoneme,
				_nextPhoneme = nextPhoneme,
				_phonemeDuration = duration,
				_phonemeEmphasis = emphasis
			};
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001BE7B File Offset: 0x0001AE7B
		internal TtsEventId Id
		{
			get
			{
				return this._evtId;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001BE83 File Offset: 0x0001AE83
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001BE8B File Offset: 0x0001AE8B
		internal Prompt Prompt
		{
			get
			{
				return this._prompt;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001BE93 File Offset: 0x0001AE93
		internal VoiceInfo Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001BE9B File Offset: 0x0001AE9B
		internal TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001BEA3 File Offset: 0x0001AEA3
		internal string Bookmark
		{
			get
			{
				return this._bookmark;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001BEAB File Offset: 0x0001AEAB
		internal IntPtr LParam
		{
			get
			{
				return this._lParam;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001BEB3 File Offset: 0x0001AEB3
		internal uint WParam
		{
			get
			{
				return this._wParam;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001BEBB File Offset: 0x0001AEBB
		internal SynthesizerEmphasis PhonemeEmphasis
		{
			get
			{
				return this._phonemeEmphasis;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001BEC3 File Offset: 0x0001AEC3
		internal string Phoneme
		{
			get
			{
				return this._phoneme;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0001BECB File Offset: 0x0001AECB
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0001BED3 File Offset: 0x0001AED3
		internal string NextPhoneme
		{
			get
			{
				return this._nextPhoneme;
			}
			set
			{
				this._nextPhoneme = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001BEDC File Offset: 0x0001AEDC
		internal TimeSpan PhonemeDuration
		{
			get
			{
				return this._phonemeDuration;
			}
		}

		// Token: 0x040004C9 RID: 1225
		private TtsEventId _evtId;

		// Token: 0x040004CA RID: 1226
		private Exception _exception;

		// Token: 0x040004CB RID: 1227
		private VoiceInfo _voice;

		// Token: 0x040004CC RID: 1228
		private TimeSpan _audioPosition;

		// Token: 0x040004CD RID: 1229
		private string _bookmark;

		// Token: 0x040004CE RID: 1230
		private uint _wParam;

		// Token: 0x040004CF RID: 1231
		private IntPtr _lParam;

		// Token: 0x040004D0 RID: 1232
		private Prompt _prompt;

		// Token: 0x040004D1 RID: 1233
		private string _phoneme;

		// Token: 0x040004D2 RID: 1234
		private string _nextPhoneme;

		// Token: 0x040004D3 RID: 1235
		private TimeSpan _phonemeDuration;

		// Token: 0x040004D4 RID: 1236
		private SynthesizerEmphasis _phonemeEmphasis;
	}
}
