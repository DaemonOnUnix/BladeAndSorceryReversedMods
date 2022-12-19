using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007D RID: 125
	[InterfaceType(1)]
	[Guid("F740A62F-7C15-489E-8234-940A33D9272D")]
	[ComImport]
	internal interface ISpRecoContext : ISpEventSource, ISpNotifySource
	{
		// Token: 0x06000224 RID: 548
		void SetNotifySink(ISpNotifySink pNotifySink);

		// Token: 0x06000225 RID: 549
		void SetNotifyWindowMessage(uint hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000226 RID: 550
		void Slot3();

		// Token: 0x06000227 RID: 551
		void Slot4();

		// Token: 0x06000228 RID: 552
		void Slot5();

		// Token: 0x06000229 RID: 553
		[PreserveSig]
		int WaitForNotifyEvent(uint dwMilliseconds);

		// Token: 0x0600022A RID: 554
		void Slot7();

		// Token: 0x0600022B RID: 555
		void SetInterest(ulong ullEventInterest, ulong ullQueuedInterest);

		// Token: 0x0600022C RID: 556
		void GetEvents(uint ulCount, out SPEVENT pEventArray, out uint pulFetched);

		// Token: 0x0600022D RID: 557
		void Slot10();

		// Token: 0x0600022E RID: 558
		void GetRecognizer(out ISpRecognizer ppRecognizer);

		// Token: 0x0600022F RID: 559
		void CreateGrammar(ulong ullGrammarID, out ISpRecoGrammar ppGrammar);

		// Token: 0x06000230 RID: 560
		void GetStatus(out SPRECOCONTEXTSTATUS pStatus);

		// Token: 0x06000231 RID: 561
		void GetMaxAlternates(out uint pcAlternates);

		// Token: 0x06000232 RID: 562
		void SetMaxAlternates(uint cAlternates);

		// Token: 0x06000233 RID: 563
		void SetAudioOptions(SPAUDIOOPTIONS Options, IntPtr pAudioFormatId, IntPtr pWaveFormatEx);

		// Token: 0x06000234 RID: 564
		void Slot17();

		// Token: 0x06000235 RID: 565
		void Slot18();

		// Token: 0x06000236 RID: 566
		void Bookmark(SPBOOKMARKOPTIONS Options, ulong ullStreamPosition, IntPtr lparamEvent);

		// Token: 0x06000237 RID: 567
		void Slot20();

		// Token: 0x06000238 RID: 568
		void Pause(uint dwReserved);

		// Token: 0x06000239 RID: 569
		void Resume(uint dwReserved);

		// Token: 0x0600023A RID: 570
		void Slot23();

		// Token: 0x0600023B RID: 571
		void Slot24();

		// Token: 0x0600023C RID: 572
		void Slot25();

		// Token: 0x0600023D RID: 573
		void Slot26();

		// Token: 0x0600023E RID: 574
		void SetContextState(SPCONTEXTSTATE eContextState);

		// Token: 0x0600023F RID: 575
		void Slot28();
	}
}
