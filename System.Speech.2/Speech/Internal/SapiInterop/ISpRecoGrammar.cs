using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000152 RID: 338
	[Guid("2177DB29-7F45-47D0-8554-067E91C80502")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoGrammar : ISpGrammarBuilder
	{
		// Token: 0x06000A04 RID: 2564
		void Slot1();

		// Token: 0x06000A05 RID: 2565
		void Slot2();

		// Token: 0x06000A06 RID: 2566
		void Slot3();

		// Token: 0x06000A07 RID: 2567
		void Slot4();

		// Token: 0x06000A08 RID: 2568
		void Slot5();

		// Token: 0x06000A09 RID: 2569
		void Slot6();

		// Token: 0x06000A0A RID: 2570
		void Slot7();

		// Token: 0x06000A0B RID: 2571
		void Slot8();

		// Token: 0x06000A0C RID: 2572
		void Slot9();

		// Token: 0x06000A0D RID: 2573
		void Slot10();

		// Token: 0x06000A0E RID: 2574
		void LoadCmdFromFile([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, SPLOADOPTIONS Options);

		// Token: 0x06000A0F RID: 2575
		void Slot12();

		// Token: 0x06000A10 RID: 2576
		void Slot13();

		// Token: 0x06000A11 RID: 2577
		void LoadCmdFromMemory(IntPtr pGrammar, SPLOADOPTIONS Options);

		// Token: 0x06000A12 RID: 2578
		void Slot15();

		// Token: 0x06000A13 RID: 2579
		[PreserveSig]
		int SetRuleState([MarshalAs(UnmanagedType.LPWStr)] string pszName, IntPtr pReserved, SPRULESTATE NewState);

		// Token: 0x06000A14 RID: 2580
		void Slot17();

		// Token: 0x06000A15 RID: 2581
		void LoadDictation([MarshalAs(UnmanagedType.LPWStr)] string pszTopicName, SPLOADOPTIONS Options);

		// Token: 0x06000A16 RID: 2582
		void Slot19();

		// Token: 0x06000A17 RID: 2583
		[PreserveSig]
		int SetDictationState(SPRULESTATE NewState);

		// Token: 0x06000A18 RID: 2584
		void SetWordSequenceData([MarshalAs(UnmanagedType.LPWStr)] string pText, uint cchText, ref SPTEXTSELECTIONINFO pInfo);

		// Token: 0x06000A19 RID: 2585
		void SetTextSelection(ref SPTEXTSELECTIONINFO pInfo);

		// Token: 0x06000A1A RID: 2586
		void Slot23();

		// Token: 0x06000A1B RID: 2587
		void SetGrammarState(SPGRAMMARSTATE eGrammarState);

		// Token: 0x06000A1C RID: 2588
		void Slot25();

		// Token: 0x06000A1D RID: 2589
		void Slot26();
	}
}
