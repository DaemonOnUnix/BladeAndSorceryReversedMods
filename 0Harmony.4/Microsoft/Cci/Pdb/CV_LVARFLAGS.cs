using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200039F RID: 927
	[Flags]
	internal enum CV_LVARFLAGS : ushort
	{
		// Token: 0x04000D55 RID: 3413
		fIsParam = 1,
		// Token: 0x04000D56 RID: 3414
		fAddrTaken = 2,
		// Token: 0x04000D57 RID: 3415
		fCompGenx = 4,
		// Token: 0x04000D58 RID: 3416
		fIsAggregate = 8,
		// Token: 0x04000D59 RID: 3417
		fIsAggregated = 16,
		// Token: 0x04000D5A RID: 3418
		fIsAliased = 32,
		// Token: 0x04000D5B RID: 3419
		fIsAlias = 64
	}
}
