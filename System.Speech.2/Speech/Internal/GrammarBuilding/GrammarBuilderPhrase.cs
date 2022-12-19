using System;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009D RID: 157
	internal sealed class GrammarBuilderPhrase : GrammarBuilderBase
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x00015320 File Offset: 0x00013520
		internal GrammarBuilderPhrase(string phrase)
			: this(phrase, false, SubsetMatchingMode.OrderedSubset)
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001532B File Offset: 0x0001352B
		internal GrammarBuilderPhrase(string phrase, SubsetMatchingMode subsetMatchingCriteria)
			: this(phrase, true, subsetMatchingCriteria)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00015338 File Offset: 0x00013538
		private GrammarBuilderPhrase(string phrase, bool subsetMatching, SubsetMatchingMode subsetMatchingCriteria)
		{
			this._phrase = string.Copy(phrase);
			this._subsetMatching = subsetMatching;
			switch (subsetMatchingCriteria)
			{
			case SubsetMatchingMode.Subsequence:
				this._matchMode = MatchMode.Subsequence;
				return;
			case SubsetMatchingMode.OrderedSubset:
				this._matchMode = MatchMode.OrderedSubset;
				return;
			case SubsetMatchingMode.SubsequenceContentRequired:
				this._matchMode = MatchMode.SubsequenceContentRequired;
				return;
			case SubsetMatchingMode.OrderedSubsetContentRequired:
				this._matchMode = MatchMode.OrderedSubsetContentRequired;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00015394 File Offset: 0x00013594
		private GrammarBuilderPhrase(string phrase, bool subsetMatching, MatchMode matchMode)
		{
			this._phrase = string.Copy(phrase);
			this._subsetMatching = subsetMatching;
			this._matchMode = matchMode;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000153B8 File Offset: 0x000135B8
		public override bool Equals(object obj)
		{
			GrammarBuilderPhrase grammarBuilderPhrase = obj as GrammarBuilderPhrase;
			return grammarBuilderPhrase != null && (this._phrase == grammarBuilderPhrase._phrase && this._matchMode == grammarBuilderPhrase._matchMode) && this._subsetMatching == grammarBuilderPhrase._subsetMatching;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00015402 File Offset: 0x00013602
		public override int GetHashCode()
		{
			return this._phrase.GetHashCode();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001540F File Offset: 0x0001360F
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderPhrase(this._phrase, this._subsetMatching, this._matchMode);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00015428 File Offset: 0x00013628
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return this.CreatePhraseElement(elementFactory, parent);
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00015432 File Offset: 0x00013632
		internal override string DebugSummary
		{
			get
			{
				return "‘" + this._phrase + "’";
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001544C File Offset: 0x0001364C
		private IElement CreatePhraseElement(IElementFactory elementFactory, IElement parent)
		{
			if (this._subsetMatching)
			{
				return elementFactory.CreateSubset(parent, this._phrase, this._matchMode);
			}
			if (elementFactory is SrgsElementCompilerFactory)
			{
				XmlParser.ParseText(parent, this._phrase, null, null, -1f, new CreateTokenCallback(elementFactory.CreateToken));
				return null;
			}
			return elementFactory.CreateText(parent, this._phrase);
		}

		// Token: 0x04000450 RID: 1104
		private readonly string _phrase;

		// Token: 0x04000451 RID: 1105
		private readonly bool _subsetMatching;

		// Token: 0x04000452 RID: 1106
		private readonly MatchMode _matchMode;
	}
}
