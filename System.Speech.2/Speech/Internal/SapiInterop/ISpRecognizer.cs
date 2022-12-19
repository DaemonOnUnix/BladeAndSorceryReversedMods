using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000157 RID: 343
	[Guid("C2B5F241-DAA0-4507-9E16-5A1EAA2B7A5C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecognizer : ISpProperties
	{
		// Token: 0x06000A49 RID: 2633
		[PreserveSig]
		int SetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, int lValue);

		// Token: 0x06000A4A RID: 2634
		[PreserveSig]
		int GetPropertyNum([MarshalAs(UnmanagedType.LPWStr)] string pName, out int plValue);

		// Token: 0x06000A4B RID: 2635
		[PreserveSig]
		int SetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] string pValue);

		// Token: 0x06000A4C RID: 2636
		[PreserveSig]
		int GetPropertyString([MarshalAs(UnmanagedType.LPWStr)] string pName, [MarshalAs(UnmanagedType.LPWStr)] out string ppCoMemValue);

		// Token: 0x06000A4D RID: 2637
		void SetRecognizer(ISpObjectToken pRecognizer);

		// Token: 0x06000A4E RID: 2638
		void GetRecognizer(out ISpObjectToken ppRecognizer);

		// Token: 0x06000A4F RID: 2639
		void SetInput([MarshalAs(UnmanagedType.IUnknown)] object pUnkInput, [MarshalAs(UnmanagedType.Bool)] bool fAllowFormatChanges);

		// Token: 0x06000A50 RID: 2640
		void Slot8();

		// Token: 0x06000A51 RID: 2641
		void Slot9();

		// Token: 0x06000A52 RID: 2642
		void CreateRecoContext(out ISpRecoContext ppNewCtxt);

		// Token: 0x06000A53 RID: 2643
		void Slot11();

		// Token: 0x06000A54 RID: 2644
		void Slot12();

		// Token: 0x06000A55 RID: 2645
		void Slot13();

		// Token: 0x06000A56 RID: 2646
		void GetRecoState(out SPRECOSTATE pState);

		// Token: 0x06000A57 RID: 2647
		void SetRecoState(SPRECOSTATE NewState);

		// Token: 0x06000A58 RID: 2648
		void GetStatus(out SPRECOGNIZERSTATUS pStatus);

		// Token: 0x06000A59 RID: 2649
		void GetFormat(SPSTREAMFORMATTYPE WaveFormatType, out Guid pFormatId, out IntPtr ppCoMemWFEX);

		// Token: 0x06000A5A RID: 2650
		void IsUISupported([MarshalAs(UnmanagedType.LPWStr)] string pszTypeOfUI, IntPtr pvExtraData, uint cbExtraData, [MarshalAs(UnmanagedType.Bool)] out bool pfSupported);

		// Token: 0x06000A5B RID: 2651
		[PreserveSig]
		int DisplayUI(IntPtr hWndParent, [MarshalAs(UnmanagedType.LPWStr)] string pszTitle, [MarshalAs(UnmanagedType.LPWStr)] string pszTypeOfUI, IntPtr pvExtraData, uint cbExtraData);

		// Token: 0x06000A5C RID: 2652
		[PreserveSig]
		int EmulateRecognition(ISpPhrase pPhrase);
	}
}
