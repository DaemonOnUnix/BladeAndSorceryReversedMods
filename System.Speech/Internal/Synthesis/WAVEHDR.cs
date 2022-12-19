using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000EF RID: 239
	internal struct WAVEHDR
	{
		// Token: 0x04000449 RID: 1097
		internal IntPtr lpData;

		// Token: 0x0400044A RID: 1098
		internal uint dwBufferLength;

		// Token: 0x0400044B RID: 1099
		internal uint dwBytesRecorded;

		// Token: 0x0400044C RID: 1100
		internal uint dwUser;

		// Token: 0x0400044D RID: 1101
		internal uint dwFlags;

		// Token: 0x0400044E RID: 1102
		internal uint dwLoops;

		// Token: 0x0400044F RID: 1103
		internal IntPtr lpNext;

		// Token: 0x04000450 RID: 1104
		internal uint reserved;
	}
}
