using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002B9 RID: 697
	[Flags]
	internal enum CFLAGSYM_FLAGS : ushort
	{
		// Token: 0x04000D58 RID: 3416
		pcode = 1,
		// Token: 0x04000D59 RID: 3417
		floatprec = 6,
		// Token: 0x04000D5A RID: 3418
		floatpkg = 24,
		// Token: 0x04000D5B RID: 3419
		ambdata = 224,
		// Token: 0x04000D5C RID: 3420
		ambcode = 1792,
		// Token: 0x04000D5D RID: 3421
		mode32 = 2048
	}
}
