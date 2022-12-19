using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000160 RID: 352
	[Guid("C8D7C7E2-0DDE-44b7-AFE3-B0C991FBEB5E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpDisplayAlternates
	{
		// Token: 0x06000AB1 RID: 2737
		void GetDisplayAlternates(IntPtr pPhrase, uint cRequestCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] IntPtr[] ppCoMemPhrases, out uint pcPhrasesReturned);
	}
}
