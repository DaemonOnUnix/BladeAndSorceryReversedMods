using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000E0 RID: 224
	internal enum MatchMode
	{
		// Token: 0x0400059A RID: 1434
		AllWords,
		// Token: 0x0400059B RID: 1435
		Subsequence,
		// Token: 0x0400059C RID: 1436
		OrderedSubset = 3,
		// Token: 0x0400059D RID: 1437
		SubsequenceContentRequired = 5,
		// Token: 0x0400059E RID: 1438
		OrderedSubsetContentRequired = 7
	}
}
