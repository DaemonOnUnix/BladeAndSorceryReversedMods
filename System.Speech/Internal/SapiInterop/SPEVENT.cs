using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002C RID: 44
	internal struct SPEVENT
	{
		// Token: 0x040000DE RID: 222
		public SPEVENTENUM eEventId;

		// Token: 0x040000DF RID: 223
		public SPEVENTLPARAMTYPE elParamType;

		// Token: 0x040000E0 RID: 224
		public uint ulStreamNum;

		// Token: 0x040000E1 RID: 225
		public ulong ullAudioStreamOffset;

		// Token: 0x040000E2 RID: 226
		public IntPtr wParam;

		// Token: 0x040000E3 RID: 227
		public IntPtr lParam;
	}
}
