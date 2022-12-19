using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000075 RID: 117
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASEPROPERTY
	{
		// Token: 0x04000287 RID: 647
		internal uint pszNameOffset;

		// Token: 0x04000288 RID: 648
		internal uint ulId;

		// Token: 0x04000289 RID: 649
		internal uint pszValueOffset;

		// Token: 0x0400028A RID: 650
		internal ushort vValue;

		// Token: 0x0400028B RID: 651
		internal ulong SpVariantSubset;

		// Token: 0x0400028C RID: 652
		internal uint ulFirstElement;

		// Token: 0x0400028D RID: 653
		internal uint ulCountOfElements;

		// Token: 0x0400028E RID: 654
		internal uint pNextSiblingOffset;

		// Token: 0x0400028F RID: 655
		internal uint pFirstChildOffset;

		// Token: 0x04000290 RID: 656
		internal float SREngineConfidence;

		// Token: 0x04000291 RID: 657
		internal sbyte Confidence;
	}
}
