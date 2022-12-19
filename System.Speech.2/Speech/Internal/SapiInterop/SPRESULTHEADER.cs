using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000144 RID: 324
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPRESULTHEADER
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal SPRESULTHEADER()
		{
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0002B498 File Offset: 0x00029698
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

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002B590 File Offset: 0x00029790
		internal void Validate()
		{
			this.ValidateOffsetAndLength(0U, this.cbHeaderSize);
			this.ValidateOffsetAndLength(this.ulPhraseOffset, this.ulPhraseDataSize);
			this.ValidateOffsetAndLength(this.ulPhraseAltOffset, this.ulPhraseAltDataSize);
			this.ValidateOffsetAndLength(this.ulRetainedOffset, this.ulRetainedDataSize);
			this.ValidateOffsetAndLength(this.ulDriverDataOffset, this.ulDriverDataSize);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002B5F2 File Offset: 0x000297F2
		private void ValidateOffsetAndLength(uint offset, uint length)
		{
			if (offset + length > this.ulSerializedSize)
			{
				throw new FormatException(SR.Get(SRID.ResultInvalidFormat, new object[0]));
			}
		}

		// Token: 0x04000782 RID: 1922
		internal uint ulSerializedSize;

		// Token: 0x04000783 RID: 1923
		internal uint cbHeaderSize;

		// Token: 0x04000784 RID: 1924
		internal Guid clsidEngine;

		// Token: 0x04000785 RID: 1925
		internal Guid clsidAlternates;

		// Token: 0x04000786 RID: 1926
		internal uint ulStreamNum;

		// Token: 0x04000787 RID: 1927
		internal ulong ullStreamPosStart;

		// Token: 0x04000788 RID: 1928
		internal ulong ullStreamPosEnd;

		// Token: 0x04000789 RID: 1929
		internal uint ulPhraseDataSize;

		// Token: 0x0400078A RID: 1930
		internal uint ulPhraseOffset;

		// Token: 0x0400078B RID: 1931
		internal uint ulPhraseAltDataSize;

		// Token: 0x0400078C RID: 1932
		internal uint ulPhraseAltOffset;

		// Token: 0x0400078D RID: 1933
		internal uint ulNumPhraseAlts;

		// Token: 0x0400078E RID: 1934
		internal uint ulRetainedDataSize;

		// Token: 0x0400078F RID: 1935
		internal uint ulRetainedOffset;

		// Token: 0x04000790 RID: 1936
		internal uint ulDriverDataSize;

		// Token: 0x04000791 RID: 1937
		internal uint ulDriverDataOffset;

		// Token: 0x04000792 RID: 1938
		internal float fTimePerByte;

		// Token: 0x04000793 RID: 1939
		internal float fInputScaleFactor;

		// Token: 0x04000794 RID: 1940
		internal SPRECORESULTTIMES times;

		// Token: 0x04000795 RID: 1941
		internal uint fAlphabet;
	}
}
