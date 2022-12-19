using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200029F RID: 671
	internal sealed class GuidHeap : Heap
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x0003644F File Offset: 0x0003464F
		public GuidHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00036D20 File Offset: 0x00034F20
		public Guid Read(uint index)
		{
			if (index == 0U || (ulong)(index - 1U + 16U) > (ulong)((long)this.data.Length))
			{
				return default(Guid);
			}
			byte[] array = new byte[16];
			Buffer.BlockCopy(this.data, (int)((index - 1U) * 16U), array, 0, 16);
			return new Guid(array);
		}
	}
}
