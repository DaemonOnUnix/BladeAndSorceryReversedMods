using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000029 RID: 41
	[Flags]
	internal enum SPVESACTIONS
	{
		// Token: 0x04000236 RID: 566
		SPVES_CONTINUE = 0,
		// Token: 0x04000237 RID: 567
		SPVES_ABORT = 1,
		// Token: 0x04000238 RID: 568
		SPVES_SKIP = 2,
		// Token: 0x04000239 RID: 569
		SPVES_RATE = 4,
		// Token: 0x0400023A RID: 570
		SPVES_VOLUME = 8
	}
}
