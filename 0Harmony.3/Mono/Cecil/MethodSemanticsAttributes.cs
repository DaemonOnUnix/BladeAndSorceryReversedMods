using System;

namespace Mono.Cecil
{
	// Token: 0x02000147 RID: 327
	[Flags]
	public enum MethodSemanticsAttributes : ushort
	{
		// Token: 0x0400039D RID: 925
		None = 0,
		// Token: 0x0400039E RID: 926
		Setter = 1,
		// Token: 0x0400039F RID: 927
		Getter = 2,
		// Token: 0x040003A0 RID: 928
		Other = 4,
		// Token: 0x040003A1 RID: 929
		AddOn = 8,
		// Token: 0x040003A2 RID: 930
		RemoveOn = 16,
		// Token: 0x040003A3 RID: 931
		Fire = 32
	}
}
