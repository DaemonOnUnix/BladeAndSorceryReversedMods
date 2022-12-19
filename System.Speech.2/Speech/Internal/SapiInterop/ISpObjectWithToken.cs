using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000111 RID: 273
	[Guid("5B559F40-E952-11D2-BB91-00C04F8EE6C0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpObjectWithToken
	{
		// Token: 0x0600096D RID: 2413
		[PreserveSig]
		int SetObjectToken(ISpObjectToken pToken);

		// Token: 0x0600096E RID: 2414
		[PreserveSig]
		int GetObjectToken(out ISpObjectToken ppToken);
	}
}
