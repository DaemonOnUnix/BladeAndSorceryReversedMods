using System;

namespace Mono.Cecil
{
	// Token: 0x0200013E RID: 318
	internal struct Range
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x000239A0 File Offset: 0x00021BA0
		public Range(uint index, uint length)
		{
			this.Start = index;
			this.Length = length;
		}

		// Token: 0x04000331 RID: 817
		public uint Start;

		// Token: 0x04000332 RID: 818
		public uint Length;
	}
}
