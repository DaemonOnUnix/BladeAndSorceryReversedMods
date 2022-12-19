using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200013E RID: 318
	internal struct SPAUDIOSTATUS
	{
		// Token: 0x04000756 RID: 1878
		internal int cbFreeBuffSpace;

		// Token: 0x04000757 RID: 1879
		internal uint cbNonBlockingIO;

		// Token: 0x04000758 RID: 1880
		internal SPAUDIOSTATE State;

		// Token: 0x04000759 RID: 1881
		internal ulong CurSeekPos;

		// Token: 0x0400075A RID: 1882
		internal ulong CurDevicePos;

		// Token: 0x0400075B RID: 1883
		internal uint dwAudioLevel;

		// Token: 0x0400075C RID: 1884
		internal uint dwReserved2;
	}
}
