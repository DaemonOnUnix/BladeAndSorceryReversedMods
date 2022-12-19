using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000022 RID: 34
	[StructLayout(LayoutKind.Sequential)]
	internal class SPVTEXTFRAG
	{
		// Token: 0x0400020E RID: 526
		public IntPtr pNext;

		// Token: 0x0400020F RID: 527
		public SPVSTATE State;

		// Token: 0x04000210 RID: 528
		public IntPtr pTextStart;

		// Token: 0x04000211 RID: 529
		public int ulTextLen;

		// Token: 0x04000212 RID: 530
		public int ulTextSrcOffset;

		// Token: 0x04000213 RID: 531
		public GCHandle gcText;

		// Token: 0x04000214 RID: 532
		public GCHandle gcNext;

		// Token: 0x04000215 RID: 533
		public GCHandle gcPhoneme;

		// Token: 0x04000216 RID: 534
		public GCHandle gcSayAsCategory;
	}
}
