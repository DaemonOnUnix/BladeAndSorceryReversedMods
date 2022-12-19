using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000089 RID: 137
	[InterfaceType(1)]
	[Guid("C8D7C7E2-0DDE-44b7-AFE3-B0C991FBEB5E")]
	[ComImport]
	internal interface ISpDisplayAlternates
	{
		// Token: 0x060002AF RID: 687
		void GetDisplayAlternates(IntPtr pPhrase, uint cRequestCount, [MarshalAs(42, SizeParamIndex = 2)] [Out] IntPtr[] ppCoMemPhrases, out uint pcPhrasesReturned);
	}
}
