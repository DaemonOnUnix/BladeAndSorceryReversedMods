using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000162 RID: 354
	[Guid("88A3342A-0BED-4834-922B-88D43173162F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpPhraseBuilder : ISpPhrase
	{
		// Token: 0x06000AB5 RID: 2741
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000AB6 RID: 2742
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x06000AB7 RID: 2743
		void GetText(uint ulStart, uint ulCount, [MarshalAs(UnmanagedType.Bool)] bool fUseTextReplacements, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x06000AB8 RID: 2744
		void Discard(uint dwValueTypes);

		// Token: 0x06000AB9 RID: 2745
		void InitFromPhrase(SPPHRASE pPhrase);

		// Token: 0x06000ABA RID: 2746
		void Slot6();

		// Token: 0x06000ABB RID: 2747
		void Slot7();

		// Token: 0x06000ABC RID: 2748
		void Slot8();

		// Token: 0x06000ABD RID: 2749
		void Slot9();

		// Token: 0x06000ABE RID: 2750
		void Slot10();
	}
}
