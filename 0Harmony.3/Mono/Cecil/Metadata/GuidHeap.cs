using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AA RID: 426
	internal sealed class GuidHeap : Heap
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002EA27 File Offset: 0x0002CC27
		public GuidHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002F2F8 File Offset: 0x0002D4F8
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
