using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019F RID: 415
	internal sealed class ResourceBuffer : ByteBuffer
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x0002EEFF File Offset: 0x0002D0FF
		public ResourceBuffer()
			: base(0)
		{
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0002EF08 File Offset: 0x0002D108
		public uint AddResource(byte[] resource)
		{
			uint position = (uint)this.position;
			base.WriteInt32(resource.Length);
			base.WriteBytes(resource);
			return position;
		}
	}
}
