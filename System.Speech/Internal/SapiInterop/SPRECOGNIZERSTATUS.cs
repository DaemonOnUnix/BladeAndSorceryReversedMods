using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000068 RID: 104
	internal struct SPRECOGNIZERSTATUS
	{
		// Token: 0x040001EB RID: 491
		internal SPAUDIOSTATUS AudioStatus;

		// Token: 0x040001EC RID: 492
		internal ulong ullRecognitionStreamPos;

		// Token: 0x040001ED RID: 493
		internal uint ulStreamNumber;

		// Token: 0x040001EE RID: 494
		internal uint ulNumActive;

		// Token: 0x040001EF RID: 495
		internal Guid clsidEngine;

		// Token: 0x040001F0 RID: 496
		internal uint cLangIDs;

		// Token: 0x040001F1 RID: 497
		[MarshalAs(30, SizeConst = 20)]
		internal short[] aLangID;

		// Token: 0x040001F2 RID: 498
		internal ulong ullRecognitionStreamTime;
	}
}
