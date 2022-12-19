using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000074 RID: 116
	[StructLayout(0)]
	internal class SPSERIALIZEDPHRASEELEMENT
	{
		// Token: 0x04000279 RID: 633
		internal uint ulAudioTimeOffset;

		// Token: 0x0400027A RID: 634
		internal uint ulAudioSizeTime;

		// Token: 0x0400027B RID: 635
		internal uint ulAudioStreamOffset;

		// Token: 0x0400027C RID: 636
		internal uint ulAudioSizeBytes;

		// Token: 0x0400027D RID: 637
		internal uint ulRetainedStreamOffset;

		// Token: 0x0400027E RID: 638
		internal uint ulRetainedSizeBytes;

		// Token: 0x0400027F RID: 639
		internal uint pszDisplayTextOffset;

		// Token: 0x04000280 RID: 640
		internal uint pszLexicalFormOffset;

		// Token: 0x04000281 RID: 641
		internal uint pszPronunciationOffset;

		// Token: 0x04000282 RID: 642
		internal byte bDisplayAttributes;

		// Token: 0x04000283 RID: 643
		internal char RequiredConfidence;

		// Token: 0x04000284 RID: 644
		internal char ActualConfidence;

		// Token: 0x04000285 RID: 645
		internal byte Reserved;

		// Token: 0x04000286 RID: 646
		internal float SREngineConfidence;
	}
}
