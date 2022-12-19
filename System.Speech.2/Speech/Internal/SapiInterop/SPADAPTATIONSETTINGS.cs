using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000129 RID: 297
	[Flags]
	internal enum SPADAPTATIONSETTINGS
	{
		// Token: 0x040006F9 RID: 1785
		SPADS_Default = 0,
		// Token: 0x040006FA RID: 1786
		SPADS_CurrentRecognizer = 1,
		// Token: 0x040006FB RID: 1787
		SPADS_RecoProfile = 2,
		// Token: 0x040006FC RID: 1788
		SPADS_Immediate = 4,
		// Token: 0x040006FD RID: 1789
		SPADS_Reset = 8
	}
}
