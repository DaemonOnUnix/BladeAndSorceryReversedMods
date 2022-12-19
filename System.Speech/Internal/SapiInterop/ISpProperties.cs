using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007F RID: 127
	[InterfaceType(1)]
	[Guid("5B4FB971-B115-4DE1-AD97-E482E3BF6EE4")]
	[ComImport]
	internal interface ISpProperties
	{
		// Token: 0x06000243 RID: 579
		[PreserveSig]
		int SetPropertyNum([MarshalAs(21)] string pName, int lValue);

		// Token: 0x06000244 RID: 580
		[PreserveSig]
		int GetPropertyNum([MarshalAs(21)] string pName, out int plValue);

		// Token: 0x06000245 RID: 581
		[PreserveSig]
		int SetPropertyString([MarshalAs(21)] string pName, [MarshalAs(21)] string pValue);

		// Token: 0x06000246 RID: 582
		[PreserveSig]
		int GetPropertyString([MarshalAs(21)] string pName, [MarshalAs(21)] out string ppCoMemValue);
	}
}
