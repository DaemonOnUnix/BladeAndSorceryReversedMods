using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B8 RID: 440
	internal sealed class UserStringHeap : StringHeap
	{
		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002F61C File Offset: 0x0002D81C
		public UserStringHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002F628 File Offset: 0x0002D828
		protected override string ReadStringAt(uint index)
		{
			int num = (int)index;
			uint num2 = (uint)((ulong)this.data.ReadCompressedUInt32(ref num) & 18446744073709551614UL);
			if (num2 < 1U)
			{
				return string.Empty;
			}
			char[] array = new char[num2 / 2U];
			int num3 = num;
			int num4 = 0;
			while ((long)num3 < (long)num + (long)((ulong)num2))
			{
				array[num4++] = (char)((int)this.data[num3] | ((int)this.data[num3 + 1] << 8));
				num3 += 2;
			}
			return new string(array);
		}
	}
}
