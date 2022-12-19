using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200015F RID: 351
	[Flags]
	internal enum SPVESACTIONS
	{
		// Token: 0x040006B0 RID: 1712
		SPVES_CONTINUE = 0,
		// Token: 0x040006B1 RID: 1713
		SPVES_ABORT = 1,
		// Token: 0x040006B2 RID: 1714
		SPVES_SKIP = 2,
		// Token: 0x040006B3 RID: 1715
		SPVES_RATE = 4,
		// Token: 0x040006B4 RID: 1716
		SPVES_VOLUME = 8
	}
}
