using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000078 RID: 120
	[StructLayout(0)]
	internal class SPWAVEFORMATEX
	{
		// Token: 0x040002A3 RID: 675
		public uint cbUsed;

		// Token: 0x040002A4 RID: 676
		public Guid Guid;

		// Token: 0x040002A5 RID: 677
		public ushort wFormatTag;

		// Token: 0x040002A6 RID: 678
		public ushort nChannels;

		// Token: 0x040002A7 RID: 679
		public uint nSamplesPerSec;

		// Token: 0x040002A8 RID: 680
		public uint nAvgBytesPerSec;

		// Token: 0x040002A9 RID: 681
		public ushort nBlockAlign;

		// Token: 0x040002AA RID: 682
		public ushort wBitsPerSample;

		// Token: 0x040002AB RID: 683
		public ushort cbSize;
	}
}
