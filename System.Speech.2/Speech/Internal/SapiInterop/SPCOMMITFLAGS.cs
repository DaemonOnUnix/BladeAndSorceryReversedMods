using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000132 RID: 306
	[Flags]
	internal enum SPCOMMITFLAGS
	{
		// Token: 0x04000724 RID: 1828
		SPCF_NONE = 0,
		// Token: 0x04000725 RID: 1829
		SPCF_ADD_TO_USER_LEXICON = 1,
		// Token: 0x04000726 RID: 1830
		SPCF_DEFINITE_CORRECTION = 2
	}
}
