using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000115 RID: 277
	[Guid("06B64F9E-7FDA-11D2-B4F2-00C04F797396")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSpObjectTokens
	{
		// Token: 0x0600099A RID: 2458
		void Slot1();

		// Token: 0x0600099B RID: 2459
		void Slot2();

		// Token: 0x0600099C RID: 2460
		void Slot3();

		// Token: 0x0600099D RID: 2461
		void Slot4();

		// Token: 0x0600099E RID: 2462
		void Item(uint Index, out ISpObjectToken ppToken);

		// Token: 0x0600099F RID: 2463
		void GetCount(out uint pCount);
	}
}
