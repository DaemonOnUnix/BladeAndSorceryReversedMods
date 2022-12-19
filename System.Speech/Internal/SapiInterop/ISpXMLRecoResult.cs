using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000087 RID: 135
	[InterfaceType(1)]
	[Guid("AE39362B-45A8-4074-9B9E-CCF49AA2D0B6")]
	[ComImport]
	internal interface ISpXMLRecoResult : ISpRecoResult, ISpPhrase
	{
		// Token: 0x0600029B RID: 667
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600029C RID: 668
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600029D RID: 669
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x0600029E RID: 670
		void Discard(uint dwValueTypes);

		// Token: 0x0600029F RID: 671
		void Slot5();

		// Token: 0x060002A0 RID: 672
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(42, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x060002A1 RID: 673
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x060002A2 RID: 674
		void Slot8();

		// Token: 0x060002A3 RID: 675
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x060002A4 RID: 676
		void Slot10();

		// Token: 0x060002A5 RID: 677
		void Slot11();

		// Token: 0x060002A6 RID: 678
		void GetXMLResult([MarshalAs(21)] out string ppszCoMemXMLResult, SPXMLRESULTOPTIONS Options);

		// Token: 0x060002A7 RID: 679
		void GetXMLErrorInfo(out SPSEMANTICERRORINFO pSemanticErrorInfo);
	}
}
