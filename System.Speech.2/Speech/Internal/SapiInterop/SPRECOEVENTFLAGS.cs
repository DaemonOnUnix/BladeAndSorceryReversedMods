using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200012B RID: 299
	[Flags]
	internal enum SPRECOEVENTFLAGS
	{
		// Token: 0x04000704 RID: 1796
		SPREF_AutoPause = 1,
		// Token: 0x04000705 RID: 1797
		SPREF_Emulated = 2,
		// Token: 0x04000706 RID: 1798
		SPREF_SMLTimeout = 4,
		// Token: 0x04000707 RID: 1799
		SPREF_ExtendableParse = 8,
		// Token: 0x04000708 RID: 1800
		SPREF_ReSent = 16,
		// Token: 0x04000709 RID: 1801
		SPREF_Hypothesis = 32,
		// Token: 0x0400070A RID: 1802
		SPREF_FalseRecognition = 64
	}
}
