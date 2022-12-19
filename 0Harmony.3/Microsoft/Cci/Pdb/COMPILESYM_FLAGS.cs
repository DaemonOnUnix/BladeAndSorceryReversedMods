using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002BB RID: 699
	[Flags]
	internal enum COMPILESYM_FLAGS : uint
	{
		// Token: 0x04000D63 RID: 3427
		iLanguage = 255U,
		// Token: 0x04000D64 RID: 3428
		fEC = 256U,
		// Token: 0x04000D65 RID: 3429
		fNoDbgInfo = 512U,
		// Token: 0x04000D66 RID: 3430
		fLTCG = 1024U,
		// Token: 0x04000D67 RID: 3431
		fNoDataAlign = 2048U,
		// Token: 0x04000D68 RID: 3432
		fManagedPresent = 4096U,
		// Token: 0x04000D69 RID: 3433
		fSecurityChecks = 8192U,
		// Token: 0x04000D6A RID: 3434
		fHotPatch = 16384U,
		// Token: 0x04000D6B RID: 3435
		fCVTCIL = 32768U,
		// Token: 0x04000D6C RID: 3436
		fMSILModule = 65536U
	}
}
