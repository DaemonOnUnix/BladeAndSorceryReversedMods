using System;
using System.Collections.Generic;

namespace System.Speech.Internal
{
	// Token: 0x0200001F RID: 31
	internal class StringBlob
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00006723 File Offset: 0x00005723
		internal StringBlob()
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006754 File Offset: 0x00005754
		internal StringBlob(char[] pszStringArray)
		{
			int num = pszStringArray.Length;
			if (num > 0)
			{
				if (pszStringArray[0] != '\0')
				{
					throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]));
				}
				int i = 1;
				int num2 = num;
				int num3 = 1;
				while (i < num2)
				{
					if (pszStringArray[i] == '\0')
					{
						string text = new string(pszStringArray, num3, i - num3);
						this._refStrings.Add(text);
						this._offsetStrings.Add(this._totalStringSizes);
						this._strings.Add(text, ++this._cWords);
						this._totalStringSizes += text.Length + 1;
						num3 = i + 1;
					}
					i++;
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006830 File Offset: 0x00005830
		internal int Add(string psz, out int idWord)
		{
			int num = 0;
			idWord = 0;
			if (!string.IsNullOrEmpty(psz))
			{
				if (!this._strings.TryGetValue(psz, ref idWord))
				{
					idWord = ++this._cWords;
					num = this._totalStringSizes;
					this._refStrings.Add(psz);
					this._offsetStrings.Add(num);
					this._strings.Add(psz, this._cWords);
					this._totalStringSizes += psz.Length + 1;
				}
				else
				{
					num = this.OffsetFromId(idWord);
				}
			}
			return num;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000068C0 File Offset: 0x000058C0
		internal int Find(string psz)
		{
			if (string.IsNullOrEmpty(psz) || this._cWords == 0)
			{
				return 0;
			}
			int num;
			if (!this._strings.TryGetValue(psz, ref num))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x1700001F RID: 31
		internal string this[int index]
		{
			get
			{
				if (index < 1 || index > this._cWords)
				{
					throw new InvalidOperationException();
				}
				return this._refStrings[index - 1];
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006918 File Offset: 0x00005918
		internal string FromOffset(int offset)
		{
			int num = 1;
			int num2 = 1;
			if (offset == 1 && this._cWords >= 1)
			{
				return this[num2];
			}
			foreach (string text in this._refStrings)
			{
				num2++;
				num += text.Length + 1;
				if (offset == num)
				{
					return this[num2];
				}
			}
			return null;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000069A0 File Offset: 0x000059A0
		internal int StringSize()
		{
			if (this._cWords <= 0)
			{
				return 0;
			}
			return this._totalStringSizes;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000069B3 File Offset: 0x000059B3
		internal int SerializeSize()
		{
			return ((this.StringSize() * 2 + 3) & -4) / 2;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000069C4 File Offset: 0x000059C4
		internal char[] SerializeData()
		{
			int num = this.SerializeSize();
			char[] array = new char[num];
			int num2 = 1;
			foreach (string text in this._refStrings)
			{
				for (int i = 0; i < text.Length; i++)
				{
					array[num2++] = text.get_Chars(i);
				}
				array[num2++] = '\0';
			}
			if (this.StringSize() % 2 == 1)
			{
				array[num2++] = '쳌';
			}
			return array;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006A68 File Offset: 0x00005A68
		internal int OffsetFromId(int index)
		{
			if (index > 0)
			{
				return this._offsetStrings[index - 1];
			}
			return 0;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00006A7E File Offset: 0x00005A7E
		internal int Count
		{
			get
			{
				return this._cWords;
			}
		}

		// Token: 0x040000A6 RID: 166
		private const int _sizeOfChar = 2;

		// Token: 0x040000A7 RID: 167
		private Dictionary<string, int> _strings = new Dictionary<string, int>();

		// Token: 0x040000A8 RID: 168
		private List<string> _refStrings = new List<string>();

		// Token: 0x040000A9 RID: 169
		private List<int> _offsetStrings = new List<int>();

		// Token: 0x040000AA RID: 170
		private int _cWords;

		// Token: 0x040000AB RID: 171
		private int _totalStringSizes = 1;
	}
}
