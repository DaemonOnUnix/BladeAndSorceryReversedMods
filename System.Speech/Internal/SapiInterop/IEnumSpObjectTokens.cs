using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000038 RID: 56
	[InterfaceType(1)]
	[Guid("06B64F9E-7FDA-11D2-B4F2-00C04F797396")]
	[ComImport]
	internal interface IEnumSpObjectTokens
	{
		// Token: 0x0600017D RID: 381
		void Slot1();

		// Token: 0x0600017E RID: 382
		void Slot2();

		// Token: 0x0600017F RID: 383
		void Slot3();

		// Token: 0x06000180 RID: 384
		void Slot4();

		// Token: 0x06000181 RID: 385
		void Item(uint Index, out ISpObjectToken ppToken);

		// Token: 0x06000182 RID: 386
		void GetCount(out uint pCount);
	}
}
