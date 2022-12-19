using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000161 RID: 353
	[TypeLibType(16)]
	internal enum SPPARTOFSPEECH
	{
		// Token: 0x040006BE RID: 1726
		SPPS_NotOverriden = -1,
		// Token: 0x040006BF RID: 1727
		SPPS_Unknown,
		// Token: 0x040006C0 RID: 1728
		SPPS_Noun = 4096,
		// Token: 0x040006C1 RID: 1729
		SPPS_Verb = 8192,
		// Token: 0x040006C2 RID: 1730
		SPPS_Modifier = 12288,
		// Token: 0x040006C3 RID: 1731
		SPPS_Function = 16384,
		// Token: 0x040006C4 RID: 1732
		SPPS_Interjection = 20480,
		// Token: 0x040006C5 RID: 1733
		SPPS_SuppressWord = 61440
	}
}
