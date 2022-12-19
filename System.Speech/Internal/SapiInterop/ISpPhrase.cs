using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000083 RID: 131
	[Guid("1A5C0354-B621-4b5a-8791-D306ED379E53")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpPhrase
	{
		// Token: 0x06000278 RID: 632
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000279 RID: 633
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x0600027A RID: 634
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x0600027B RID: 635
		void Discard(uint dwValueTypes);
	}
}
