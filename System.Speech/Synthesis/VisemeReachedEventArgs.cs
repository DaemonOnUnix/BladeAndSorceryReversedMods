using System;

namespace System.Speech.Synthesis
{
	// Token: 0x020001B1 RID: 433
	public class VisemeReachedEventArgs : PromptEventArgs
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x00031BDC File Offset: 0x00030BDC
		internal VisemeReachedEventArgs(Prompt speakPrompt, int currentViseme, TimeSpan audioPosition, TimeSpan duration, SynthesizerEmphasis emphasis, int nextViseme)
			: base(speakPrompt)
		{
			this._currentViseme = currentViseme;
			this._audioPosition = audioPosition;
			this._duration = duration;
			this._emphasis = emphasis;
			this._nextViseme = nextViseme;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00031C0B File Offset: 0x00030C0B
		public int Viseme
		{
			get
			{
				return this._currentViseme;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00031C13 File Offset: 0x00030C13
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00031C1B File Offset: 0x00030C1B
		public TimeSpan Duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00031C23 File Offset: 0x00030C23
		public SynthesizerEmphasis Emphasis
		{
			get
			{
				return this._emphasis;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00031C2B File Offset: 0x00030C2B
		public int NextViseme
		{
			get
			{
				return this._nextViseme;
			}
		}

		// Token: 0x04000995 RID: 2453
		private int _currentViseme;

		// Token: 0x04000996 RID: 2454
		private TimeSpan _audioPosition;

		// Token: 0x04000997 RID: 2455
		private TimeSpan _duration;

		// Token: 0x04000998 RID: 2456
		private SynthesizerEmphasis _emphasis;

		// Token: 0x04000999 RID: 2457
		private int _nextViseme;
	}
}
