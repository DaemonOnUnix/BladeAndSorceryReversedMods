using System;

namespace Mono.Cecil
{
	// Token: 0x02000234 RID: 564
	public enum MethodCallingConvention : byte
	{
		// Token: 0x0400039C RID: 924
		Default,
		// Token: 0x0400039D RID: 925
		C,
		// Token: 0x0400039E RID: 926
		StdCall,
		// Token: 0x0400039F RID: 927
		ThisCall,
		// Token: 0x040003A0 RID: 928
		FastCall,
		// Token: 0x040003A1 RID: 929
		VarArg,
		// Token: 0x040003A2 RID: 930
		Generic = 16
	}
}
