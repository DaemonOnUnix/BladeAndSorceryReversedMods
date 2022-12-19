using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000020 RID: 32
	public class PhonemeReachedEventArgs : PromptEventArgs
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00004969 File Offset: 0x00002B69
		internal PhonemeReachedEventArgs(Prompt prompt, string currentPhoneme, TimeSpan audioPosition, TimeSpan duration, SynthesizerEmphasis emphasis, string nextPhoneme)
			: base(prompt)
		{
			this._currentPhoneme = currentPhoneme;
			this._audioPosition = audioPosition;
			this._duration = duration;
			this._emphasis = emphasis;
			this._nextPhoneme = nextPhoneme;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004998 File Offset: 0x00002B98
		public string Phoneme
		{
			get
			{
				return this._currentPhoneme;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000049A0 File Offset: 0x00002BA0
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000049A8 File Offset: 0x00002BA8
		public TimeSpan Duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000049B0 File Offset: 0x00002BB0
		public SynthesizerEmphasis Emphasis
		{
			get
			{
				return this._emphasis;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000049B8 File Offset: 0x00002BB8
		public string NextPhoneme
		{
			get
			{
				return this._nextPhoneme;
			}
		}

		// Token: 0x04000209 RID: 521
		private string _currentPhoneme;

		// Token: 0x0400020A RID: 522
		private TimeSpan _audioPosition;

		// Token: 0x0400020B RID: 523
		private TimeSpan _duration;

		// Token: 0x0400020C RID: 524
		private SynthesizerEmphasis _emphasis;

		// Token: 0x0400020D RID: 525
		private string _nextPhoneme;
	}
}
