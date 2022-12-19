using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000069 RID: 105
	internal struct SPRECOCONTEXTSTATUS
	{
		// Token: 0x040001F3 RID: 499
		internal SPINTERFERENCE eInterference;

		// Token: 0x040001F4 RID: 500
		[MarshalAs(30, SizeConst = 255)]
		internal short[] szRequestTypeOfUI;

		// Token: 0x040001F5 RID: 501
		internal uint dwReserved1;

		// Token: 0x040001F6 RID: 502
		internal uint dwReserved2;
	}
}
