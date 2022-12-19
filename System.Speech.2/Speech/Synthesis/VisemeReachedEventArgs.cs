using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001F RID: 31
	public class VisemeReachedEventArgs : PromptEventArgs
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00004912 File Offset: 0x00002B12
		internal VisemeReachedEventArgs(Prompt speakPrompt, int currentViseme, TimeSpan audioPosition, TimeSpan duration, SynthesizerEmphasis emphasis, int nextViseme)
			: base(speakPrompt)
		{
			this._currentViseme = currentViseme;
			this._audioPosition = audioPosition;
			this._duration = duration;
			this._emphasis = emphasis;
			this._nextViseme = nextViseme;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004941 File Offset: 0x00002B41
		public int Viseme
		{
			get
			{
				return this._currentViseme;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004949 File Offset: 0x00002B49
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004951 File Offset: 0x00002B51
		public TimeSpan Duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004959 File Offset: 0x00002B59
		public SynthesizerEmphasis Emphasis
		{
			get
			{
				return this._emphasis;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004961 File Offset: 0x00002B61
		public int NextViseme
		{
			get
			{
				return this._nextViseme;
			}
		}

		// Token: 0x04000204 RID: 516
		private int _currentViseme;

		// Token: 0x04000205 RID: 517
		private TimeSpan _audioPosition;

		// Token: 0x04000206 RID: 518
		private TimeSpan _duration;

		// Token: 0x04000207 RID: 519
		private SynthesizerEmphasis _emphasis;

		// Token: 0x04000208 RID: 520
		private int _nextViseme;
	}
}
