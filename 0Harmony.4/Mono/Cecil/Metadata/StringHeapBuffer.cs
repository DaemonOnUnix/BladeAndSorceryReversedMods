using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000298 RID: 664
	internal class StringHeapBuffer : HeapBuffer
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x000369E1 File Offset: 0x00034BE1
		public sealed override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000369EF File Offset: 0x00034BEF
		public StringHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00036A10 File Offset: 0x00034C10
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

		// Token: 0x060010FC RID: 4348 RVA: 0x00036A4C File Offset: 0x00034C4C
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

		// Token: 0x060010FD RID: 4349 RVA: 0x00036B28 File Offset: 0x00034D28
		private static List<KeyValuePair<string, uint>> SortStrings(Dictionary<string, uint> strings)
		{
			List<KeyValuePair<string, uint>> list = new List<KeyValuePair<string, uint>>(strings);
			list.Sort(new StringHeapBuffer.SuffixSort());
			return list;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00036B3B File Offset: 0x00034D3B
		private static bool IsLowSurrogateChar(int c)
		{
			return c - 56320 <= 1023;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00036B4E File Offset: 0x00034D4E
		protected virtual void WriteString(string @string)
		{
			base.WriteBytes(Encoding.UTF8.GetBytes(@string));
			base.WriteByte(0);
		}

		// Token: 0x04000640 RID: 1600
		protected Dictionary<string, uint> strings = new Dictionary<string, uint>(StringComparer.Ordinal);

		// Token: 0x02000299 RID: 665
		private class SuffixSort : IComparer<KeyValuePair<string, uint>>
		{
			// Token: 0x06001100 RID: 4352 RVA: 0x00036B68 File Offset: 0x00034D68
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
