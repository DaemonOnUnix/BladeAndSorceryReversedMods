using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A7 RID: 423
	internal sealed class PdbHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00011F38 File Offset: 0x00010138
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002F2EF File Offset: 0x0002D4EF
		public PdbHeapBuffer()
			: base(0)
		{
		}
	}
}
