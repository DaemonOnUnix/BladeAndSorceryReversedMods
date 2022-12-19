using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200008B RID: 139
	[InterfaceType(1)]
	[Guid("88A3342A-0BED-4834-922B-88D43173162F")]
	[ComImport]
	internal interface ISpPhraseBuilder : ISpPhrase
	{
		// Token: 0x060002B3 RID: 691
		void GetPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x060002B4 RID: 692
		void GetSerializedPhrase(out IntPtr ppCoMemPhrase);

		// Token: 0x060002B5 RID: 693
		void GetText(uint ulStart, uint ulCount, [MarshalAs(2)] bool fUseTextReplacements, [MarshalAs(21)] out string ppszCoMemText, out byte pbDisplayAttributes);

		// Token: 0x060002B6 RID: 694
		void Discard(uint dwValueTypes);

		// Token: 0x060002B7 RID: 695
		void InitFromPhrase(SPPHRASE pPhrase);

		// Token: 0x060002B8 RID: 696
		void Slot6();

		// Token: 0x060002B9 RID: 697
		void Slot7();

		// Token: 0x060002BA RID: 698
		void Slot8();

		// Token: 0x060002BB RID: 699
		void Slot9();

		// Token: 0x060002BC RID: 700
		void Slot10();
	}
}
