using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000153 RID: 339
	public class SpeakProgressEventArgs : PromptEventArgs
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x00027D02 File Offset: 0x00026D02
		internal SpeakProgressEventArgs(Prompt prompt, TimeSpan audioPosition, int iWordPos, int cWordLen)
			: base(prompt)
		{
			this._audioPosition = audioPosition;
			this._iWordPos = iWordPos;
			this._cWordLen = cWordLen;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00027D21 File Offset: 0x00026D21
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00027D29 File Offset: 0x00026D29
		public int CharacterPosition
		{
			get
			{
				return this._iWordPos;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00027D31 File Offset: 0x00026D31
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00027D39 File Offset: 0x00026D39
		public int CharacterCount
		{
			get
			{
				return this._cWordLen;
			}
			internal set
			{
				this._cWordLen = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x00027D42 File Offset: 0x00026D42
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00027D4A File Offset: 0x00026D4A
		public string Text
		{
			get
			{
				return this._word;
			}
			internal set
			{
				this._word = value;
			}
		}

		// Token: 0x04000675 RID: 1653
		private TimeSpan _audioPosition;

		// Token: 0x04000676 RID: 1654
		private int _iWordPos;

		// Token: 0x04000677 RID: 1655
		private int _cWordLen;

		// Token: 0x04000678 RID: 1656
		private string _word;
	}
}
