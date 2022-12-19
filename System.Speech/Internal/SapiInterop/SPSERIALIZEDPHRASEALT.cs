using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000063 RID: 99
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASEALT
	{
		// Token: 0x040001D6 RID: 470
		internal uint ulStartElementInParent;

		// Token: 0x040001D7 RID: 471
		internal uint cElementsInParent;

		// Token: 0x040001D8 RID: 472
		internal uint cElementsInAlternate;

		// Token: 0x040001D9 RID: 473
		internal uint cbAltExtra;
	}
}
