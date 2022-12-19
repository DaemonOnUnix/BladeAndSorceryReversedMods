using System;

namespace Mono.Cecil
{
	// Token: 0x02000231 RID: 561
	internal struct Range
	{
		// Token: 0x06000C42 RID: 3138 RVA: 0x00029C78 File Offset: 0x00027E78
		public Range(uint index, uint length)
		{
			this.Start = index;
			this.Length = length;
		}

		// Token: 0x04000363 RID: 867
		public uint Start;

		// Token: 0x04000364 RID: 868
		public uint Length;
	}
}
