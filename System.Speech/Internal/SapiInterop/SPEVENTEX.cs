using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002D RID: 45
	internal struct SPEVENTEX
	{
		// Token: 0x040000E4 RID: 228
		public SPEVENTENUM eEventId;

		// Token: 0x040000E5 RID: 229
		public SPEVENTLPARAMTYPE elParamType;

		// Token: 0x040000E6 RID: 230
		public uint ulStreamNum;

		// Token: 0x040000E7 RID: 231
		public ulong ullAudioStreamOffset;

		// Token: 0x040000E8 RID: 232
		public IntPtr wParam;

		// Token: 0x040000E9 RID: 233
		public IntPtr lParam;

		// Token: 0x040000EA RID: 234
		public ulong ullAudioTimeOffset;
	}
}
