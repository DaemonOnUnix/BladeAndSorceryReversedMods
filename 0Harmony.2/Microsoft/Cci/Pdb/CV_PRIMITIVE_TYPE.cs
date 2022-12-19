using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200033F RID: 831
	internal struct CV_PRIMITIVE_TYPE
	{
		// Token: 0x04000AD8 RID: 2776
		private const uint CV_MMASK = 1792U;

		// Token: 0x04000AD9 RID: 2777
		private const uint CV_TMASK = 240U;

		// Token: 0x04000ADA RID: 2778
		private const uint CV_SMASK = 15U;

		// Token: 0x04000ADB RID: 2779
		private const int CV_MSHIFT = 8;

		// Token: 0x04000ADC RID: 2780
		private const int CV_TSHIFT = 4;

		// Token: 0x04000ADD RID: 2781
		private const int CV_SSHIFT = 0;

		// Token: 0x04000ADE RID: 2782
		private const uint CV_FIRST_NONPRIM = 4096U;
	}
}
