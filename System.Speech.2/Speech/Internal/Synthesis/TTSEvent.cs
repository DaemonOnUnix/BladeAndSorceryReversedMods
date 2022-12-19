using System;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000CB RID: 203
	internal class TTSEvent
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x0001CC3A File Offset: 0x0001AE3A
		internal TTSEvent(TtsEventId id, Prompt prompt, Exception exception, VoiceInfo voice)
		{
			this._evtId = id;
			this._prompt = prompt;
			this._exception = exception;
			this._voice = voice;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001CC5F File Offset: 0x0001AE5F
		internal TTSEvent(TtsEventId id, Prompt prompt, Exception exception, VoiceInfo voice, TimeSpan audioPosition, long streamPosition, string bookmark, uint wParam, IntPtr lParam)
			: this(id, prompt, exception, voice)
		{
			this._audioPosition = audioPosition;
			this._bookmark = bookmark;
			this._wParam = wParam;
			this._lParam = lParam;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00003BF5 File Offset: 0x00001DF5
		private TTSEvent()
		{
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001CC8C File Offset: 0x0001AE8C
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001CCD3 File Offset: 0x0001AED3
		internal TtsEventId Id
		{
			get
			{
				return this._evtId;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001CCDB File Offset: 0x0001AEDB
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001CCE3 File Offset: 0x0001AEE3
		internal Prompt Prompt
		{
			get
			{
				return this._prompt;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001CCEB File Offset: 0x0001AEEB
		internal VoiceInfo Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001CCF3 File Offset: 0x0001AEF3
		internal TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001CCFB File Offset: 0x0001AEFB
		internal string Bookmark
		{
			get
			{
				return this._bookmark;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001CD03 File Offset: 0x0001AF03
		internal IntPtr LParam
		{
			get
			{
				return this._lParam;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001CD0B File Offset: 0x0001AF0B
		internal uint WParam
		{
			get
			{
				return this._wParam;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001CD13 File Offset: 0x0001AF13
		internal SynthesizerEmphasis PhonemeEmphasis
		{
			get
			{
				return this._phonemeEmphasis;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001CD1B File Offset: 0x0001AF1B
		internal string Phoneme
		{
			get
			{
				return this._phoneme;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001CD23 File Offset: 0x0001AF23
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0001CD2B File Offset: 0x0001AF2B
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

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0001CD34 File Offset: 0x0001AF34
		internal TimeSpan PhonemeDuration
		{
			get
			{
				return this._phonemeDuration;
			}
		}

		// Token: 0x04000540 RID: 1344
		private TtsEventId _evtId;

		// Token: 0x04000541 RID: 1345
		private Exception _exception;

		// Token: 0x04000542 RID: 1346
		private VoiceInfo _voice;

		// Token: 0x04000543 RID: 1347
		private TimeSpan _audioPosition;

		// Token: 0x04000544 RID: 1348
		private string _bookmark;

		// Token: 0x04000545 RID: 1349
		private uint _wParam;

		// Token: 0x04000546 RID: 1350
		private IntPtr _lParam;

		// Token: 0x04000547 RID: 1351
		private Prompt _prompt;

		// Token: 0x04000548 RID: 1352
		private string _phoneme;

		// Token: 0x04000549 RID: 1353
		private string _nextPhoneme;

		// Token: 0x0400054A RID: 1354
		private TimeSpan _phonemeDuration;

		// Token: 0x0400054B RID: 1355
		private SynthesizerEmphasis _phonemeEmphasis;
	}
}
