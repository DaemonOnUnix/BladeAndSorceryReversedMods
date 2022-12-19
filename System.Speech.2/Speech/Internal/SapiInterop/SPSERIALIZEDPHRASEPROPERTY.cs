using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014C RID: 332
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASEPROPERTY
	{
		// Token: 0x040007F9 RID: 2041
		internal uint pszNameOffset;

		// Token: 0x040007FA RID: 2042
		internal uint ulId;

		// Token: 0x040007FB RID: 2043
		internal uint pszValueOffset;

		// Token: 0x040007FC RID: 2044
		internal ushort vValue;

		// Token: 0x040007FD RID: 2045
		internal ulong SpVariantSubset;

		// Token: 0x040007FE RID: 2046
		internal uint ulFirstElement;

		// Token: 0x040007FF RID: 2047
		internal uint ulCountOfElements;

		// Token: 0x04000800 RID: 2048
		internal uint pNextSiblingOffset;

		// Token: 0x04000801 RID: 2049
		internal uint pFirstChildOffset;

		// Token: 0x04000802 RID: 2050
		internal float SREngineConfidence;

		// Token: 0x04000803 RID: 2051
		internal sbyte Confidence;
	}
}
