using System;

namespace Mono.Cecil
{
	// Token: 0x02000141 RID: 321
	public enum MethodCallingConvention : byte
	{
		// Token: 0x0400036A RID: 874
		Default,
		// Token: 0x0400036B RID: 875
		C,
		// Token: 0x0400036C RID: 876
		StdCall,
		// Token: 0x0400036D RID: 877
		ThisCall,
		// Token: 0x0400036E RID: 878
		FastCall,
		// Token: 0x0400036F RID: 879
		VarArg,
		// Token: 0x04000370 RID: 880
		Generic = 16
	}
}
