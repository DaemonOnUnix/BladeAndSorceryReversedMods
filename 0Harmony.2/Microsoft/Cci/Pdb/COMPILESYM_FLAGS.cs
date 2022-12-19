using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003B1 RID: 945
	[Flags]
	internal enum COMPILESYM_FLAGS : uint
	{
		// Token: 0x04000DA2 RID: 3490
		iLanguage = 255U,
		// Token: 0x04000DA3 RID: 3491
		fEC = 256U,
		// Token: 0x04000DA4 RID: 3492
		fNoDbgInfo = 512U,
		// Token: 0x04000DA5 RID: 3493
		fLTCG = 1024U,
		// Token: 0x04000DA6 RID: 3494
		fNoDataAlign = 2048U,
		// Token: 0x04000DA7 RID: 3495
		fManagedPresent = 4096U,
		// Token: 0x04000DA8 RID: 3496
		fSecurityChecks = 8192U,
		// Token: 0x04000DA9 RID: 3497
		fHotPatch = 16384U,
		// Token: 0x04000DAA RID: 3498
		fCVTCIL = 32768U,
		// Token: 0x04000DAB RID: 3499
		fMSILModule = 65536U
	}
}
