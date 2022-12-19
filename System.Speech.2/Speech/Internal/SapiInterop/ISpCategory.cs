using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000150 RID: 336
	[Guid("B638799F-6598-4c56-B3ED-509CA3F35B22")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpCategory
	{
		// Token: 0x060009F1 RID: 2545
		void GetType(out SPCATEGORYTYPE peCategoryType);

		// Token: 0x060009F2 RID: 2546
		void SetPrefix([MarshalAs(UnmanagedType.LPWStr)] string pszPrefix);

		// Token: 0x060009F3 RID: 2547
		void GetPrefix([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemPrefix);

		// Token: 0x060009F4 RID: 2548
		void SetIsPrefixRequired([MarshalAs(UnmanagedType.Bool)] bool fRequired);

		// Token: 0x060009F5 RID: 2549
		void GetIsPrefixRequired([MarshalAs(UnmanagedType.Bool)] out bool pfRequired);

		// Token: 0x060009F6 RID: 2550
		void SetState(SPCATEGORYSTATE eCategoryState);

		// Token: 0x060009F7 RID: 2551
		void GetState(out SPCATEGORYSTATE peCategoryState);

		// Token: 0x060009F8 RID: 2552
		void SetName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x060009F9 RID: 2553
		void GetName([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemName);

		// Token: 0x060009FA RID: 2554
		void SetIcon([MarshalAs(UnmanagedType.LPWStr)] string pszIcon);

		// Token: 0x060009FB RID: 2555
		void GetIcon([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemIcon);
	}
}
