using System;

namespace Mono.Cecil
{
	// Token: 0x0200024A RID: 586
	[Flags]
	public enum ModuleAttributes
	{
		// Token: 0x04000445 RID: 1093
		ILOnly = 1,
		// Token: 0x04000446 RID: 1094
		Required32Bit = 2,
		// Token: 0x04000447 RID: 1095
		ILLibrary = 4,
		// Token: 0x04000448 RID: 1096
		StrongNameSigned = 8,
		// Token: 0x04000449 RID: 1097
		Preferred32Bit = 131072
	}
}
