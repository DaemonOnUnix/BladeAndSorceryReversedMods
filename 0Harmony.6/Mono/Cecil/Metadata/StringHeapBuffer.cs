using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A3 RID: 419
	internal class StringHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0002EFB9 File Offset: 0x0002D1B9
		public sealed override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002EFC7 File Offset: 0x0002D1C7
		public StringHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0002EFE8 File Offset: 0x0002D1E8
		public virtual uint GetStringIndex(string @string)
		{
			uint num;
			if (this.strings.TryGetValue(@string, out num))
			{
				return num;
			}
			num = (uint)(this.strings.Count + 1);
			this.strings.Add(@string, num);
			return num;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002F024 File Offset: 0x0002D224
		public uint[] WriteStrings()
		{
			List<KeyValuePair<string, uint>> list = StringHeapBuffer.SortStrings(this.strings);
			this.strings = null;
			uint[] array = new uint[list.Count + 1];
			array[0] = 0U;
			string text = string.Empty;
			foreach (KeyValuePair<string, uint> keyValuePair in list)
			{
				string key = keyValuePair.Key;
				uint value = keyValuePair.Value;
				int position = this.position;
				if (text.EndsWith(key, StringComparison.Ordinal) && !StringHeapBuffer.IsLowSurrogateChar((int)keyValuePair.Key[0]))
				{
					array[(int)value] = (uint)(position - (Encoding.UTF8.GetByteCount(keyValuePair.Key) + 1));
				}
				else
				{
					array[(int)value] = (uint)position;
					this.WriteString(key);
				}
				text = keyValuePair.Key;
			}
			return array;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002F100 File Offset: 0x0002D300
		private static List<KeyValuePair<string, uint>> SortStrings(Dictionary<string, uint> strings)
		{
			List<KeyValuePair<string, uint>> list = new List<KeyValuePair<string, uint>>(strings);
			list.Sort(new StringHeapBuffer.SuffixSort());
			return list;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002F113 File Offset: 0x0002D313
		private static bool IsLowSurrogateChar(int c)
		{
			return c - 56320 <= 1023;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002F126 File Offset: 0x0002D326
		protected virtual void WriteString(string @string)
		{
			base.WriteBytes(Encoding.UTF8.GetBytes(@string));
			base.WriteByte(0);
		}

		// Token: 0x04000608 RID: 1544
		protected Dictionary<string, uint> strings = new Dictionary<string, uint>(StringComparer.Ordinal);

		// Token: 0x020001A4 RID: 420
		private class SuffixSort : IComparer<KeyValuePair<string, uint>>
		{
			// Token: 0x06000D9D RID: 3485 RVA: 0x0002F140 File Offset: 0x0002D340
			public int Compare(KeyValuePair<string, uint> xPair, KeyValuePair<string, uint> yPair)
			{
				string key = xPair.Key;
				string key2 = yPair.Key;
				int num = key.Length - 1;
				int num2 = key2.Length - 1;
				while ((num >= 0) & (num2 >= 0))
				{
					if (key[num] < key2[num2])
					{
						return -1;
					}
					if (key[num] > key2[num2])
					{
						return 1;
					}
					num--;
					num2--;
				}
				return key2.Length.CompareTo(key.Length);
			}
		}
	}
}
