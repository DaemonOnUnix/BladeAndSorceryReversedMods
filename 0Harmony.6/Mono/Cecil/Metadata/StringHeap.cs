using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B4 RID: 436
	internal class StringHeap : Heap
	{
		// Token: 0x06000DBA RID: 3514 RVA: 0x0002F52B File Offset: 0x0002D72B
		public StringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002F540 File Offset: 0x0002D740
		public string Read(uint index)
		{
			if (index == 0U)
			{
				return string.Empty;
			}
			string text;
			if (this.strings.TryGetValue(index, out text))
			{
				return text;
			}
			if ((ulong)index > (ulong)((long)(this.data.Length - 1)))
			{
				return string.Empty;
			}
			text = this.ReadStringAt(index);
			if (text.Length != 0)
			{
				this.strings.Add(index, text);
			}
			return text;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002F59C File Offset: 0x0002D79C
		protected virtual string ReadStringAt(uint index)
		{
			int num = 0;
			int num2 = (int)index;
			while (this.data[num2] != 0)
			{
				num++;
				num2++;
			}
			return Encoding.UTF8.GetString(this.data, (int)index, num);
		}

		// Token: 0x04000663 RID: 1635
		private readonly Dictionary<uint, string> strings = new Dictionary<uint, string>();
	}
}
