using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000054 RID: 84
	[Flags]
	internal enum SPRECOEVENTFLAGS
	{
		// Token: 0x04000192 RID: 402
		SPREF_AutoPause = 1,
		// Token: 0x04000193 RID: 403
		SPREF_Emulated = 2,
		// Token: 0x04000194 RID: 404
		SPREF_SMLTimeout = 4,
		// Token: 0x04000195 RID: 405
		SPREF_ExtendableParse = 8,
		// Token: 0x04000196 RID: 406
		SPREF_ReSent = 16,
		// Token: 0x04000197 RID: 407
		SPREF_Hypothesis = 32,
		// Token: 0x04000198 RID: 408
		SPREF_FalseRecognition = 64
	}
}
