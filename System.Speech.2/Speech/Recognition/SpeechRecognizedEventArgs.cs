using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class SpeechRecognizedEventArgs : RecognitionEventArgs
	{
		// Token: 0x06000187 RID: 391 RVA: 0x000067FB File Offset: 0x000049FB
		internal SpeechRecognizedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
