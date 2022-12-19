using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000C8 RID: 200
	internal enum MatchMode
	{
		// Token: 0x040003A7 RID: 935
		AllWords,
		// Token: 0x040003A8 RID: 936
		Subsequence,
		// Token: 0x040003A9 RID: 937
		OrderedSubset = 3,
		// Token: 0x040003AA RID: 938
		SubsequenceContentRequired = 5,
		// Token: 0x040003AB RID: 939
		OrderedSubsetContentRequired = 7
	}
}
