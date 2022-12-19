using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014D RID: 333
	[StructLayout(LayoutKind.Sequential)]
	internal class SPPHRASEREPLACEMENT
	{
		// Token: 0x04000804 RID: 2052
		internal byte bDisplayAttributes;

		// Token: 0x04000805 RID: 2053
		internal uint pszReplacementText;

		// Token: 0x04000806 RID: 2054
		internal uint ulFirstElement;

		// Token: 0x04000807 RID: 2055
		internal uint ulCountOfElements;
	}
}
