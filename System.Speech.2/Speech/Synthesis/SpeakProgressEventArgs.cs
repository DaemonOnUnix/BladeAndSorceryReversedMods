using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001B RID: 27
	public class SpeakProgressEventArgs : PromptEventArgs
	{
		// Token: 0x0600008F RID: 143 RVA: 0x000044D9 File Offset: 0x000026D9
		internal SpeakProgressEventArgs(Prompt prompt, TimeSpan audioPosition, int iWordPos, int cWordLen)
			: base(prompt)
		{
			this._audioPosition = audioPosition;
			this._iWordPos = iWordPos;
			this._cWordLen = cWordLen;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000044F8 File Offset: 0x000026F8
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004500 File Offset: 0x00002700
		public int CharacterPosition
		{
			get
			{
				return this._iWordPos;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004508 File Offset: 0x00002708
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004510 File Offset: 0x00002710
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004519 File Offset: 0x00002719
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004521 File Offset: 0x00002721
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

		// Token: 0x040001F1 RID: 497
		private TimeSpan _audioPosition;

		// Token: 0x040001F2 RID: 498
		private int _iWordPos;

		// Token: 0x040001F3 RID: 499
		private int _cWordLen;

		// Token: 0x040001F4 RID: 500
		private string _word;
	}
}
