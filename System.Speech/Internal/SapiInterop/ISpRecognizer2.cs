using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000081 RID: 129
	[InterfaceType(1)]
	[Guid("8FC6D974-C81E-4098-93C5-0147F61ED4D3")]
	[ComImport]
	internal interface ISpRecognizer2
	{
		// Token: 0x0600025B RID: 603
		[PreserveSig]
		int EmulateRecognitionEx(ISpPhrase pPhrase, uint dwCompareFlags);

		// Token: 0x0600025C RID: 604
		void SetTrainingState(bool fDoingTraining, bool fAdaptFromTrainingData);

		// Token: 0x0600025D RID: 605
		void ResetAcousticModelAdaptation();
	}
}
