using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000024 RID: 36
	[ComConversionLoss]
	[TypeLibType(16)]
	internal struct SPVSTATE
	{
		// Token: 0x04000219 RID: 537
		public SPVACTIONS eAction;

		// Token: 0x0400021A RID: 538
		public short LangID;

		// Token: 0x0400021B RID: 539
		public short wReserved;

		// Token: 0x0400021C RID: 540
		public int EmphAdj;

		// Token: 0x0400021D RID: 541
		public int RateAdj;

		// Token: 0x0400021E RID: 542
		public int Volume;

		// Token: 0x0400021F RID: 543
		public SPVPITCH PitchAdj;

		// Token: 0x04000220 RID: 544
		public int SilenceMSecs;

		// Token: 0x04000221 RID: 545
		public IntPtr pPhoneIds;

		// Token: 0x04000222 RID: 546
		public SPPARTOFSPEECH ePartOfSpeech;

		// Token: 0x04000223 RID: 547
		public SPVCONTEXT Context;
	}
}
