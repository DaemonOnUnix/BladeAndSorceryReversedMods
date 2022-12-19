using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000157 RID: 343
	[InterfaceType(1)]
	[Guid("A74D7C8E-4CC5-4F2F-A6EB-804DEE18500E")]
	[ComImport]
	internal interface ITtsEngine
	{
		// Token: 0x06000907 RID: 2311
		[PreserveSig]
		int Speak(SPEAKFLAGS dwSpeakFlags, ref Guid rguidFormatId, IntPtr pWaveFormatEx, IntPtr pTextFragList, IntPtr pOutputSite);

		// Token: 0x06000908 RID: 2312
		[PreserveSig]
		int GetOutputFormat(ref Guid pTargetFmtId, IntPtr pTargetWaveFormatEx, out Guid pOutputFormatId, out IntPtr ppCoMemOutputWaveFormatEx);
	}
}
