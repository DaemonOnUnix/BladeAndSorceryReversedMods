using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003D3 RID: 979
	[Flags]
	internal enum FRAMEPROCSYM_FLAGS : uint
	{
		// Token: 0x04000E66 RID: 3686
		fHasAlloca = 1U,
		// Token: 0x04000E67 RID: 3687
		fHasSetJmp = 2U,
		// Token: 0x04000E68 RID: 3688
		fHasLongJmp = 4U,
		// Token: 0x04000E69 RID: 3689
		fHasInlAsm = 8U,
		// Token: 0x04000E6A RID: 3690
		fHasEH = 16U,
		// Token: 0x04000E6B RID: 3691
		fInlSpec = 32U,
		// Token: 0x04000E6C RID: 3692
		fHasSEH = 64U,
		// Token: 0x04000E6D RID: 3693
		fNaked = 128U,
		// Token: 0x04000E6E RID: 3694
		fSecurityChecks = 256U,
		// Token: 0x04000E6F RID: 3695
		fAsyncEH = 512U,
		// Token: 0x04000E70 RID: 3696
		fGSNoStackOrdering = 1024U,
		// Token: 0x04000E71 RID: 3697
		fWasInlined = 2048U
	}
}
