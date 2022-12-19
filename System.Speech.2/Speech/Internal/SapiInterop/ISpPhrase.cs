using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015A RID: 346
	[Guid("1A5C0354-B621-4b5a-8791-D306ED379E53")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpPhrase
	{
		// Token: 0x06000A7A RID: 2682
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A7B RID: 2683
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A7C RID: 2684
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000A7D RID: 2685
		void Discard(uint dwValueTypes);
	}
}
