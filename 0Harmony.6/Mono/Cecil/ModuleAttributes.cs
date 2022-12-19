using System;

namespace Mono.Cecil
{
	// Token: 0x02000156 RID: 342
	[Flags]
	public enum ModuleAttributes
	{
		// Token: 0x04000410 RID: 1040
		ILOnly = 1,
		// Token: 0x04000411 RID: 1041
		Required32Bit = 2,
		// Token: 0x04000412 RID: 1042
		ILLibrary = 4,
		// Token: 0x04000413 RID: 1043
		StrongNameSigned = 8,
		// Token: 0x04000414 RID: 1044
		Preferred32Bit = 131072
	}
}
