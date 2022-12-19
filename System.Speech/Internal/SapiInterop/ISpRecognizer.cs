using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000080 RID: 128
	[Guid("C2B5F241-DAA0-4507-9E16-5A1EAA2B7A5C")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpRecognizer : ISpProperties
	{
		// Token: 0x06000247 RID: 583
		[PreserveSig]
		int SetPropertyNum([MarshalAs(21)] string pName, int lValue);

		// Token: 0x06000248 RID: 584
		[PreserveSig]
		int GetPropertyNum([MarshalAs(21)] string pName, out int plValue);

		// Token: 0x06000249 RID: 585
		[PreserveSig]
		int SetPropertyString([MarshalAs(21)] string pName, [MarshalAs(21)] string pValue);

		// Token: 0x0600024A RID: 586
		[PreserveSig]
		int GetPropertyString([MarshalAs(21)] string pName, [MarshalAs(21)] out string ppCoMemValue);

		// Token: 0x0600024B RID: 587
		void SetRecognizer(ISpObjectToken pRecognizer);

		// Token: 0x0600024C RID: 588
		void GetRecognizer(out ISpObjectToken ppRecognizer);

		// Token: 0x0600024D RID: 589
		void SetInput([MarshalAs(25)] object pUnkInput, [MarshalAs(2)] bool fAllowFormatChanges);

		// Token: 0x0600024E RID: 590
		void Slot8();

		// Token: 0x0600024F RID: 591
		void Slot9();

		// Token: 0x06000250 RID: 592
		void CreateRecoContext(out ISpRecoContext ppNewCtxt);

		// Token: 0x06000251 RID: 593
		void Slot11();

		// Token: 0x06000252 RID: 594
		void Slot12();

		// Token: 0x06000253 RID: 595
		void Slot13();

		// Token: 0x06000254 RID: 596
		void GetRecoState(out SPRECOSTATE pState);

		// Token: 0x06000255 RID: 597
		void SetRecoState(SPRECOSTATE NewState);

		// Token: 0x06000256 RID: 598
		void GetStatus(out SPRECOGNIZERSTATUS pStatus);

		// Token: 0x06000257 RID: 599
		void GetFormat(SPSTREAMFORMATTYPE WaveFormatType, out Guid pFormatId, out IntPtr ppCoMemWFEX);

		// Token: 0x06000258 RID: 600
		void IsUISupported([MarshalAs(21)] string pszTypeOfUI, IntPtr pvExtraData, uint cbExtraData, [MarshalAs(2)] out bool pfSupported);

		// Token: 0x06000259 RID: 601
		[PreserveSig]
		int DisplayUI(IntPtr hWndParent, [MarshalAs(21)] string pszTitle, [MarshalAs(21)] string pszTypeOfUI, IntPtr pvExtraData, uint cbExtraData);

		// Token: 0x0600025A RID: 602
		[PreserveSig]
		int EmulateRecognition(ISpPhrase pPhrase);
	}
}
