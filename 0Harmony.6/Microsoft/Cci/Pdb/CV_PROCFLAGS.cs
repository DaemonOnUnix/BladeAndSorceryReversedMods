using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002A7 RID: 679
	[Flags]
	internal enum CV_PROCFLAGS : byte
	{
		// Token: 0x04000D0B RID: 3339
		CV_PFLAG_NOFPO = 1,
		// Token: 0x04000D0C RID: 3340
		CV_PFLAG_INT = 2,
		// Token: 0x04000D0D RID: 3341
		CV_PFLAG_FAR = 4,
		// Token: 0x04000D0E RID: 3342
		CV_PFLAG_NEVER = 8,
		// Token: 0x04000D0F RID: 3343
		CV_PFLAG_NOTREACHED = 16,
		// Token: 0x04000D10 RID: 3344
		CV_PFLAG_CUST_CALL = 32,
		// Token: 0x04000D11 RID: 3345
		CV_PFLAG_NOINLINE = 64,
		// Token: 0x04000D12 RID: 3346
		CV_PFLAG_OPTDBGINFO = 128
	}
}
