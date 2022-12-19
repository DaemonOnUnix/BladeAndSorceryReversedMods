using System;
using System.Collections.Generic;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A5 RID: 421
	internal sealed class BlobHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0002EFB9 File Offset: 0x0002D1B9
		public override bool IsEmpty
		{
			get
			{
				return this.length <= 1;
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002F1C3 File Offset: 0x0002D3C3
		public BlobHeapBuffer()
			: base(1)
		{
			base.WriteByte(0);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002F1E4 File Offset: 0x0002D3E4
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

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002F21F File Offset: 0x0002D41F
		private void WriteBlob(ByteBuffer blob)
		{
			base.WriteCompressedUInt32((uint)blob.length);
			base.WriteBytes(blob);
		}

		// Token: 0x04000609 RID: 1545
		private readonly Dictionary<ByteBuffer, uint> blobs = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
	}
}
