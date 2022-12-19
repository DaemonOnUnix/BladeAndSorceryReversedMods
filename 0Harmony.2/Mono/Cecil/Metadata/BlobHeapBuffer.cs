using System;
using System.Collections.Generic;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200029A RID: 666
	internal sealed class BlobHeapBuffer : HeapBuffer
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x000369E1 File Offset: 0x00034BE1
		public override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00036BEB File Offset: 0x00034DEB
		public BlobHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00036C0C File Offset: 0x00034E0C
		public uint GetBlobIndex(ByteBuffer blob)
		{
			uint position;
			if (this.blobs.TryGetValue(blob, out position))
			{
				return position;
			}
			position = (uint)this.position;
			this.WriteBlob(blob);
			this.blobs.Add(blob, position);
			return position;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00036C47 File Offset: 0x00034E47
		private void WriteBlob(ByteBuffer blob)
		{
			base.WriteCompressedUInt32((uint)blob.length);
			base.WriteBytes(blob);
		}

		// Token: 0x04000641 RID: 1601
		private readonly Dictionary<ByteBuffer, uint> blobs = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
	}
}
