using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014F RID: 335
	[StructLayout(LayoutKind.Sequential)]
	internal class SPWAVEFORMATEX
	{
		// Token: 0x04000815 RID: 2069
		public uint cbUsed;

		// Token: 0x04000816 RID: 2070
		public Guid Guid;

		// Token: 0x04000817 RID: 2071
		public ushort wFormatTag;

		// Token: 0x04000818 RID: 2072
		public ushort nChannels;

		// Token: 0x04000819 RID: 2073
		public uint nSamplesPerSec;

		// Token: 0x0400081A RID: 2074
		public uint nAvgBytesPerSec;

		// Token: 0x0400081B RID: 2075
		public ushort nBlockAlign;

		// Token: 0x0400081C RID: 2076
		public ushort wBitsPerSample;

		// Token: 0x0400081D RID: 2077
		public ushort cbSize;
	}
}
