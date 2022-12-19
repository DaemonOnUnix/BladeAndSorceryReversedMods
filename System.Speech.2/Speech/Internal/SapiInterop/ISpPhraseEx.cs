using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015F RID: 351
	[Guid("F264DA52-E457-4696-B856-A737B717AF79")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpPhraseEx : ISpPhrase
	{
		// Token: 0x06000AAA RID: 2730
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000AAB RID: 2731
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000AAC RID: 2732
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000AAD RID: 2733
		void Discard(uint dwValueTypes);

		// Token: 0x06000AAE RID: 2734
		void GetXMLResult([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemXMLResult, SPXMLRESULTOPTIONS Options);

		// Token: 0x06000AAF RID: 2735
		void GetXMLErrorInfo(out SPSEMANTICERRORINFO pSemanticErrorInfo);

		// Token: 0x06000AB0 RID: 2736
		void Slot7();
	}
}
