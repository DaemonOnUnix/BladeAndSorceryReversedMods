using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public class SpeechRecognitionRejectedEventArgs : RecognitionEventArgs
	{
		// Token: 0x06000188 RID: 392 RVA: 0x000067FB File Offset: 0x000049FB
		internal SpeechRecognitionRejectedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
