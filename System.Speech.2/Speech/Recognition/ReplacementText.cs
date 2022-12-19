using System;
using System.Runtime.InteropServices;

namespace System.Speech.Recognition
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ReplacementText
	{
		// Token: 0x0600019B RID: 411 RVA: 0x000073AD File Offset: 0x000055AD
		internal ReplacementText(DisplayAttributes displayAttributes, string text, int wordIndex, int countOfWords)
		{
			this._displayAttributes = displayAttributes;
			this._text = text;
			this._wordIndex = wordIndex;
			this._countOfWords = countOfWords;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000073D2 File Offset: 0x000055D2
		public DisplayAttributes DisplayAttributes
		{
			get
			{
				return this._displayAttributes;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000073DA File Offset: 0x000055DA
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000073E2 File Offset: 0x000055E2
		public int FirstWordIndex
		{
			get
			{
				return this._wordIndex;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000073EA File Offset: 0x000055EA
		public int CountOfWords
		{
			get
			{
				return this._countOfWords;
			}
		}

		// Token: 0x04000301 RID: 769
		private DisplayAttributes _displayAttributes;

		// Token: 0x04000302 RID: 770
		private string _text;

		// Token: 0x04000303 RID: 771
		private int _wordIndex;

		// Token: 0x04000304 RID: 772
		private int _countOfWords;
	}
}
