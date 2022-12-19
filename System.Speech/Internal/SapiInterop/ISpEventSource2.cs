using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000031 RID: 49
	[Guid("2373A435-6A4B-429e-A6AC-D4231A61975B")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpEventSource2 : ISpEventSource, ISpNotifySource
	{
		// Token: 0x06000142 RID: 322
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000143 RID: 323
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000144 RID: 324
		void Slot3();

		// Token: 0x06000145 RID: 325
		void Slot4();

		// Token: 0x06000146 RID: 326
		void Slot5();

		// Token: 0x06000147 RID: 327
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000148 RID: 328
		void Slot7();

		// Token: 0x06000149 RID: 329
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x0600014A RID: 330
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x0600014B RID: 331
		void Slot10();

		// Token: 0x0600014C RID: 332
		void GetEventsEx(uint ulCount, out SPEVENTEX pEventArray, out uint pulFetched);
	}
}
