using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000160 RID: 352
	[TypeLibType(16)]
	internal enum SPVACTIONS
	{
		// Token: 0x040006B6 RID: 1718
		SPVA_Speak,
		// Token: 0x040006B7 RID: 1719
		SPVA_Silence,
		// Token: 0x040006B8 RID: 1720
		SPVA_Pronounce,
		// Token: 0x040006B9 RID: 1721
		SPVA_Bookmark,
		// Token: 0x040006BA RID: 1722
		SPVA_SpellOut,
		// Token: 0x040006BB RID: 1723
		SPVA_Section,
		// Token: 0x040006BC RID: 1724
		SPVA_ParseUnknownTag
	}
}
