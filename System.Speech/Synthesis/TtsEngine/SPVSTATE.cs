using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200015A RID: 346
	[ComConversionLoss]
	[TypeLibType(16)]
	internal struct SPVSTATE
	{
		// Token: 0x04000693 RID: 1683
		public SPVACTIONS eAction;

		// Token: 0x04000694 RID: 1684
		public short LangID;

		// Token: 0x04000695 RID: 1685
		public short wReserved;

		// Token: 0x04000696 RID: 1686
		public int EmphAdj;

		// Token: 0x04000697 RID: 1687
		public int RateAdj;

		// Token: 0x04000698 RID: 1688
		public int Volume;

		// Token: 0x04000699 RID: 1689
		public SPVPITCH PitchAdj;

		// Token: 0x0400069A RID: 1690
		public int SilenceMSecs;

		// Token: 0x0400069B RID: 1691
		public IntPtr pPhoneIds;

		// Token: 0x0400069C RID: 1692
		public SPPARTOFSPEECH ePartOfSpeech;

		// Token: 0x0400069D RID: 1693
		public SPVCONTEXT Context;
	}
}
