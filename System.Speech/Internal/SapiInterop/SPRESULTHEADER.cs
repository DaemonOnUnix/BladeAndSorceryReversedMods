using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	[StructLayout(0)]
	internal class SPRESULTHEADER
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00008F6F File Offset: 0x00007F6F
		internal SPRESULTHEADER()
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008F78 File Offset: 0x00007F78
		internal SPRESULTHEADER(SPRESULTHEADER_Sapi51 source)
		{
			this.ulSerializedSize = source.ulSerializedSize;
			this.cbHeaderSize = source.cbHeaderSize;
			this.clsidEngine = source.clsidEngine;
			this.clsidAlternates = source.clsidAlternates;
			this.ulStreamNum = source.ulStreamNum;
			this.ullStreamPosStart = source.ullStreamPosStart;
			this.ullStreamPosEnd = source.ullStreamPosEnd;
			this.ulPhraseDataSize = source.ulPhraseDataSize;
			this.ulPhraseOffset = source.ulPhraseOffset;
			this.ulPhraseAltDataSize = source.ulPhraseAltDataSize;
			this.ulPhraseAltOffset = source.ulPhraseAltOffset;
			this.ulNumPhraseAlts = source.ulNumPhraseAlts;
			this.ulRetainedDataSize = source.ulRetainedDataSize;
			this.ulRetainedOffset = source.ulRetainedOffset;
			this.ulDriverDataSize = source.ulDriverDataSize;
			this.ulDriverDataOffset = source.ulDriverDataOffset;
			this.fTimePerByte = source.fTimePerByte;
			this.fInputScaleFactor = source.fInputScaleFactor;
			this.times = source.times;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009070 File Offset: 0x00008070
		internal void Validate()
		{
			this.ValidateOffsetAndLength(0U, this.cbHeaderSize);
			this.ValidateOffsetAndLength(this.ulPhraseOffset, this.ulPhraseDataSize);
			this.ValidateOffsetAndLength(this.ulPhraseAltOffset, this.ulPhraseAltDataSize);
			this.ValidateOffsetAndLength(this.ulRetainedOffset, this.ulRetainedDataSize);
			this.ValidateOffsetAndLength(this.ulDriverDataOffset, this.ulDriverDataSize);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000090D2 File Offset: 0x000080D2
		private void ValidateOffsetAndLength(uint offset, uint length)
		{
			if (offset + length > this.ulSerializedSize)
			{
				throw new FormatException(SR.Get(SRID.ResultInvalidFormat, new object[0]));
			}
		}

		// Token: 0x04000210 RID: 528
		internal uint ulSerializedSize;

		// Token: 0x04000211 RID: 529
		internal uint cbHeaderSize;

		// Token: 0x04000212 RID: 530
		internal Guid clsidEngine;

		// Token: 0x04000213 RID: 531
		internal Guid clsidAlternates;

		// Token: 0x04000214 RID: 532
		internal uint ulStreamNum;

		// Token: 0x04000215 RID: 533
		internal ulong ullStreamPosStart;

		// Token: 0x04000216 RID: 534
		internal ulong ullStreamPosEnd;

		// Token: 0x04000217 RID: 535
		internal uint ulPhraseDataSize;

		// Token: 0x04000218 RID: 536
		internal uint ulPhraseOffset;

		// Token: 0x04000219 RID: 537
		internal uint ulPhraseAltDataSize;

		// Token: 0x0400021A RID: 538
		internal uint ulPhraseAltOffset;

		// Token: 0x0400021B RID: 539
		internal uint ulNumPhraseAlts;

		// Token: 0x0400021C RID: 540
		internal uint ulRetainedDataSize;

		// Token: 0x0400021D RID: 541
		internal uint ulRetainedOffset;

		// Token: 0x0400021E RID: 542
		internal uint ulDriverDataSize;

		// Token: 0x0400021F RID: 543
		internal uint ulDriverDataOffset;

		// Token: 0x04000220 RID: 544
		internal float fTimePerByte;

		// Token: 0x04000221 RID: 545
		internal float fInputScaleFactor;

		// Token: 0x04000222 RID: 546
		internal SPRECORESULTTIMES times;

		// Token: 0x04000223 RID: 547
		internal uint fAlphabet;
	}
}
