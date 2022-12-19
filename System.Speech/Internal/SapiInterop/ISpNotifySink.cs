using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000029 RID: 41
	[Guid("259684DC-37C3-11D2-9603-00C04F8EE628")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpNotifySink
	{
		// Token: 0x0600012A RID: 298
		void Notify();
	}
}
