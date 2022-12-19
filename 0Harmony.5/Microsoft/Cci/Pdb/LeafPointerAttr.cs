using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000352 RID: 850
	[Flags]
	internal enum LeafPointerAttr : uint
	{
		// Token: 0x04000C1E RID: 3102
		ptrtype = 31U,
		// Token: 0x04000C1F RID: 3103
		ptrmode = 224U,
		// Token: 0x04000C20 RID: 3104
		isflat32 = 256U,
		// Token: 0x04000C21 RID: 3105
		isvolatile = 512U,
		// Token: 0x04000C22 RID: 3106
		isconst = 1024U,
		// Token: 0x04000C23 RID: 3107
		isunaligned = 2048U,
		// Token: 0x04000C24 RID: 3108
		isrestrict = 4096U
	}
}
