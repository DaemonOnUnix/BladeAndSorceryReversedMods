using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000107 RID: 263
	internal struct SPEVENTEX
	{
		// Token: 0x04000660 RID: 1632
		public SPEVENTENUM eEventId;

		// Token: 0x04000661 RID: 1633
		public SPEVENTLPARAMTYPE elParamType;

		// Token: 0x04000662 RID: 1634
		public uint ulStreamNum;

		// Token: 0x04000663 RID: 1635
		public ulong ullAudioStreamOffset;

		// Token: 0x04000664 RID: 1636
		public IntPtr wParam;

		// Token: 0x04000665 RID: 1637
		public IntPtr lParam;

		// Token: 0x04000666 RID: 1638
		public ulong ullAudioTimeOffset;
	}
}
