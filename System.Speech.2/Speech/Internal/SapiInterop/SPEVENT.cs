using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000106 RID: 262
	internal struct SPEVENT
	{
		// Token: 0x0400065A RID: 1626
		public SPEVENTENUM eEventId;

		// Token: 0x0400065B RID: 1627
		public SPEVENTLPARAMTYPE elParamType;

		// Token: 0x0400065C RID: 1628
		public uint ulStreamNum;

		// Token: 0x0400065D RID: 1629
		public ulong ullAudioStreamOffset;

		// Token: 0x0400065E RID: 1630
		public IntPtr wParam;

		// Token: 0x0400065F RID: 1631
		public IntPtr lParam;
	}
}
