using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A0 RID: 672
	internal abstract class Heap
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x00036D6F File Offset: 0x00034F6F
		protected Heap(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x04000678 RID: 1656
		public int IndexSize;

		// Token: 0x04000679 RID: 1657
		internal readonly byte[] data;
	}
}
