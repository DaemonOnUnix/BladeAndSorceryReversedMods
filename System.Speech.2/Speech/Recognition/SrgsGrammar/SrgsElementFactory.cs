using System;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000076 RID: 118
	internal class SrgsElementFactory : IElementFactory
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0000F7CD File Offset: 0x0000D9CD
		internal SrgsElementFactory(SrgsGrammar grammar)
		{
			this._grammar = grammar;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.RemoveAllRules()
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		IPropertyTag IElementFactory.CreatePropertyTag(IElement parent)
		{
			return new SrgsNameValueTag();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F7E3 File Offset: 0x0000D9E3
		ISemanticTag IElementFactory.CreateSemanticTag(IElement parent)
		{
			return new SrgsSemanticInterpretationTag();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F7EA File Offset: 0x0000D9EA
		IElementText IElementFactory.CreateText(IElement parent, string value)
		{
			return new SrgsText(value);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F7F4 File Offset: 0x0000D9F4
		IToken IElementFactory.CreateToken(IElement parent, string content, string pronunciation, string display, float reqConfidence)
		{
			SrgsToken srgsToken = new SrgsToken(content);
			if (!string.IsNullOrEmpty(pronunciation))
			{
				int num;
				for (int i = 0; i < pronunciation.Length; i = num + 1)
				{
					num = pronunciation.IndexOfAny(SrgsElementFactory._pronSeparator, i);
					if (num == -1)
					{
						num = pronunciation.Length;
					}
					string text = pronunciation.Substring(i, num - i);
					switch (this._grammar.PhoneticAlphabet)
					{
					case AlphabetType.Sapi:
						text = PhonemeConverter.ConvertPronToId(text, this._grammar.Culture.LCID);
						break;
					case AlphabetType.Ipa:
						PhonemeConverter.ValidateUpsIds(text);
						break;
					case AlphabetType.Ups:
						text = PhonemeConverter.UpsConverter.ConvertPronToId(text);
						break;
					}
				}
				srgsToken.Pronunciation = pronunciation;
			}
			if (!string.IsNullOrEmpty(display))
			{
				srgsToken.Display = display;
			}
			if (reqConfidence >= 0f)
			{
				throw new NotSupportedException(SR.Get(SRID.ReqConfidenceNotSupported, new object[0]));
			}
			return srgsToken;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		IItem IElementFactory.CreateItem(IElement parent, IRule rule, int minRepeat, int maxRepeat, float repeatProbability, float weight)
		{
			SrgsItem srgsItem = new SrgsItem();
			if (minRepeat != 1 || maxRepeat != 1)
			{
				srgsItem.SetRepeat(minRepeat, maxRepeat);
			}
			srgsItem.RepeatProbability = repeatProbability;
			srgsItem.Weight = weight;
			return srgsItem;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000F90E File Offset: 0x0000DB0E
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri)
		{
			return new SrgsRuleRef(srgsUri);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F916 File Offset: 0x0000DB16
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters)
		{
			return new SrgsRuleRef(semanticKey, parameters, srgsUri);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000F921 File Offset: 0x0000DB21
		IOneOf IElementFactory.CreateOneOf(IElement parent, IRule rule)
		{
			return new SrgsOneOf();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F928 File Offset: 0x0000DB28
		ISubset IElementFactory.CreateSubset(IElement parent, string text, MatchMode matchMode)
		{
			SubsetMatchingMode subsetMatchingMode = SubsetMatchingMode.Subsequence;
			switch (matchMode)
			{
			case MatchMode.Subsequence:
				subsetMatchingMode = SubsetMatchingMode.Subsequence;
				break;
			case MatchMode.OrderedSubset:
				subsetMatchingMode = SubsetMatchingMode.OrderedSubset;
				break;
			case MatchMode.SubsequenceContentRequired:
				subsetMatchingMode = SubsetMatchingMode.SubsequenceContentRequired;
				break;
			case MatchMode.OrderedSubsetContentRequired:
				subsetMatchingMode = SubsetMatchingMode.OrderedSubsetContentRequired;
				break;
			}
			return new SrgsSubset(text, subsetMatchingMode);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.InitSpecialRuleRef(IElement parent, IRuleRef special)
		{
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F974 File Offset: 0x0000DB74
		void IElementFactory.AddScript(IGrammar grammar, string sRule, string code)
		{
			SrgsGrammar srgsGrammar = (SrgsGrammar)grammar;
			SrgsRule srgsRule = srgsGrammar.Rules[sRule];
			if (srgsRule != null)
			{
				srgsRule.Script += code;
				return;
			}
			srgsGrammar.AddScript(sRule, code);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F9B3 File Offset: 0x0000DBB3
		string IElementFactory.AddScript(IGrammar grammar, string sRule, string code, string filename, int line)
		{
			return code;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
		void IElementFactory.AddScript(IGrammar grammar, string script, string filename, int line)
		{
			SrgsGrammar srgsGrammar = (SrgsGrammar)grammar;
			srgsGrammar.AddScript(null, script);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		void IElementFactory.AddItem(IOneOf oneOf, IItem value)
		{
			((SrgsOneOf)oneOf).Add((SrgsItem)value);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000F9E7 File Offset: 0x0000DBE7
		void IElementFactory.AddElement(IRule rule, IElement value)
		{
			((SrgsRule)rule).Elements.Add((SrgsElement)value);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000F9FF File Offset: 0x0000DBFF
		void IElementFactory.AddElement(IItem item, IElement value)
		{
			((SrgsItem)item).Elements.Add((SrgsElement)value);
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000FA17 File Offset: 0x0000DC17
		IGrammar IElementFactory.Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000FA1F File Offset: 0x0000DC1F
		IRuleRef IElementFactory.Null
		{
			get
			{
				return SrgsRuleRef.Null;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000FA26 File Offset: 0x0000DC26
		IRuleRef IElementFactory.Void
		{
			get
			{
				return SrgsRuleRef.Void;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000FA2D File Offset: 0x0000DC2D
		IRuleRef IElementFactory.Garbage
		{
			get
			{
				return SrgsRuleRef.Garbage;
			}
		}

		// Token: 0x040003CA RID: 970
		private SrgsGrammar _grammar;

		// Token: 0x040003CB RID: 971
		private static readonly char[] _pronSeparator = new char[] { ' ', '\t', '\n', '\r', ';' };
	}
}
