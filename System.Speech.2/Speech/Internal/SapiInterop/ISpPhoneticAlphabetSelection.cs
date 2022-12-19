using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000116 RID: 278
	[Guid("B2745EFD-42CE-48CA-81F1-A96E02538A90")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpPhoneticAlphabetSelection
	{
		// Token: 0x060009A0 RID: 2464
		void IsAlphabetUPS([MarshalAs(UnmanagedType.Bool)] out bool pfIsUPS);

		// Token: 0x060009A1 RID: 2465
		void SetAlphabetToUPS([MarshalAs(UnmanagedType.Bool)] bool fForceUPS);
	}
}
