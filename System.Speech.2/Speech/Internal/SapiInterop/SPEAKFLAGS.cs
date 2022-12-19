using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000120 RID: 288
	[Flags]
	internal enum SPEAKFLAGS
	{
		// Token: 0x040006C1 RID: 1729
		SPF_DEFAULT = 0,
		// Token: 0x040006C2 RID: 1730
		SPF_ASYNC = 1,
		// Token: 0x040006C3 RID: 1731
		SPF_PURGEBEFORESPEAK = 2,
		// Token: 0x040006C4 RID: 1732
		SPF_IS_FILENAME = 4,
		// Token: 0x040006C5 RID: 1733
		SPF_IS_XML = 8,
		// Token: 0x040006C6 RID: 1734
		SPF_IS_NOT_XML = 16,
		// Token: 0x040006C7 RID: 1735
		SPF_PERSIST_XML = 32,
		// Token: 0x040006C8 RID: 1736
		SPF_NLP_SPEAK_PUNC = 64,
		// Token: 0x040006C9 RID: 1737
		SPF_PARSE_SAPI = 128,
		// Token: 0x040006CA RID: 1738
		SPF_PARSE_SSML = 256
	}
}
