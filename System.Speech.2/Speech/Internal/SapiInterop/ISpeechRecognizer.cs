using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000159 RID: 345
	[Guid("2D5F1C0C-BD75-4b08-9478-3B11FEA2586C")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComImport]
	internal interface ISpeechRecognizer
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A61 RID: 2657
		// (set) Token: 0x06000A60 RID: 2656
		object Slot1 { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A63 RID: 2659
		// (set) Token: 0x06000A62 RID: 2658
		object Slot2 { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A65 RID: 2661
		// (set) Token: 0x06000A64 RID: 2660
		object Slot3 { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A67 RID: 2663
		// (set) Token: 0x06000A66 RID: 2662
		object Slot4 { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A68 RID: 2664
		object Slot5 { get; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A6A RID: 2666
		// (set) Token: 0x06000A69 RID: 2665
		object Slot6 { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A6C RID: 2668
		// (set) Token: 0x06000A6B RID: 2667
		object Slot7 { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A6D RID: 2669
		object Slot8 { get; }

		// Token: 0x06000A6E RID: 2670
		[DispId(9)]
		[PreserveSig]
		int EmulateRecognition(object TextElements, ref object ElementDisplayAttributes, int LanguageId);

		// Token: 0x06000A6F RID: 2671
		void Slot10();

		// Token: 0x06000A70 RID: 2672
		void Slot11();

		// Token: 0x06000A71 RID: 2673
		void Slot12();

		// Token: 0x06000A72 RID: 2674
		void Slot13();

		// Token: 0x06000A73 RID: 2675
		void Slot14();

		// Token: 0x06000A74 RID: 2676
		void Slot15();

		// Token: 0x06000A75 RID: 2677
		void Slot16();

		// Token: 0x06000A76 RID: 2678
		void Slot17();

		// Token: 0x06000A77 RID: 2679
		void Slot18();

		// Token: 0x06000A78 RID: 2680
		void Slot19();

		// Token: 0x06000A79 RID: 2681
		void Slot20();
	}
}
