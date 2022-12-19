using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A9 RID: 681
	internal class StringHeap : Heap
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x00036F53 File Offset: 0x00035153
		public StringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00036F68 File Offset: 0x00035168
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

		// Token: 0x0600111F RID: 4383 RVA: 0x00036FC4 File Offset: 0x000351C4
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

		// Token: 0x0400069B RID: 1691
		private readonly Dictionary<uint, string> strings = new Dictionary<uint, string>();
	}
}
