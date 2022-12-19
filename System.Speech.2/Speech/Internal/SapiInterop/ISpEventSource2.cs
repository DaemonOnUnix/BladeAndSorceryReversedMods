using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010B RID: 267
	[Guid("2373A435-6A4B-429e-A6AC-D4231A61975B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpEventSource2 : ISpEventSource, ISpNotifySource
	{
		// Token: 0x0600093C RID: 2364
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x0600093D RID: 2365
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600093E RID: 2366
		void Slot3();

		// Token: 0x0600093F RID: 2367
		void Slot4();

		// Token: 0x06000940 RID: 2368
		void Slot5();

		// Token: 0x06000941 RID: 2369
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000942 RID: 2370
		void Slot7();

		// Token: 0x06000943 RID: 2371
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x06000944 RID: 2372
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x06000945 RID: 2373
		void Slot10();

		// Token: 0x06000946 RID: 2374
		void GetEventsEx(uint ulCount, out SPEVENTEX pEventArray, out uint pulFetched);
	}
}
