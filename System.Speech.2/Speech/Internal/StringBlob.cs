using System;
using System.Collections.Generic;

namespace System.Speech.Internal
{
	// Token: 0x02000099 RID: 153
	internal class StringBlob
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x00014847 File Offset: 0x00012A47
		internal StringBlob()
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00014878 File Offset: 0x00012A78
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
						Dictionary<string, int> strings = this._strings;
						string text2 = text;
						int num4 = this._cWords + 1;
						this._cWords = num4;
						strings.Add(text2, num4);
						this._totalStringSizes += text.Length + 1;
						num3 = i + 1;
					}
					i++;
				}
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00014954 File Offset: 0x00012B54
		internal int Add(string psz, out int idWord)
		{
			int num = 0;
			idWord = 0;
			if (!string.IsNullOrEmpty(psz))
			{
				if (!this._strings.TryGetValue(psz, out idWord))
				{
					int num2 = this._cWords + 1;
					this._cWords = num2;
					idWord = num2;
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

		// Token: 0x06000513 RID: 1299 RVA: 0x000149E4 File Offset: 0x00012BE4
		internal int Find(string psz)
		{
			if (string.IsNullOrEmpty(psz) || this._cWords == 0)
			{
				return 0;
			}
			int num;
			if (!this._strings.TryGetValue(psz, out num))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x1700012F RID: 303
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

		// Token: 0x06000515 RID: 1301 RVA: 0x00014A3C File Offset: 0x00012C3C
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

		// Token: 0x06000516 RID: 1302 RVA: 0x00014AC4 File Offset: 0x00012CC4
		internal int StringSize()
		{
			if (this._cWords <= 0)
			{
				return 0;
			}
			return this._totalStringSizes;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00014AD7 File Offset: 0x00012CD7
		internal int SerializeSize()
		{
			return ((this.StringSize() * 2 + 3) & -4) / 2;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00014AE8 File Offset: 0x00012CE8
		internal char[] SerializeData()
		{
			int num = this.SerializeSize();
			char[] array = new char[num];
			int num2 = 1;
			foreach (string text in this._refStrings)
			{
				for (int i = 0; i < text.Length; i++)
				{
					array[num2++] = text[i];
				}
				array[num2++] = '\0';
			}
			if (this.StringSize() % 2 == 1)
			{
				array[num2++] = '쳌';
			}
			return array;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00014B8C File Offset: 0x00012D8C
		internal int OffsetFromId(int index)
		{
			if (index > 0)
			{
				return this._offsetStrings[index - 1];
			}
			return 0;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00014BA2 File Offset: 0x00012DA2
		internal int Count
		{
			get
			{
				return this._cWords;
			}
		}

		// Token: 0x04000445 RID: 1093
		private Dictionary<string, int> _strings = new Dictionary<string, int>();

		// Token: 0x04000446 RID: 1094
		private List<string> _refStrings = new List<string>();

		// Token: 0x04000447 RID: 1095
		private List<int> _offsetStrings = new List<int>();

		// Token: 0x04000448 RID: 1096
		private int _cWords;

		// Token: 0x04000449 RID: 1097
		private int _totalStringSizes = 1;

		// Token: 0x0400044A RID: 1098
		private const int _sizeOfChar = 2;
	}
}
