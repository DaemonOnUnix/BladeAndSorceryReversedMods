using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015B RID: 347
	[Guid("20B053BE-E235-43cd-9A2A-8D17A48B7842")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoResult : ISpPhrase
	{
		// Token: 0x06000A7E RID: 2686
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A7F RID: 2687
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A80 RID: 2688
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000A81 RID: 2689
		void Discard(uint dwValueTypes);

		// Token: 0x06000A82 RID: 2690
		void Slot5();

		// Token: 0x06000A83 RID: 2691
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x06000A84 RID: 2692
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x06000A85 RID: 2693
		void Slot8();

		// Token: 0x06000A86 RID: 2694
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x06000A87 RID: 2695
		void Slot10();

		// Token: 0x06000A88 RID: 2696
		void Slot11();
	}
}
