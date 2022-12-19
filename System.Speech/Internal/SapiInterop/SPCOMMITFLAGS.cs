using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200005B RID: 91
	[Flags]
	internal enum SPCOMMITFLAGS
	{
		// Token: 0x040001B2 RID: 434
		SPCF_NONE = 0,
		// Token: 0x040001B3 RID: 435
		SPCF_ADD_TO_USER_LEXICON = 1,
		// Token: 0x040001B4 RID: 436
		SPCF_DEFINITE_CORRECTION = 2
	}
}
