using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Recognition
{
	// Token: 0x02000066 RID: 102
	[Guid("06B64F9E-7FDA-11D2-B4F2-00C04F797396")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSpObjectTokens
	{
		// Token: 0x060002DB RID: 731
		void Slot1();

		// Token: 0x060002DC RID: 732
		void Slot2();

		// Token: 0x060002DD RID: 733
		void Slot3();

		// Token: 0x060002DE RID: 734
		void Slot4();

		// Token: 0x060002DF RID: 735
		void Item(uint Index, out ISpObjectToken ppToken);

		// Token: 0x060002E0 RID: 736
		void GetCount(out uint pCount);
	}
}
