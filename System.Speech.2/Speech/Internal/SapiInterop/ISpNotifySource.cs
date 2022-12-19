using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000109 RID: 265
	[Guid("5EFF4AEF-8487-11D2-961C-00C04F8EE628")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpNotifySource
	{
		// Token: 0x0600092B RID: 2347
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x0600092C RID: 2348
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600092D RID: 2349
		void Slot3();

		// Token: 0x0600092E RID: 2350
		void Slot4();

		// Token: 0x0600092F RID: 2351
		void Slot5();

		// Token: 0x06000930 RID: 2352
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000931 RID: 2353
		void Slot7();
	}
}
