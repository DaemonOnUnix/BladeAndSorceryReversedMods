using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200010D RID: 269
	internal interface IRecognizerInternal
	{
		// Token: 0x060006D3 RID: 1747
		void SetGrammarState(Grammar grammar, bool enabled);

		// Token: 0x060006D4 RID: 1748
		void SetGrammarWeight(Grammar grammar, float weight);

		// Token: 0x060006D5 RID: 1749
		void SetGrammarPriority(Grammar grammar, int priority);

		// Token: 0x060006D6 RID: 1750
		Grammar GetGrammarFromId(ulong id);

		// Token: 0x060006D7 RID: 1751
		void SetDictationContext(Grammar grammar, string precedingText, string subsequentText);
	}
}
