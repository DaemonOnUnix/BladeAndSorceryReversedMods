using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AB RID: 427
	internal abstract class Heap
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x0002F347 File Offset: 0x0002D547
		protected Heap(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x04000640 RID: 1600
		public int IndexSize;

		// Token: 0x04000641 RID: 1601
		internal readonly byte[] data;
	}
}
