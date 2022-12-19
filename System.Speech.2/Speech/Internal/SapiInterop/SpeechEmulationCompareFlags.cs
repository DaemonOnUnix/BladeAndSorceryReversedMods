using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000128 RID: 296
	[Flags]
	internal enum SpeechEmulationCompareFlags
	{
		// Token: 0x040006F2 RID: 1778
		SECFIgnoreCase = 1,
		// Token: 0x040006F3 RID: 1779
		SECFIgnoreKanaType = 65536,
		// Token: 0x040006F4 RID: 1780
		SECFIgnoreWidth = 131072,
		// Token: 0x040006F5 RID: 1781
		SECFNoSpecialChars = 536870912,
		// Token: 0x040006F6 RID: 1782
		SECFEmulateResult = 1073741824,
		// Token: 0x040006F7 RID: 1783
		SECFDefault = 196609
	}
}
