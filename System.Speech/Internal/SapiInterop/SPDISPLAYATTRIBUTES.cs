using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200005C RID: 92
	[Flags]
	internal enum SPDISPLAYATTRIBUTES
	{
		// Token: 0x040001B6 RID: 438
		SPAF_ZERO_TRAILING_SPACE = 0,
		// Token: 0x040001B7 RID: 439
		SPAF_ONE_TRAILING_SPACE = 2,
		// Token: 0x040001B8 RID: 440
		SPAF_TWO_TRAILING_SPACES = 4,
		// Token: 0x040001B9 RID: 441
		SPAF_CONSUME_LEADING_SPACES = 8,
		// Token: 0x040001BA RID: 442
		SPAF_USER_SPECIFIED = 128
	}
}
