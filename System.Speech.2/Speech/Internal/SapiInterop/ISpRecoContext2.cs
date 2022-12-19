using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000155 RID: 341
	[Guid("BEAD311C-52FF-437f-9464-6B21054CA73D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoContext2
	{
		// Token: 0x06000A42 RID: 2626
		void SetGrammarOptions(SPGRAMMAROPTIONS eGrammarOptions);

		// Token: 0x06000A43 RID: 2627
		void Slot2();

		// Token: 0x06000A44 RID: 2628
		void SetAdaptationData2([MarshalAs(UnmanagedType.LPWStr)] string pAdaptationData, uint cch, [MarshalAs(UnmanagedType.LPWStr)] string pTopicName, SPADAPTATIONSETTINGS eSettings, SPADAPTATIONRELEVANCE eRelevance);
	}
}
