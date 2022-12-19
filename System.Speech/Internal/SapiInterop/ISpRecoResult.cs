using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000084 RID: 132
	[Guid("20B053BE-E235-43cd-9A2A-8D17A48B7842")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRecoResult : ISpPhrase
	{
		// Token: 0x0600027C RID: 636
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600027D RID: 637
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600027E RID: 638
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x0600027F RID: 639
		void Discard(uint dwValueTypes);

		// Token: 0x06000280 RID: 640
		void Slot5();

		// Token: 0x06000281 RID: 641
		void GetAlternates(int ulStartElement, int cElements, int ulRequestCount, [MarshalAs(42, SizeParamIndex = 2)] [Out] IntPtr[] ppPhrases, out int pcPhrasesReturned);

		// Token: 0x06000282 RID: 642
		void GetAudio(uint ulStartElement, uint cElements, out ISpStreamFormat ppStream);

		// Token: 0x06000283 RID: 643
		void Slot8();

		// Token: 0x06000284 RID: 644
		void Serialize(out IntPtr ppCoMemSerializedResult);

		// Token: 0x06000285 RID: 645
		void Slot10();

		// Token: 0x06000286 RID: 646
		void Slot11();
	}
}
