using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000158 RID: 344
	[Guid("8FC6D974-C81E-4098-93C5-0147F61ED4D3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecognizer2
	{
		// Token: 0x06000A5D RID: 2653
		[PreserveSig]
		int EmulateRecognitionEx(ISpPhrase pPhrase, uint dwCompareFlags);

		// Token: 0x06000A5E RID: 2654
		void SetTrainingState(bool fDoingTraining, bool fAdaptFromTrainingData);

		// Token: 0x06000A5F RID: 2655
		void ResetAcousticModelAdaptation();
	}
}
