using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200013D RID: 317
	internal struct SPTEXTSELECTIONINFO
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x0002B478 File Offset: 0x00029678
		internal SPTEXTSELECTIONINFO(uint ulStartActiveOffset, uint cchActiveChars, uint ulStartSelection, uint cchSelection)
		{
			this.ulStartActiveOffset = ulStartActiveOffset;
			this.cchActiveChars = cchActiveChars;
			this.ulStartSelection = ulStartSelection;
			this.cchSelection = cchSelection;
		}

		// Token: 0x04000752 RID: 1874
		internal uint ulStartActiveOffset;

		// Token: 0x04000753 RID: 1875
		internal uint cchActiveChars;

		// Token: 0x04000754 RID: 1876
		internal uint ulStartSelection;

		// Token: 0x04000755 RID: 1877
		internal uint cchSelection;
	}
}
