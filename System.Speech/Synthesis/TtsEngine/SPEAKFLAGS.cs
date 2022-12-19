using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200015E RID: 350
	[Flags]
	internal enum SPEAKFLAGS
	{
		// Token: 0x040006A5 RID: 1701
		SPF_DEFAULT = 0,
		// Token: 0x040006A6 RID: 1702
		SPF_ASYNC = 1,
		// Token: 0x040006A7 RID: 1703
		SPF_PURGEBEFORESPEAK = 2,
		// Token: 0x040006A8 RID: 1704
		SPF_IS_FILENAME = 4,
		// Token: 0x040006A9 RID: 1705
		SPF_IS_XML = 8,
		// Token: 0x040006AA RID: 1706
		SPF_IS_NOT_XML = 16,
		// Token: 0x040006AB RID: 1707
		SPF_PERSIST_XML = 32,
		// Token: 0x040006AC RID: 1708
		SPF_NLP_SPEAK_PUNC = 64,
		// Token: 0x040006AD RID: 1709
		SPF_PARSE_SAPI = 128,
		// Token: 0x040006AE RID: 1710
		SPF_PARSE_SSML = 256
	}
}
