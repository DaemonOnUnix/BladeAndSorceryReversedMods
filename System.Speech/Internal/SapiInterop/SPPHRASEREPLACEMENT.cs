using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000076 RID: 118
	[StructLayout(0)]
	internal class SPPHRASEREPLACEMENT
	{
		// Token: 0x04000292 RID: 658
		internal byte bDisplayAttributes;

		// Token: 0x04000293 RID: 659
		internal uint pszReplacementText;

		// Token: 0x04000294 RID: 660
		internal uint ulFirstElement;

		// Token: 0x04000295 RID: 661
		internal uint ulCountOfElements;
	}
}
