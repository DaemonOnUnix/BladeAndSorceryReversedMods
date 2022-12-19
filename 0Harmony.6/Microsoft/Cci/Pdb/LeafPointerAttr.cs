using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200025C RID: 604
	[Flags]
	internal enum LeafPointerAttr : uint
	{
		// Token: 0x04000BDF RID: 3039
		ptrtype = 31U,
		// Token: 0x04000BE0 RID: 3040
		ptrmode = 224U,
		// Token: 0x04000BE1 RID: 3041
		isflat32 = 256U,
		// Token: 0x04000BE2 RID: 3042
		isvolatile = 512U,
		// Token: 0x04000BE3 RID: 3043
		isconst = 1024U,
		// Token: 0x04000BE4 RID: 3044
		isunaligned = 2048U,
		// Token: 0x04000BE5 RID: 3045
		isrestrict = 4096U
	}
}
