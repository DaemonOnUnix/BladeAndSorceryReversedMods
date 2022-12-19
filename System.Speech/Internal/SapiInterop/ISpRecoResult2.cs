using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000086 RID: 134
	[Guid("27CAC6C4-88F2-41f2-8817-0C95E59F1E6E")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRecoResult2 : ISpRecoResult, ISpPhrase
	{
		// Token: 0x0600028D RID: 653
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600028E RID: 654
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600028F RID: 655
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000290 RID: 656
		void Discard(uint dwValueTypes);

		// Token: 0x06000291 RID: 657
		void Slot5();

		// Token: 0x06000292 RID: 658
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(42, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x06000293 RID: 659
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x06000294 RID: 660
		void Slot8();

		// Token: 0x06000295 RID: 661
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x06000296 RID: 662
		void Slot10();

		// Token: 0x06000297 RID: 663
		void Slot11();

		// Token: 0x06000298 RID: 664
		void CommitAlternate(ISpPhraseAlt pPhraseAlt, out ISpRecoResult ppNewResult);

		// Token: 0x06000299 RID: 665
		void CommitText(uint ulStartElement, uint ulCountOfElements, [MarshalAs(21)] string pszCorrectedData, SPCOMMITFLAGS commitFlags);

		// Token: 0x0600029A RID: 666
		void SetTextFeedback([MarshalAs(21)] string pszFeedback, [MarshalAs(2)] bool fSuccessful);
	}
}
