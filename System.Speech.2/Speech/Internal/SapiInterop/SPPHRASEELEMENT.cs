using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000148 RID: 328
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class SPPHRASEELEMENT
	{
		// Token: 0x040007C1 RID: 1985
		internal uint ulAudioTimeOffset;

		// Token: 0x040007C2 RID: 1986
		internal uint ulAudioSizeTime;

		// Token: 0x040007C3 RID: 1987
		internal uint ulAudioStreamOffset;

		// Token: 0x040007C4 RID: 1988
		internal uint ulAudioSizeBytes;

		// Token: 0x040007C5 RID: 1989
		internal uint ulRetainedStreamOffset;

		// Token: 0x040007C6 RID: 1990
		internal uint ulRetainedSizeBytes;

		// Token: 0x040007C7 RID: 1991
		internal IntPtr pszDisplayText;

		// Token: 0x040007C8 RID: 1992
		internal IntPtr pszLexicalForm;

		// Token: 0x040007C9 RID: 1993
		internal IntPtr pszPronunciation;

		// Token: 0x040007CA RID: 1994
		internal byte bDisplayAttributes;

		// Token: 0x040007CB RID: 1995
		internal byte RequiredConfidence;

		// Token: 0x040007CC RID: 1996
		internal byte ActualConfidence;

		// Token: 0x040007CD RID: 1997
		internal byte Reserved;

		// Token: 0x040007CE RID: 1998
		internal float SREngineConfidence;
	}
}
