using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000077 RID: 119
	internal struct SPVOICESTATUS
	{
		// Token: 0x04000296 RID: 662
		internal uint ulCurrentStream;

		// Token: 0x04000297 RID: 663
		internal uint ulLastStreamQueued;

		// Token: 0x04000298 RID: 664
		internal int hrLastResult;

		// Token: 0x04000299 RID: 665
		internal SpeechRunState dwRunningState;

		// Token: 0x0400029A RID: 666
		internal uint ulInputWordPos;

		// Token: 0x0400029B RID: 667
		internal uint ulInputWordLen;

		// Token: 0x0400029C RID: 668
		internal uint ulInputSentPos;

		// Token: 0x0400029D RID: 669
		internal uint ulInputSentLen;

		// Token: 0x0400029E RID: 670
		internal int lBookmarkId;

		// Token: 0x0400029F RID: 671
		internal ushort PhonemeId;

		// Token: 0x040002A0 RID: 672
		internal int VisemeId;

		// Token: 0x040002A1 RID: 673
		internal uint dwReserved1;

		// Token: 0x040002A2 RID: 674
		internal uint dwReserved2;
	}
}
