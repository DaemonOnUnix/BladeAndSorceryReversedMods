using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014E RID: 334
	internal struct SPVOICESTATUS
	{
		// Token: 0x04000808 RID: 2056
		internal uint ulCurrentStream;

		// Token: 0x04000809 RID: 2057
		internal uint ulLastStreamQueued;

		// Token: 0x0400080A RID: 2058
		internal int hrLastResult;

		// Token: 0x0400080B RID: 2059
		internal SpeechRunState dwRunningState;

		// Token: 0x0400080C RID: 2060
		internal uint ulInputWordPos;

		// Token: 0x0400080D RID: 2061
		internal uint ulInputWordLen;

		// Token: 0x0400080E RID: 2062
		internal uint ulInputSentPos;

		// Token: 0x0400080F RID: 2063
		internal uint ulInputSentLen;

		// Token: 0x04000810 RID: 2064
		internal int lBookmarkId;

		// Token: 0x04000811 RID: 2065
		internal ushort PhonemeId;

		// Token: 0x04000812 RID: 2066
		internal int VisemeId;

		// Token: 0x04000813 RID: 2067
		internal uint dwReserved1;

		// Token: 0x04000814 RID: 2068
		internal uint dwReserved2;
	}
}
