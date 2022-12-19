using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002A RID: 42
	[TypeLibType(16)]
	internal enum SPVACTIONS
	{
		// Token: 0x0400023C RID: 572
		SPVA_Speak,
		// Token: 0x0400023D RID: 573
		SPVA_Silence,
		// Token: 0x0400023E RID: 574
		SPVA_Pronounce,
		// Token: 0x0400023F RID: 575
		SPVA_Bookmark,
		// Token: 0x04000240 RID: 576
		SPVA_SpellOut,
		// Token: 0x04000241 RID: 577
		SPVA_Section,
		// Token: 0x04000242 RID: 578
		SPVA_ParseUnknownTag
	}
}
