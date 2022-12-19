using System;

namespace Mono.Cecil
{
	// Token: 0x02000162 RID: 354
	[Flags]
	public enum PropertyAttributes : ushort
	{
		// Token: 0x04000470 RID: 1136
		None = 0,
		// Token: 0x04000471 RID: 1137
		SpecialName = 512,
		// Token: 0x04000472 RID: 1138
		RTSpecialName = 1024,
		// Token: 0x04000473 RID: 1139
		HasDefault = 4096,
		// Token: 0x04000474 RID: 1140
		Unused = 59903
	}
}
