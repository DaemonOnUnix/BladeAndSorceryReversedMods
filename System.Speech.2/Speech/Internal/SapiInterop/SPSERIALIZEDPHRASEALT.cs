using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200013A RID: 314
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASEALT
	{
		// Token: 0x04000748 RID: 1864
		internal uint ulStartElementInParent;

		// Token: 0x04000749 RID: 1865
		internal uint cElementsInParent;

		// Token: 0x0400074A RID: 1866
		internal uint cElementsInAlternate;

		// Token: 0x0400074B RID: 1867
		internal uint cbAltExtra;
	}
}
