using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002DD RID: 733
	[Flags]
	internal enum FRAMEPROCSYM_FLAGS : uint
	{
		// Token: 0x04000E27 RID: 3623
		fHasAlloca = 1U,
		// Token: 0x04000E28 RID: 3624
		fHasSetJmp = 2U,
		// Token: 0x04000E29 RID: 3625
		fHasLongJmp = 4U,
		// Token: 0x04000E2A RID: 3626
		fHasInlAsm = 8U,
		// Token: 0x04000E2B RID: 3627
		fHasEH = 16U,
		// Token: 0x04000E2C RID: 3628
		fInlSpec = 32U,
		// Token: 0x04000E2D RID: 3629
		fHasSEH = 64U,
		// Token: 0x04000E2E RID: 3630
		fNaked = 128U,
		// Token: 0x04000E2F RID: 3631
		fSecurityChecks = 256U,
		// Token: 0x04000E30 RID: 3632
		fAsyncEH = 512U,
		// Token: 0x04000E31 RID: 3633
		fGSNoStackOrdering = 1024U,
		// Token: 0x04000E32 RID: 3634
		fWasInlined = 2048U
	}
}
