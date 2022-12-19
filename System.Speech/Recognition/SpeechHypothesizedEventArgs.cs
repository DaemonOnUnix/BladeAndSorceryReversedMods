using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200012D RID: 301
	[Serializable]
	public class SpeechHypothesizedEventArgs : RecognitionEventArgs
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x00022B5D File Offset: 0x00021B5D
		internal SpeechHypothesizedEventArgs(RecognitionResult result)
			: base(result)
		{
		}
	}
}
