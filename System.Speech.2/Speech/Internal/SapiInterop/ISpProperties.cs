using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000156 RID: 342
	[Guid("5B4FB971-B115-4DE1-AD97-E482E3BF6EE4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpProperties
	{
		// Token: 0x06000A45 RID: 2629
		[PreserveSig]
		int SetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, int lValue);

		// Token: 0x06000A46 RID: 2630
		[PreserveSig]
		int GetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, out int plValue);

		// Token: 0x06000A47 RID: 2631
		[PreserveSig]
		int SetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] string pValue);

		// Token: 0x06000A48 RID: 2632
		[PreserveSig]
		int GetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] out string ppCoMemValue);
	}
}
