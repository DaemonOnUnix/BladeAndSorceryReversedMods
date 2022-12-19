using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000143 RID: 323
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPRESULTHEADER_Sapi51
	{
		// Token: 0x0400076F RID: 1903
		internal uint ulSerializedSize;

		// Token: 0x04000770 RID: 1904
		internal uint cbHeaderSize;

		// Token: 0x04000771 RID: 1905
		internal Guid clsidEngine;

		// Token: 0x04000772 RID: 1906
		internal Guid clsidAlternates;

		// Token: 0x04000773 RID: 1907
		internal uint ulStreamNum;

		// Token: 0x04000774 RID: 1908
		internal ulong ullStreamPosStart;

		// Token: 0x04000775 RID: 1909
		internal ulong ullStreamPosEnd;

		// Token: 0x04000776 RID: 1910
		internal uint ulPhraseDataSize;

		// Token: 0x04000777 RID: 1911
		internal uint ulPhraseOffset;

		// Token: 0x04000778 RID: 1912
		internal uint ulPhraseAltDataSize;

		// Token: 0x04000779 RID: 1913
		internal uint ulPhraseAltOffset;

		// Token: 0x0400077A RID: 1914
		internal uint ulNumPhraseAlts;

		// Token: 0x0400077B RID: 1915
		internal uint ulRetainedDataSize;

		// Token: 0x0400077C RID: 1916
		internal uint ulRetainedOffset;

		// Token: 0x0400077D RID: 1917
		internal uint ulDriverDataSize;

		// Token: 0x0400077E RID: 1918
		internal uint ulDriverDataOffset;

		// Token: 0x0400077F RID: 1919
		internal float fTimePerByte;

		// Token: 0x04000780 RID: 1920
		internal float fInputScaleFactor;

		// Token: 0x04000781 RID: 1921
		internal SPRECORESULTTIMES times;
	}
}
