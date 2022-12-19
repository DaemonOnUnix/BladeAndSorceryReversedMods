using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000066 RID: 102
	internal struct SPTEXTSELECTIONINFO
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00008F40 File Offset: 0x00007F40
		internal SPTEXTSELECTIONINFO(uint ulStartActiveOffset, uint cchActiveChars, uint ulStartSelection, uint cchSelection)
		{
			this.ulStartActiveOffset = ulStartActiveOffset;
			this.cchActiveChars = cchActiveChars;
			this.ulStartSelection = ulStartSelection;
			this.cchSelection = cchSelection;
		}

		// Token: 0x040001E0 RID: 480
		internal uint ulStartActiveOffset;

		// Token: 0x040001E1 RID: 481
		internal uint cchActiveChars;

		// Token: 0x040001E2 RID: 482
		internal uint ulStartSelection;

		// Token: 0x040001E3 RID: 483
		internal uint cchSelection;
	}
}
