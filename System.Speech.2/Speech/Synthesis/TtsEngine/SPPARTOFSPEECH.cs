using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002B RID: 43
	[TypeLibType(16)]
	internal enum SPPARTOFSPEECH
	{
		// Token: 0x04000244 RID: 580
		SPPS_NotOverriden = -1,
		// Token: 0x04000245 RID: 581
		SPPS_Unknown,
		// Token: 0x04000246 RID: 582
		SPPS_Noun = 4096,
		// Token: 0x04000247 RID: 583
		SPPS_Verb = 8192,
		// Token: 0x04000248 RID: 584
		SPPS_Modifier = 12288,
		// Token: 0x04000249 RID: 585
		SPPS_Function = 16384,
		// Token: 0x0400024A RID: 586
		SPPS_Interjection = 20480,
		// Token: 0x0400024B RID: 587
		SPPS_SuppressWord = 61440
	}
}
