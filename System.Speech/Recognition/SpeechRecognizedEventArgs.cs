using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200012B RID: 299
	[Serializable]
	public class SpeechRecognizedEventArgs : RecognitionEventArgs
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x00022B4B File Offset: 0x00021B4B
		internal SpeechRecognizedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
