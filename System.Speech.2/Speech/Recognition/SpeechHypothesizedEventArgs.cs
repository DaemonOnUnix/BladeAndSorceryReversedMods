using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public class SpeechHypothesizedEventArgs : RecognitionEventArgs
	{
		// Token: 0x06000189 RID: 393 RVA: 0x000067FB File Offset: 0x000049FB
		internal SpeechHypothesizedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
