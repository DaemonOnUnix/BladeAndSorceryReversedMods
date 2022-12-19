using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C7 RID: 711
	[Flags]
	internal enum CV_PUBSYMFLAGS : uint
	{
		// Token: 0x04000D97 RID: 3479
		fNone = 0U,
		// Token: 0x04000D98 RID: 3480
		fCode = 1U,
		// Token: 0x04000D99 RID: 3481
		fFunction = 2U,
		// Token: 0x04000D9A RID: 3482
		fManaged = 4U,
		// Token: 0x04000D9B RID: 3483
		fMSIL = 8U
	}
}
