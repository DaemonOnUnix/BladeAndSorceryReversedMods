using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007E RID: 126
	[Guid("BEAD311C-52FF-437f-9464-6B21054CA73D")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRecoContext2
	{
		// Token: 0x06000240 RID: 576
		void SetGrammarOptions(SPGRAMMAROPTIONS eGrammarOptions);

		// Token: 0x06000241 RID: 577
		void Slot2();

		// Token: 0x06000242 RID: 578
		void SetAdaptationData2([MarshalAs(21)] string pAdaptationData, uint cch, [MarshalAs(21)] string pTopicName, SPADAPTATIONSETTINGS eSettings, SPADAPTATIONRELEVANCE eRelevance);
	}
}
