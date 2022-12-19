using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000158 RID: 344
	[StructLayout(0)]
	internal class SPVTEXTFRAG
	{
		// Token: 0x04000688 RID: 1672
		public IntPtr pNext;

		// Token: 0x04000689 RID: 1673
		public SPVSTATE State;

		// Token: 0x0400068A RID: 1674
		public IntPtr pTextStart;

		// Token: 0x0400068B RID: 1675
		public int ulTextLen;

		// Token: 0x0400068C RID: 1676
		public int ulTextSrcOffset;

		// Token: 0x0400068D RID: 1677
		public GCHandle gcText;

		// Token: 0x0400068E RID: 1678
		public GCHandle gcNext;

		// Token: 0x0400068F RID: 1679
		public GCHandle gcPhoneme;

		// Token: 0x04000690 RID: 1680
		public GCHandle gcSayAsCategory;
	}
}
