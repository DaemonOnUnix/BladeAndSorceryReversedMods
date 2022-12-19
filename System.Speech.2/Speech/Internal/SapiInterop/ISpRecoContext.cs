using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000154 RID: 340
	[Guid("F740A62F-7C15-489E-8234-940A33D9272D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoContext : ISpEventSource, ISpNotifySource
	{
		// Token: 0x06000A26 RID: 2598
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000A27 RID: 2599
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000A28 RID: 2600
		void Slot3();

		// Token: 0x06000A29 RID: 2601
		void Slot4();

		// Token: 0x06000A2A RID: 2602
		void Slot5();

		// Token: 0x06000A2B RID: 2603
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x06000A2C RID: 2604
		void Slot7();

		// Token: 0x06000A2D RID: 2605
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x06000A2E RID: 2606
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x06000A2F RID: 2607
		void Slot10();

		// Token: 0x06000A30 RID: 2608
		void GetRecognizer(out ISpRecognizer ppRecognizer);

		// Token: 0x06000A31 RID: 2609
		void CreateGrammar(ulong ullGrammarID, out ISpRecoGrammar ppGrammar);

		// Token: 0x06000A32 RID: 2610
		void GetStatus(out SPRECOCONTEXTSTATUS pStatus);

		// Token: 0x06000A33 RID: 2611
		void GetMaxAlternates(out uint pcAlternates);

		// Token: 0x06000A34 RID: 2612
		void SetMaxAlternates(uint cAlternates);

		// Token: 0x06000A35 RID: 2613
		void SetAudioOptions(SPAUDIOOPTIONS Options, IntPtr pAudioFormatId, IntPtr pWaveFormatEx);

		// Token: 0x06000A36 RID: 2614
		void Slot17();

		// Token: 0x06000A37 RID: 2615
		void Slot18();

		// Token: 0x06000A38 RID: 2616
		void Bookmark(SPBOOKMARKOPTIONS Options, ulong ullStreamPosition, IntPtr lparamEvent);

		// Token: 0x06000A39 RID: 2617
		void Slot20();

		// Token: 0x06000A3A RID: 2618
		void Pause(uint dwReserved);

		// Token: 0x06000A3B RID: 2619
		void Resume(uint dwReserved);

		// Token: 0x06000A3C RID: 2620
		void Slot23();

		// Token: 0x06000A3D RID: 2621
		void Slot24();

		// Token: 0x06000A3E RID: 2622
		void Slot25();

		// Token: 0x06000A3F RID: 2623
		void Slot26();

		// Token: 0x06000A40 RID: 2624
		void SetContextState(SPCONTEXTSTATE eContextState);

		// Token: 0x06000A41 RID: 2625
		void Slot28();
	}
}
