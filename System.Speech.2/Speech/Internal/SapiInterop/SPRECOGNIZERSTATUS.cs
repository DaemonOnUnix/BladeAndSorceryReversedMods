using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200013F RID: 319
	internal struct SPRECOGNIZERSTATUS
	{
		// Token: 0x0400075D RID: 1885
		internal SPAUDIOSTATUS AudioStatus;

		// Token: 0x0400075E RID: 1886
		internal ulong ullRecognitionStreamPos;

		// Token: 0x0400075F RID: 1887
		internal uint ulStreamNumber;

		// Token: 0x04000760 RID: 1888
		internal uint ulNumActive;

		// Token: 0x04000761 RID: 1889
		internal Guid clsidEngine;

		// Token: 0x04000762 RID: 1890
		internal uint cLangIDs;

		// Token: 0x04000763 RID: 1891
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		internal short[] aLangID;

		// Token: 0x04000764 RID: 1892
		internal ulong ullRecognitionStreamTime;
	}
}
