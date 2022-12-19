using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000085 RID: 133
	[Guid("8FCEBC98-4E49-4067-9C6C-D86A0E092E3D")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpPhraseAlt : ISpPhrase
	{
		// Token: 0x06000287 RID: 647
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000288 RID: 648
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000289 RID: 649
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x0600028A RID: 650
		void Discard(uint dwValueTypes);

		// Token: 0x0600028B RID: 651
		void GetAltInfo(out ISpPhrase ppParent, out uint pulStartElementInParent, out uint pcElementsInParent, out uint pcElementsInAlt);

		// Token: 0x0600028C RID: 652
		void Commit();
	}
}
