using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000030 RID: 48
	[Guid("BE7A9CCE-5F9E-11D2-960F-00C04F8EE628")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpEventSource : ISpNotifySource
	{
		// Token: 0x06000138 RID: 312
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000139 RID: 313
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600013A RID: 314
		void Slot3();

		// Token: 0x0600013B RID: 315
		void Slot4();

		// Token: 0x0600013C RID: 316
		void Slot5();

		// Token: 0x0600013D RID: 317
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x0600013E RID: 318
		void Slot7();

		// Token: 0x0600013F RID: 319
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x06000140 RID: 320
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x06000141 RID: 321
		void Slot10();
	}
}
