using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000BB RID: 187
	internal struct WAVEHDR
	{
		// Token: 0x040004C8 RID: 1224
		internal IntPtr lpData;

		// Token: 0x040004C9 RID: 1225
		internal uint dwBufferLength;

		// Token: 0x040004CA RID: 1226
		internal uint dwBytesRecorded;

		// Token: 0x040004CB RID: 1227
		internal uint dwUser;

		// Token: 0x040004CC RID: 1228
		internal uint dwFlags;

		// Token: 0x040004CD RID: 1229
		internal uint dwLoops;

		// Token: 0x040004CE RID: 1230
		internal IntPtr lpNext;

		// Token: 0x040004CF RID: 1231
		internal uint reserved;
	}
}
