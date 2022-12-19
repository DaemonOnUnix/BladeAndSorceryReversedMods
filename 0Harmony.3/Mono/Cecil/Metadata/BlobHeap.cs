using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019D RID: 413
	internal sealed class BlobHeap : Heap
	{
		// Token: 0x06000D73 RID: 3443 RVA: 0x0002EA27 File Offset: 0x0002CC27
		public BlobHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002EA30 File Offset: 0x0002CC30
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

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002EA90 File Offset: 0x0002CC90
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
