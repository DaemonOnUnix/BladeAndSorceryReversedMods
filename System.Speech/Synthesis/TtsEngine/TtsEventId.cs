using System;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000173 RID: 371
	public enum TtsEventId
	{
		// Token: 0x04000721 RID: 1825
		StartInputStream = 1,
		// Token: 0x04000722 RID: 1826
		EndInputStream,
		// Token: 0x04000723 RID: 1827
		VoiceChange,
		// Token: 0x04000724 RID: 1828
		Bookmark,
		// Token: 0x04000725 RID: 1829
		WordBoundary,
		// Token: 0x04000726 RID: 1830
		Phoneme,
		// Token: 0x04000727 RID: 1831
		SentenceBoundary,
		// Token: 0x04000728 RID: 1832
		Viseme,
		// Token: 0x04000729 RID: 1833
		AudioLevel
	}
}
