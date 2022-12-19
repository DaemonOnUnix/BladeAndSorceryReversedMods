using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000349 RID: 841
	[Flags]
	internal enum CV_prop : ushort
	{
		// Token: 0x04000BFE RID: 3070
		packed = 1,
		// Token: 0x04000BFF RID: 3071
		ctor = 2,
		// Token: 0x04000C00 RID: 3072
		ovlops = 4,
		// Token: 0x04000C01 RID: 3073
		isnested = 8,
		// Token: 0x04000C02 RID: 3074
		cnested = 16,
		// Token: 0x04000C03 RID: 3075
		opassign = 32,
		// Token: 0x04000C04 RID: 3076
		opcast = 64,
		// Token: 0x04000C05 RID: 3077
		fwdref = 128,
		// Token: 0x04000C06 RID: 3078
		scoped = 256
	}
}
