using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200015C RID: 348
	[Guid("8FCEBC98-4E49-4067-9C6C-D86A0E092E3D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpPhraseAlt : ISpPhrase
	{
		// Token: 0x06000A89 RID: 2697
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A8A RID: 2698
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000A8B RID: 2699
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000A8C RID: 2700
		void Discard(uint dwValueTypes);

		// Token: 0x06000A8D RID: 2701
		void GetAltInfo(out ISpPhrase ppParent, out uint pulStartElementInParent, out uint pcElementsInParent, out uint pcElementsInAlt);

		// Token: 0x06000A8E RID: 2702
		void Commit();
	}
}
