using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007B RID: 123
	[Guid("2177DB29-7F45-47D0-8554-067E91C80502")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRecoGrammar : ISpGrammarBuilder
	{
		// Token: 0x06000202 RID: 514
		void Slot1();

		// Token: 0x06000203 RID: 515
		void Slot2();

		// Token: 0x06000204 RID: 516
		void Slot3();

		// Token: 0x06000205 RID: 517
		void Slot4();

		// Token: 0x06000206 RID: 518
		void Slot5();

		// Token: 0x06000207 RID: 519
		void Slot6();

		// Token: 0x06000208 RID: 520
		void Slot7();

		// Token: 0x06000209 RID: 521
		void Slot8();

		// Token: 0x0600020A RID: 522
		void Slot9();

		// Token: 0x0600020B RID: 523
		void Slot10();

		// Token: 0x0600020C RID: 524
		void LoadCmdFromFile([MarshalAs(21)] string pszFileName, SPLOADOPTIONS Options);

		// Token: 0x0600020D RID: 525
		void Slot12();

		// Token: 0x0600020E RID: 526
		void Slot13();

		// Token: 0x0600020F RID: 527
		void LoadCmdFromMemory(IntPtr pGrammar, SPLOADOPTIONS Options);

		// Token: 0x06000210 RID: 528
		void Slot15();

		// Token: 0x06000211 RID: 529
		[PreserveSig]
		int SetRuleState([MarshalAs(21)] string pszName, IntPtr pReserved, SPRULESTATE NewState);

		// Token: 0x06000212 RID: 530
		void Slot17();

		// Token: 0x06000213 RID: 531
		void LoadDictation([MarshalAs(21)] string pszTopicName, SPLOADOPTIONS Options);

		// Token: 0x06000214 RID: 532
		void Slot19();

		// Token: 0x06000215 RID: 533
		[PreserveSig]
		int SetDictationState(SPRULESTATE NewState);

		// Token: 0x06000216 RID: 534
		void SetWordSequenceData([MarshalAs(21)] string pText, uint cchText, ref SPTEXTSELECTIONINFO pInfo);

		// Token: 0x06000217 RID: 535
		void SetTextSelection(ref SPTEXTSELECTIONINFO pInfo);

		// Token: 0x06000218 RID: 536
		void Slot23();

		// Token: 0x06000219 RID: 537
		void SetGrammarState(SPGRAMMARSTATE eGrammarState);

		// Token: 0x0600021A RID: 538
		void Slot25();

		// Token: 0x0600021B RID: 539
		void Slot26();
	}
}
