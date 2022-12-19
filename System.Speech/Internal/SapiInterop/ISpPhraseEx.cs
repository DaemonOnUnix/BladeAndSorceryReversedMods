using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000088 RID: 136
	[Guid("F264DA52-E457-4696-B856-A737B717AF79")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpPhraseEx : ISpPhrase
	{
		// Token: 0x060002A8 RID: 680
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x060002A9 RID: 681
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x060002AA RID: 682
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x060002AB RID: 683
		void Discard(uint dwValueTypes);

		// Token: 0x060002AC RID: 684
		void GetXMLResult([MarshalAs(21)] out string ppszCoMemXMLResult, SPXMLRESULTOPTIONS Options);

		// Token: 0x060002AD RID: 685
		void GetXMLErrorInfo(out SPSEMANTICERRORINFO pSemanticErrorInfo);

		// Token: 0x060002AE RID: 686
		void Slot7();
	}
}
