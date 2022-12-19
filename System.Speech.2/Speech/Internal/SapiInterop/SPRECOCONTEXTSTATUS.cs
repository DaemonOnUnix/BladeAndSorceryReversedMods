using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000140 RID: 320
	internal struct SPRECOCONTEXTSTATUS
	{
		// Token: 0x04000765 RID: 1893
		internal SPINTERFERENCE eInterference;

		// Token: 0x04000766 RID: 1894
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
		internal short[] szRequestTypeOfUI;

		// Token: 0x04000767 RID: 1895
		internal uint dwReserved1;

		// Token: 0x04000768 RID: 1896
		internal uint dwReserved2;
	}
}
