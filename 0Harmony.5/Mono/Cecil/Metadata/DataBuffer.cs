using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000295 RID: 661
	internal sealed class DataBuffer : ByteBuffer
	{
		// Token: 0x060010F0 RID: 4336 RVA: 0x00036927 File Offset: 0x00034B27
		public DataBuffer()
			: base(0)
		{
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00036948 File Offset: 0x00034B48
		public uint AddData(byte[] data)
		{
			uint position = (uint)this.position;
			base.WriteBytes(data);
			return position;
		}
	}
}
