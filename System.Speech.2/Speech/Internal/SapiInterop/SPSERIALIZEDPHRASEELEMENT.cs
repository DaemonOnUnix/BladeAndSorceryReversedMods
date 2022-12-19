using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200014B RID: 331
	[StructLayout(LayoutKind.Sequential)]
	internal class SPSERIALIZEDPHRASEELEMENT
	{
		// Token: 0x040007EB RID: 2027
		internal uint ulAudioTimeOffset;

		// Token: 0x040007EC RID: 2028
		internal uint ulAudioSizeTime;

		// Token: 0x040007ED RID: 2029
		internal uint ulAudioStreamOffset;

		// Token: 0x040007EE RID: 2030
		internal uint ulAudioSizeBytes;

		// Token: 0x040007EF RID: 2031
		internal uint ulRetainedStreamOffset;

		// Token: 0x040007F0 RID: 2032
		internal uint ulRetainedSizeBytes;

		// Token: 0x040007F1 RID: 2033
		internal uint pszDisplayTextOffset;

		// Token: 0x040007F2 RID: 2034
		internal uint pszLexicalFormOffset;

		// Token: 0x040007F3 RID: 2035
		internal uint pszPronunciationOffset;

		// Token: 0x040007F4 RID: 2036
		internal byte bDisplayAttributes;

		// Token: 0x040007F5 RID: 2037
		internal char RequiredConfidence;

		// Token: 0x040007F6 RID: 2038
		internal char ActualConfidence;

		// Token: 0x040007F7 RID: 2039
		internal byte Reserved;

		// Token: 0x040007F8 RID: 2040
		internal float SREngineConfidence;
	}
}
