using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000048 RID: 72
	internal interface IRecognizerInternal
	{
		// Token: 0x0600017E RID: 382
		void SetGrammarState(Grammar grammar, bool enabled);

		// Token: 0x0600017F RID: 383
		void SetGrammarWeight(Grammar grammar, float weight);

		// Token: 0x06000180 RID: 384
		void SetGrammarPriority(Grammar grammar, int priority);

		// Token: 0x06000181 RID: 385
		Grammar GetGrammarFromId(ulong id);

		// Token: 0x06000182 RID: 386
		void SetDictationContext(Grammar grammar, string precedingText, string subsequentText);
	}
}
