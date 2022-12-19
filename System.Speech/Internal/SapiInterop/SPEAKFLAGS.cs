using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000049 RID: 73
	[Flags]
	internal enum SPEAKFLAGS
	{
		// Token: 0x0400014F RID: 335
		SPF_DEFAULT = 0,
		// Token: 0x04000150 RID: 336
		SPF_ASYNC = 1,
		// Token: 0x04000151 RID: 337
		SPF_PURGEBEFORESPEAK = 2,
		// Token: 0x04000152 RID: 338
		SPF_IS_FILENAME = 4,
		// Token: 0x04000153 RID: 339
		SPF_IS_XML = 8,
		// Token: 0x04000154 RID: 340
		SPF_IS_NOT_XML = 16,
		// Token: 0x04000155 RID: 341
		SPF_PERSIST_XML = 32,
		// Token: 0x04000156 RID: 342
		SPF_NLP_SPEAK_PUNC = 64,
		// Token: 0x04000157 RID: 343
		SPF_PARSE_SAPI = 128,
		// Token: 0x04000158 RID: 344
		SPF_PARSE_SSML = 256
	}
}
