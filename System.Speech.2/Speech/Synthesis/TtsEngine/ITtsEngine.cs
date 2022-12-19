using System;
using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000021 RID: 33
	[Guid("A74D7C8E-4CC5-4F2F-A6EB-804DEE18500E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITtsEngine
	{
		// Token: 0x060000BD RID: 189
		[PreserveSig]
		void Speak(SPEAKFLAGS dwSpeakFlags, ref Guid rguidFormatId, IntPtr pWaveFormatEx, IntPtr pTextFragList, IntPtr pOutputSite);

		// Token: 0x060000BE RID: 190
		[PreserveSig]
		void GetOutputFormat(ref Guid pTargetFmtId, IntPtr pTargetWaveFormatEx, out Guid pOutputFormatId, out IntPtr ppCoMemOutputWaveFormatEx);
	}
}
