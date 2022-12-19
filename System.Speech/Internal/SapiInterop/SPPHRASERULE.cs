using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	[StructLayout(0)]
	internal class SPPHRASERULE
	{
		// Token: 0x04000247 RID: 583
		[MarshalAs(21)]
		internal string pszName;

		// Token: 0x04000248 RID: 584
		internal uint ulId;

		// Token: 0x04000249 RID: 585
		internal uint ulFirstElement;

		// Token: 0x0400024A RID: 586
		internal uint ulCountOfElements;

		// Token: 0x0400024B RID: 587
		internal IntPtr pNextSibling;

		// Token: 0x0400024C RID: 588
		internal IntPtr pFirstChild;

		// Token: 0x0400024D RID: 589
		internal float SREngineConfidence;

		// Token: 0x0400024E RID: 590
		internal byte Confidence;
	}
}
