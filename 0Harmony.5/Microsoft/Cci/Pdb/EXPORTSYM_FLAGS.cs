using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003DC RID: 988
	[Flags]
	internal enum EXPORTSYM_FLAGS : ushort
	{
		// Token: 0x04000E9F RID: 3743
		fConstant = 1,
		// Token: 0x04000EA0 RID: 3744
		fData = 2,
		// Token: 0x04000EA1 RID: 3745
		fPrivate = 4,
		// Token: 0x04000EA2 RID: 3746
		fNoName = 8,
		// Token: 0x04000EA3 RID: 3747
		fOrdinal = 16,
		// Token: 0x04000EA4 RID: 3748
		fForwarder = 32
	}
}
