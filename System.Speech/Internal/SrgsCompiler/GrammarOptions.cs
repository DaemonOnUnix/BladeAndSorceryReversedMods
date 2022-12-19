using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A3 RID: 163
	[Flags]
	internal enum GrammarOptions
	{
		// Token: 0x0400034E RID: 846
		KeyValuePairs = 0,
		// Token: 0x0400034F RID: 847
		MssV1 = 1,
		// Token: 0x04000350 RID: 848
		KeyValuePairSrgs = 2,
		// Token: 0x04000351 RID: 849
		IpaPhoneme = 4,
		// Token: 0x04000352 RID: 850
		W3cV1 = 8,
		// Token: 0x04000353 RID: 851
		STG = 16,
		// Token: 0x04000354 RID: 852
		TagFormat = 11,
		// Token: 0x04000355 RID: 853
		SemanticInterpretation = 9
	}
}
