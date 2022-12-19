using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200039D RID: 925
	[Flags]
	internal enum CV_PROCFLAGS : byte
	{
		// Token: 0x04000D4A RID: 3402
		CV_PFLAG_NOFPO = 1,
		// Token: 0x04000D4B RID: 3403
		CV_PFLAG_INT = 2,
		// Token: 0x04000D4C RID: 3404
		CV_PFLAG_FAR = 4,
		// Token: 0x04000D4D RID: 3405
		CV_PFLAG_NEVER = 8,
		// Token: 0x04000D4E RID: 3406
		CV_PFLAG_NOTREACHED = 16,
		// Token: 0x04000D4F RID: 3407
		CV_PFLAG_CUST_CALL = 32,
		// Token: 0x04000D50 RID: 3408
		CV_PFLAG_NOINLINE = 64,
		// Token: 0x04000D51 RID: 3409
		CV_PFLAG_OPTDBGINFO = 128
	}
}
