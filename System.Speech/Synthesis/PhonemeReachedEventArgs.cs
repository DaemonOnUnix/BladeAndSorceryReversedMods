using System;

namespace System.Speech.Synthesis
{
	// Token: 0x020001B2 RID: 434
	public class PhonemeReachedEventArgs : PromptEventArgs
	{
		// Token: 0x06000BDF RID: 3039 RVA: 0x00031C33 File Offset: 0x00030C33
		internal PhonemeReachedEventArgs(Prompt prompt, string currentPhoneme, TimeSpan audioPosition, TimeSpan duration, SynthesizerEmphasis emphasis, string nextPhoneme)
			: base(prompt)
		{
			this._currentPhoneme = currentPhoneme;
			this._audioPosition = audioPosition;
			this._duration = duration;
			this._emphasis = emphasis;
			this._nextPhoneme = nextPhoneme;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00031C62 File Offset: 0x00030C62
		public string Phoneme
		{
			get
			{
				return this._currentPhoneme;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00031C6A File Offset: 0x00030C6A
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00031C72 File Offset: 0x00030C72
		public TimeSpan Duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00031C7A File Offset: 0x00030C7A
		public SynthesizerEmphasis Emphasis
		{
			get
			{
				return this._emphasis;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00031C82 File Offset: 0x00030C82
		public string NextPhoneme
		{
			get
			{
				return this._nextPhoneme;
			}
		}

		// Token: 0x0400099A RID: 2458
		private string _currentPhoneme;

		// Token: 0x0400099B RID: 2459
		private TimeSpan _audioPosition;

		// Token: 0x0400099C RID: 2460
		private TimeSpan _duration;

		// Token: 0x0400099D RID: 2461
		private SynthesizerEmphasis _emphasis;

		// Token: 0x0400099E RID: 2462
		private string _nextPhoneme;
	}
}
