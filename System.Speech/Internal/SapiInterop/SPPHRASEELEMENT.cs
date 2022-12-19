using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	[StructLayout(0)]
	internal class SPPHRASEELEMENT
	{
		// Token: 0x0400024F RID: 591
		internal uint ulAudioTimeOffset;

		// Token: 0x04000250 RID: 592
		internal uint ulAudioSizeTime;

		// Token: 0x04000251 RID: 593
		internal uint ulAudioStreamOffset;

		// Token: 0x04000252 RID: 594
		internal uint ulAudioSizeBytes;

		// Token: 0x04000253 RID: 595
		internal uint ulRetainedStreamOffset;

		// Token: 0x04000254 RID: 596
		internal uint ulRetainedSizeBytes;

		// Token: 0x04000255 RID: 597
		internal IntPtr pszDisplayText;

		// Token: 0x04000256 RID: 598
		internal IntPtr pszLexicalForm;

		// Token: 0x04000257 RID: 599
		internal IntPtr pszPronunciation;

		// Token: 0x04000258 RID: 600
		internal byte bDisplayAttributes;

		// Token: 0x04000259 RID: 601
		internal byte RequiredConfidence;

		// Token: 0x0400025A RID: 602
		internal byte ActualConfidence;

		// Token: 0x0400025B RID: 603
		internal byte Reserved;

		// Token: 0x0400025C RID: 604
		internal float SREngineConfidence;
	}
}
