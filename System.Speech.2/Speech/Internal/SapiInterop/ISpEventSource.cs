using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010A RID: 266
	[Guid("BE7A9CCE-5F9E-11D2-960F-00C04F8EE628")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpEventSource : ISpNotifySource
	{
		// Token: 0x06000932 RID: 2354
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000933 RID: 2355
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000934 RID: 2356
		void Slot3();

		// Token: 0x06000935 RID: 2357
		void Slot4();

		// Token: 0x06000936 RID: 2358
		void Slot5();

		// Token: 0x06000937 RID: 2359
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000938 RID: 2360
		void Slot7();

		// Token: 0x06000939 RID: 2361
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x0600093A RID: 2362
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x0600093B RID: 2363
		void Slot10();
	}
}
