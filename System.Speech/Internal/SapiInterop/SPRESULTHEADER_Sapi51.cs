using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	[StructLayout(0)]
	internal class SPRESULTHEADER_Sapi51
	{
		// Token: 0x040001FD RID: 509
		internal uint ulSerializedSize;

		// Token: 0x040001FE RID: 510
		internal uint cbHeaderSize;

		// Token: 0x040001FF RID: 511
		internal Guid clsidEngine;

		// Token: 0x04000200 RID: 512
		internal Guid clsidAlternates;

		// Token: 0x04000201 RID: 513
		internal uint ulStreamNum;

		// Token: 0x04000202 RID: 514
		internal ulong ullStreamPosStart;

		// Token: 0x04000203 RID: 515
		internal ulong ullStreamPosEnd;

		// Token: 0x04000204 RID: 516
		internal uint ulPhraseDataSize;

		// Token: 0x04000205 RID: 517
		internal uint ulPhraseOffset;

		// Token: 0x04000206 RID: 518
		internal uint ulPhraseAltDataSize;

		// Token: 0x04000207 RID: 519
		internal uint ulPhraseAltOffset;

		// Token: 0x04000208 RID: 520
		internal uint ulNumPhraseAlts;

		// Token: 0x04000209 RID: 521
		internal uint ulRetainedDataSize;

		// Token: 0x0400020A RID: 522
		internal uint ulRetainedOffset;

		// Token: 0x0400020B RID: 523
		internal uint ulDriverDataSize;

		// Token: 0x0400020C RID: 524
		internal uint ulDriverDataOffset;

		// Token: 0x0400020D RID: 525
		internal float fTimePerByte;

		// Token: 0x0400020E RID: 526
		internal float fInputScaleFactor;

		// Token: 0x0400020F RID: 527
		internal SPRECORESULTTIMES times;
	}
}
