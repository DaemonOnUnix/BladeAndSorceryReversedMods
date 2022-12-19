using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A0 RID: 416
	internal sealed class DataBuffer : ByteBuffer
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x0002EEFF File Offset: 0x0002D0FF
		public DataBuffer()
			: base(0)
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0002EF20 File Offset: 0x0002D120
		public uint AddData(byte[] data)
		{
			uint position = (uint)this.position;
			base.WriteBytes(data);
			return position;
		}
	}
}
