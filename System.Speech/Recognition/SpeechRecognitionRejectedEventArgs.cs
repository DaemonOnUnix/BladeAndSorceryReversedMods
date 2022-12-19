using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200012C RID: 300
	[Serializable]
	public class SpeechRecognitionRejectedEventArgs : RecognitionEventArgs
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x00022B54 File Offset: 0x00021B54
		internal SpeechRecognitionRejectedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
