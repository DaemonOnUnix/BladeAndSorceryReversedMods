using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010C RID: 268
	[Guid("259684DC-37C3-11D2-9603-00C04F8EE628")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpNotifySink
	{
		// Token: 0x06000947 RID: 2375
		void Notify();
	}
}
