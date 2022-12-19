using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000067 RID: 103
	internal struct SPAUDIOSTATUS
	{
		// Token: 0x040001E4 RID: 484
		internal int cbFreeBuffSpace;

		// Token: 0x040001E5 RID: 485
		internal uint cbNonBlockingIO;

		// Token: 0x040001E6 RID: 486
		internal SPAUDIOSTATE State;

		// Token: 0x040001E7 RID: 487
		internal ulong CurSeekPos;

		// Token: 0x040001E8 RID: 488
		internal ulong CurDevicePos;

		// Token: 0x040001E9 RID: 489
		internal uint dwAudioLevel;

		// Token: 0x040001EA RID: 490
		internal uint dwReserved2;
	}
}
