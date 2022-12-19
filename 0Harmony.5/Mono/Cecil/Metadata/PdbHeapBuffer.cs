using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200029C RID: 668
	internal sealed class PdbHeapBuffer : HeapBuffer
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00036D17 File Offset: 0x00034F17
		public PdbHeapBuffer()
			: base(0)
		{
		}
	}
}
