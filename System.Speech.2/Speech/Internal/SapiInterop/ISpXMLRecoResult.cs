using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015E RID: 350
	[Guid("AE39362B-45A8-4074-9B9E-CCF49AA2D0B6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpXMLRecoResult : ISpRecoResult, ISpPhrase
	{
		// Token: 0x06000A9D RID: 2717
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A9E RID: 2718
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A9F RID: 2719
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000AA0 RID: 2720
		void Discard(uint dwValueTypes);

		// Token: 0x06000AA1 RID: 2721
		void Slot5();

		// Token: 0x06000AA2 RID: 2722
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x06000AA3 RID: 2723
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x06000AA4 RID: 2724
		void Slot8();

		// Token: 0x06000AA5 RID: 2725
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x06000AA6 RID: 2726
		void Slot10();

		// Token: 0x06000AA7 RID: 2727
		void Slot11();

		// Token: 0x06000AA8 RID: 2728
		void GetXMLResult([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemXMLResult, SPXMLRESULTOPTIONS Options);

		// Token: 0x06000AA9 RID: 2729
		void GetXMLErrorInfo(out SPSEMANTICERRORINFO pSemanticErrorInfo);
	}
}
