using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000133 RID: 307
	[Flags]
	internal enum SPDISPLAYATTRIBUTES
	{
		// Token: 0x04000728 RID: 1832
		SPAF_ZERO_TRAILING_SPACE = 0,
		// Token: 0x04000729 RID: 1833
		SPAF_ONE_TRAILING_SPACE = 2,
		// Token: 0x0400072A RID: 1834
		SPAF_TWO_TRAILING_SPACES = 4,
		// Token: 0x0400072B RID: 1835
		SPAF_CONSUME_LEADING_SPACES = 8,
		// Token: 0x0400072C RID: 1836
		SPAF_USER_SPECIFIED = 128
	}
}
