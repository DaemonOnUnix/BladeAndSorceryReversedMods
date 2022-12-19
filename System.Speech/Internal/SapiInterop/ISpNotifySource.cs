using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002F RID: 47
	[Guid("5EFF4AEF-8487-11D2-961C-00C04F8EE628")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpNotifySource
	{
		// Token: 0x06000131 RID: 305
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000132 RID: 306
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000133 RID: 307
		void Slot3();

		// Token: 0x06000134 RID: 308
		void Slot4();

		// Token: 0x06000135 RID: 309
		void Slot5();

		// Token: 0x06000136 RID: 310
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000137 RID: 311
		void Slot7();
	}
}
