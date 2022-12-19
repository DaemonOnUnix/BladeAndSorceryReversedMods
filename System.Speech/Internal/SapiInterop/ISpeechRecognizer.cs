using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000082 RID: 130
	[InterfaceType(0)]
	[Guid("2D5F1C0C-BD75-4b08-9478-3B11FEA2586C")]
	[ComImport]
	internal interface ISpeechRecognizer
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600025F RID: 607
		// (set) Token: 0x0600025E RID: 606
		object Slot1 { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000261 RID: 609
		// (set) Token: 0x06000260 RID: 608
		object Slot2 { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000263 RID: 611
		// (set) Token: 0x06000262 RID: 610
		object Slot3 { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000265 RID: 613
		// (set) Token: 0x06000264 RID: 612
		object Slot4 { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000266 RID: 614
		object Slot5 { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000268 RID: 616
		// (set) Token: 0x06000267 RID: 615
		object Slot6 { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600026A RID: 618
		// (set) Token: 0x06000269 RID: 617
		object Slot7 { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600026B RID: 619
		object Slot8 { get; }

		// Token: 0x0600026C RID: 620
		[DispId(9)]
		[PreserveSig]
		int EmulateRecognition(object TextElements, ref object ElementDisplayAttributes, int LanguageId);

		// Token: 0x0600026D RID: 621
		void Slot10();

		// Token: 0x0600026E RID: 622
		void Slot11();

		// Token: 0x0600026F RID: 623
		void Slot12();

		// Token: 0x06000270 RID: 624
		void Slot13();

		// Token: 0x06000271 RID: 625
		void Slot14();

		// Token: 0x06000272 RID: 626
		void Slot15();

		// Token: 0x06000273 RID: 627
		void Slot16();

		// Token: 0x06000274 RID: 628
		void Slot17();

		// Token: 0x06000275 RID: 629
		void Slot18();

		// Token: 0x06000276 RID: 630
		void Slot19();

		// Token: 0x06000277 RID: 631
		void Slot20();
	}
}
