using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200003E RID: 62
	public enum TtsEventId
	{
		// Token: 0x040002A7 RID: 679
		StartInputStream = 1,
		// Token: 0x040002A8 RID: 680
		EndInputStream,
		// Token: 0x040002A9 RID: 681
		VoiceChange,
		// Token: 0x040002AA RID: 682
		Bookmark,
		// Token: 0x040002AB RID: 683
		WordBoundary,
		// Token: 0x040002AC RID: 684
		Phoneme,
		// Token: 0x040002AD RID: 685
		SentenceBoundary,
		// Token: 0x040002AE RID: 686
		Viseme,
		// Token: 0x040002AF RID: 687
		AudioLevel
	}
}
