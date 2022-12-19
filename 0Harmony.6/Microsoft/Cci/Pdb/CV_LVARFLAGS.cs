using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002A9 RID: 681
	[Flags]
	internal enum CV_LVARFLAGS : ushort
	{
		// Token: 0x04000D16 RID: 3350
		fIsParam = 1,
		// Token: 0x04000D17 RID: 3351
		fAddrTaken = 2,
		// Token: 0x04000D18 RID: 3352
		fCompGenx = 4,
		// Token: 0x04000D19 RID: 3353
		fIsAggregate = 8,
		// Token: 0x04000D1A RID: 3354
		fIsAggregated = 16,
		// Token: 0x04000D1B RID: 3355
		fIsAliased = 32,
		// Token: 0x04000D1C RID: 3356
		fIsAlias = 64
	}
}
