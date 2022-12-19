using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015D RID: 349
	[Guid("27CAC6C4-88F2-41f2-8817-0C95E59F1E6E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoResult2 : ISpRecoResult, ISpPhrase
	{
		// Token: 0x06000A8F RID: 2703
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A90 RID: 2704
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A91 RID: 2705
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000A92 RID: 2706
		void Discard(uint dwValueTypes);

		// Token: 0x06000A93 RID: 2707
		void Slot5();

		// Token: 0x06000A94 RID: 2708
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x06000A95 RID: 2709
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x06000A96 RID: 2710
		void Slot8();

		// Token: 0x06000A97 RID: 2711
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x06000A98 RID: 2712
		void Slot10();

		// Token: 0x06000A99 RID: 2713
		void Slot11();

		// Token: 0x06000A9A RID: 2714
		void CommitAlternate(ISpPhraseAlt pPhraseAlt, out ISpRecoResult ppNewResult);

		// Token: 0x06000A9B RID: 2715
		void CommitText(uint ulStartElement, uint ulCountOfElements, [MarshalAs(UnmanagedType.LPWStr)] string pszCorrectedData, SPCOMMITFLAGS commitFlags);

		// Token: 0x06000A9C RID: 2716
		void SetTextFeedback([MarshalAs(UnmanagedType.LPWStr)] string pszFeedback, [MarshalAs(UnmanagedType.Bool)] bool fSuccessful);
	}
}
