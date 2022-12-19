using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000292 RID: 658
	internal sealed class BlobHeap : Heap
	{
		// Token: 0x060010D6 RID: 4310 RVA: 0x0003644F File Offset: 0x0003464F
		public BlobHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00036458 File Offset: 0x00034658
		public byte[] Read(uint index)
		{
			if (index == 0U || (ulong)index > (ulong)((long)(this.data.Length - 1)))
			{
				return Empty<byte>.Array;
			}
			int num = (int)index;
			int num2 = (int)this.data.ReadCompressedUInt32(ref num);
			if (num2 > this.data.Length - num)
			{
				return Empty<byte>.Array;
			}
			byte[] array = new byte[num2];
			Buffer.BlockCopy(this.data, num, array, 0, num2);
			return array;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000364B8 File Offset: 0x000346B8
		public void GetView(uint signature, out byte[] buffer, out int index, out int length)
		{
			if (signature == 0U || (ulong)signature > (ulong)((long)(this.data.Length - 1)))
			{
				buffer = null;
				index = (length = 0);
				return;
			}
			buffer = this.data;
			index = (int)signature;
			length = (int)buffer.ReadCompressedUInt32(ref index);
		}
	}
}
