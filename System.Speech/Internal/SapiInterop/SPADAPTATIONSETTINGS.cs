using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000052 RID: 82
	[Flags]
	internal enum SPADAPTATIONSETTINGS
	{
		// Token: 0x04000187 RID: 391
		SPADS_Default = 0,
		// Token: 0x04000188 RID: 392
		SPADS_CurrentRecognizer = 1,
		// Token: 0x04000189 RID: 393
		SPADS_RecoProfile = 2,
		// Token: 0x0400018A RID: 394
		SPADS_Immediate = 4,
		// Token: 0x0400018B RID: 395
		SPADS_Reset = 8
	}
}
