using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000EE RID: 238
	[Flags]
	internal enum GrammarOptions
	{
		// Token: 0x040005FF RID: 1535
		KeyValuePairs = 0,
		// Token: 0x04000600 RID: 1536
		MssV1 = 1,
		// Token: 0x04000601 RID: 1537
		KeyValuePairSrgs = 2,
		// Token: 0x04000602 RID: 1538
		IpaPhoneme = 4,
		// Token: 0x04000603 RID: 1539
		W3cV1 = 8,
		// Token: 0x04000604 RID: 1540
		STG = 16,
		// Token: 0x04000605 RID: 1541
		TagFormat = 11,
		// Token: 0x04000606 RID: 1542
		SemanticInterpretation = 9
	}
}
