using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000028 RID: 40
	[Flags]
	internal enum SPEAKFLAGS
	{
		// Token: 0x0400022B RID: 555
		SPF_DEFAULT = 0,
		// Token: 0x0400022C RID: 556
		SPF_ASYNC = 1,
		// Token: 0x0400022D RID: 557
		SPF_PURGEBEFORESPEAK = 2,
		// Token: 0x0400022E RID: 558
		SPF_IS_FILENAME = 4,
		// Token: 0x0400022F RID: 559
		SPF_IS_XML = 8,
		// Token: 0x04000230 RID: 560
		SPF_IS_NOT_XML = 16,
		// Token: 0x04000231 RID: 561
		SPF_PERSIST_XML = 32,
		// Token: 0x04000232 RID: 562
		SPF_NLP_SPEAK_PUNC = 64,
		// Token: 0x04000233 RID: 563
		SPF_PARSE_SAPI = 128,
		// Token: 0x04000234 RID: 564
		SPF_PARSE_SSML = 256
	}
}
