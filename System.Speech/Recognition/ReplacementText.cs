using System;
using System.Runtime.InteropServices;

namespace System.Speech.Recognition
{
	// Token: 0x02000132 RID: 306
	[Serializable]
	[StructLayout(0)]
	public class ReplacementText
	{
		// Token: 0x06000821 RID: 2081 RVA: 0x000251AD File Offset: 0x000241AD
		internal ReplacementText(DisplayAttributes displayAttributes, string text, int wordIndex, int countOfWords)
		{
			this._displayAttributes = displayAttributes;
			this._text = text;
			this._wordIndex = wordIndex;
			this._countOfWords = countOfWords;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x000251D2 File Offset: 0x000241D2
		public DisplayAttributes DisplayAttributes
		{
			get
			{
				return this._displayAttributes;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x000251DA File Offset: 0x000241DA
		public string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x000251E2 File Offset: 0x000241E2
		public int FirstWordIndex
		{
			get
			{
				return this._wordIndex;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000251EA File Offset: 0x000241EA
		public int CountOfWords
		{
			get
			{
				return this._countOfWords;
			}
		}

		// Token: 0x040005C9 RID: 1481
		private DisplayAttributes _displayAttributes;

		// Token: 0x040005CA RID: 1482
		private string _text;

		// Token: 0x040005CB RID: 1483
		private int _wordIndex;

		// Token: 0x040005CC RID: 1484
		private int _countOfWords;
	}
}
