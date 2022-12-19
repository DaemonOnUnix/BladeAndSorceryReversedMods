using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002E6 RID: 742
	[Flags]
	internal enum EXPORTSYM_FLAGS : ushort
	{
		// Token: 0x04000E60 RID: 3680
		fConstant = 1,
		// Token: 0x04000E61 RID: 3681
		fData = 2,
		// Token: 0x04000E62 RID: 3682
		fPrivate = 4,
		// Token: 0x04000E63 RID: 3683
		fNoName = 8,
		// Token: 0x04000E64 RID: 3684
		fOrdinal = 16,
		// Token: 0x04000E65 RID: 3685
		fForwarder = 32
	}
}
