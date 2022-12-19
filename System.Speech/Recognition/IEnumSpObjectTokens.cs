using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x0200019A RID: 410
	[Guid("06B64F9E-7FDA-11D2-B4F2-00C04F797396")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IEnumSpObjectTokens
	{
		// Token: 0x06000ACD RID: 2765
		void Slot1();

		// Token: 0x06000ACE RID: 2766
		void Slot2();

		// Token: 0x06000ACF RID: 2767
		void Slot3();

		// Token: 0x06000AD0 RID: 2768
		void Slot4();

		// Token: 0x06000AD1 RID: 2769
		void Item(uint Index, out ISpObjectToken ppToken);

		// Token: 0x06000AD2 RID: 2770
		void GetCount(out uint pCount);
	}
}
