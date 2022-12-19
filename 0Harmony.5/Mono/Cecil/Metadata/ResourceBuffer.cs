using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000294 RID: 660
	internal sealed class ResourceBuffer : ByteBuffer
	{
		// Token: 0x060010EE RID: 4334 RVA: 0x00036927 File Offset: 0x00034B27
		public ResourceBuffer()
			: base(0)
		{
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00036930 File Offset: 0x00034B30
		public uint AddResource(byte[] resource)
		{
			uint position = (uint)this.position;
			base.WriteInt32(resource.Length);
			base.WriteBytes(resource);
			return position;
		}
	}
}
