using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000051 RID: 81
	[Flags]
	internal enum SpeechEmulationCompareFlags
	{
		// Token: 0x04000180 RID: 384
		SECFIgnoreCase = 1,
		// Token: 0x04000181 RID: 385
		SECFIgnoreKanaType = 65536,
		// Token: 0x04000182 RID: 386
		SECFIgnoreWidth = 131072,
		// Token: 0x04000183 RID: 387
		SECFNoSpecialChars = 536870912,
		// Token: 0x04000184 RID: 388
		SECFEmulateResult = 1073741824,
		// Token: 0x04000185 RID: 389
		SECFDefault = 196609
	}
}
