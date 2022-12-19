using System;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000115 RID: 277
	internal class SrgsElementFactory : IElementFactory
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x0001FF33 File Offset: 0x0001EF33
		internal SrgsElementFactory(SrgsGrammar grammar)
		{
			this._grammar = grammar;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001FF42 File Offset: 0x0001EF42
		void IElementFactory.RemoveAllRules()
		{
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001FF44 File Offset: 0x0001EF44
		IPropertyTag IElementFactory.CreatePropertyTag(IElement parent)
		{
			return new SrgsNameValueTag();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001FF4B File Offset: 0x0001EF4B
		ISemanticTag IElementFactory.CreateSemanticTag(IElement parent)
		{
			return new SrgsSemanticInterpretationTag();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001FF52 File Offset: 0x0001EF52
		IElementText IElementFactory.CreateText(IElement parent, string value)
		{
			return new SrgsText(value);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001FF5C File Offset: 0x0001EF5C
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

		// Token: 0x0600070A RID: 1802 RVA: 0x00020040 File Offset: 0x0001F040
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

		// Token: 0x0600070B RID: 1803 RVA: 0x00020076 File Offset: 0x0001F076
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri)
		{
			return new SrgsRuleRef(srgsUri);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002007E File Offset: 0x0001F07E
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters)
		{
			return new SrgsRuleRef(semanticKey, parameters, srgsUri);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00020089 File Offset: 0x0001F089
		IOneOf IElementFactory.CreateOneOf(IElement parent, IRule rule)
		{
			return new SrgsOneOf();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00020090 File Offset: 0x0001F090
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

		// Token: 0x0600070F RID: 1807 RVA: 0x000200DC File Offset: 0x0001F0DC
		void IElementFactory.InitSpecialRuleRef(IElement parent, IRuleRef special)
		{
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000200E0 File Offset: 0x0001F0E0
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

		// Token: 0x06000711 RID: 1809 RVA: 0x0002011F File Offset: 0x0001F11F
		string IElementFactory.AddScript(IGrammar grammar, string sRule, string code, string filename, int line)
		{
			return code;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00020124 File Offset: 0x0001F124
		void IElementFactory.AddScript(IGrammar grammar, string script, string filename, int line)
		{
			SrgsGrammar srgsGrammar = (SrgsGrammar)grammar;
			srgsGrammar.AddScript(null, script);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00020140 File Offset: 0x0001F140
		void IElementFactory.AddItem(IOneOf oneOf, IItem value)
		{
			((SrgsOneOf)oneOf).Add((SrgsItem)value);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00020153 File Offset: 0x0001F153
		void IElementFactory.AddElement(IRule rule, IElement value)
		{
			((SrgsRule)rule).Elements.Add((SrgsElement)value);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002016B File Offset: 0x0001F16B
		void IElementFactory.AddElement(IItem item, IElement value)
		{
			((SrgsItem)item).Elements.Add((SrgsElement)value);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00020183 File Offset: 0x0001F183
		IGrammar IElementFactory.Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002018B File Offset: 0x0001F18B
		IRuleRef IElementFactory.Null
		{
			get
			{
				return SrgsRuleRef.Null;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00020192 File Offset: 0x0001F192
		IRuleRef IElementFactory.Void
		{
			get
			{
				return SrgsRuleRef.Void;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00020199 File Offset: 0x0001F199
		IRuleRef IElementFactory.Garbage
		{
			get
			{
				return SrgsRuleRef.Garbage;
			}
		}

		// Token: 0x0400054E RID: 1358
		private SrgsGrammar _grammar;

		// Token: 0x0400054F RID: 1359
		private static readonly char[] _pronSeparator = new char[] { ' ', '\t', '\n', '\r', ';' };
	}
}
