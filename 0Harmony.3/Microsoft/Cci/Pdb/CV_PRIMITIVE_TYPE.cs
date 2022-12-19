using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000249 RID: 585
	internal struct CV_PRIMITIVE_TYPE
	{
		// Token: 0x04000A99 RID: 2713
		private const uint CV_MMASK = 1792U;

		// Token: 0x04000A9A RID: 2714
		private const uint CV_TMASK = 240U;

		// Token: 0x04000A9B RID: 2715
		private const uint CV_SMASK = 15U;

		// Token: 0x04000A9C RID: 2716
		private const int CV_MSHIFT = 8;

		// Token: 0x04000A9D RID: 2717
		private const int CV_TSHIFT = 4;

		// Token: 0x04000A9E RID: 2718
		private const int CV_SSHIFT = 0;

		// Token: 0x04000A9F RID: 2719
		private const uint CV_FIRST_NONPRIM = 4096U;
	}
}
