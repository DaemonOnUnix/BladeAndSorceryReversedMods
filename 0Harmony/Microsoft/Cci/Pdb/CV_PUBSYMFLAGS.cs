using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003BD RID: 957
	[Flags]
	internal enum CV_PUBSYMFLAGS : uint
	{
		// Token: 0x04000DD6 RID: 3542
		fNone = 0U,
		// Token: 0x04000DD7 RID: 3543
		fCode = 1U,
		// Token: 0x04000DD8 RID: 3544
		fFunction = 2U,
		// Token: 0x04000DD9 RID: 3545
		fManaged = 4U,
		// Token: 0x04000DDA RID: 3546
		fMSIL = 8U
	}
}
