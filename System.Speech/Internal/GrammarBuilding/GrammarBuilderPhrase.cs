using System;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001A7 RID: 423
	internal sealed class GrammarBuilderPhrase : GrammarBuilderBase
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x000310F4 File Offset: 0x000300F4
		internal GrammarBuilderPhrase(string phrase)
			: this(phrase, false, SubsetMatchingMode.OrderedSubset)
		{
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x000310FF File Offset: 0x000300FF
		internal GrammarBuilderPhrase(string phrase, SubsetMatchingMode subsetMatchingCriteria)
			: this(phrase, true, subsetMatchingCriteria)
		{
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0003110C File Offset: 0x0003010C
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

		// Token: 0x06000B92 RID: 2962 RVA: 0x0003116A File Offset: 0x0003016A
		private GrammarBuilderPhrase(string phrase, bool subsetMatching, MatchMode matchMode)
		{
			this._phrase = string.Copy(phrase);
			this._subsetMatching = subsetMatching;
			this._matchMode = matchMode;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0003118C File Offset: 0x0003018C
		public override bool Equals(object obj)
		{
			GrammarBuilderPhrase grammarBuilderPhrase = obj as GrammarBuilderPhrase;
			return grammarBuilderPhrase != null && (this._phrase == grammarBuilderPhrase._phrase && this._matchMode == grammarBuilderPhrase._matchMode) && this._subsetMatching == grammarBuilderPhrase._subsetMatching;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000311D6 File Offset: 0x000301D6
		public override int GetHashCode()
		{
			return this._phrase.GetHashCode();
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000311E3 File Offset: 0x000301E3
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderPhrase(this._phrase, this._subsetMatching, this._matchMode);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000311FC File Offset: 0x000301FC
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return this.CreatePhraseElement(elementFactory, parent);
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00031206 File Offset: 0x00030206
		internal override string DebugSummary
		{
			get
			{
				return "‘" + this._phrase + "’";
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00031220 File Offset: 0x00030220
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

		// Token: 0x04000986 RID: 2438
		private readonly string _phrase;

		// Token: 0x04000987 RID: 2439
		private readonly bool _subsetMatching;

		// Token: 0x04000988 RID: 2440
		private readonly MatchMode _matchMode;
	}
}
