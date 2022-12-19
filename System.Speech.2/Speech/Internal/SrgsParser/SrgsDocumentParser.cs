using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000E3 RID: 227
	internal class SrgsDocumentParser : ISrgsParser
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
		internal SrgsDocumentParser(SrgsGrammar grammar)
		{
			this._grammar = grammar;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		public void Parse()
		{
			try
			{
				this.ProcessGrammarElement(this._grammar, this._parser.Grammar);
			}
			catch
			{
				this._parser.RemoveAllRules();
				throw;
			}
		}

		// Token: 0x1700018E RID: 398
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0001F60C File Offset: 0x0001D80C
		public IElementFactory ElementFactory
		{
			set
			{
				this._parser = value;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001F618 File Offset: 0x0001D818
		private void ProcessGrammarElement(SrgsGrammar source, IGrammar grammar)
		{
			grammar.Culture = source.Culture;
			grammar.Mode = source.Mode;
			if (source.Root != null)
			{
				grammar.Root = source.Root.Id;
			}
			grammar.TagFormat = source.TagFormat;
			grammar.XmlBase = source.XmlBase;
			grammar.GlobalTags = source.GlobalTags;
			grammar.PhoneticAlphabet = source.PhoneticAlphabet;
			foreach (SrgsRule srgsRule in source.Rules)
			{
				IRule rule = this.ParseRule(grammar, srgsRule);
				rule.PostParse(grammar);
			}
			grammar.AssemblyReferences = source.AssemblyReferences;
			grammar.CodeBehind = source.CodeBehind;
			grammar.Debug = source.Debug;
			grammar.ImportNamespaces = source.ImportNamespaces;
			grammar.Language = ((source.Language == null) ? "C#" : source.Language);
			grammar.Namespace = source.Namespace;
			this._parser.AddScript(grammar, source.Script, null, -1);
			grammar.PostParse(null);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001F744 File Offset: 0x0001D944
		private IRule ParseRule(IGrammar grammar, SrgsRule srgsRule)
		{
			string id = srgsRule.Id;
			bool flag = srgsRule.OnInit != null || srgsRule.OnParse != null || srgsRule.OnError != null || srgsRule.OnRecognition != null;
			IRule rule = grammar.CreateRule(id, (srgsRule.Scope == SrgsRuleScope.Public) ? RulePublic.True : RulePublic.False, srgsRule.Dynamic, flag);
			if (srgsRule.OnInit != null)
			{
				rule.CreateScript(grammar, id, srgsRule.OnInit, RuleMethodScript.onInit);
			}
			if (srgsRule.OnParse != null)
			{
				rule.CreateScript(grammar, id, srgsRule.OnParse, RuleMethodScript.onParse);
			}
			if (srgsRule.OnError != null)
			{
				rule.CreateScript(grammar, id, srgsRule.OnError, RuleMethodScript.onError);
			}
			if (srgsRule.OnRecognition != null)
			{
				rule.CreateScript(grammar, id, srgsRule.OnRecognition, RuleMethodScript.onRecognition);
			}
			if (srgsRule.Script.Length > 0)
			{
				this._parser.AddScript(grammar, id, srgsRule.Script);
			}
			rule.BaseClass = srgsRule.BaseClass;
			foreach (SrgsElement srgsElement in this.GetSortedTagElements(srgsRule.Elements))
			{
				this.ProcessChildNodes(srgsElement, rule, rule);
			}
			return rule;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001F86C File Offset: 0x0001DA6C
		private IRuleRef ParseRuleRef(SrgsRuleRef srgsRuleRef, IElement parent)
		{
			bool flag = true;
			IRuleRef ruleRef;
			if (srgsRuleRef == SrgsRuleRef.Null)
			{
				ruleRef = this._parser.Null;
			}
			else if (srgsRuleRef == SrgsRuleRef.Void)
			{
				ruleRef = this._parser.Void;
			}
			else if (srgsRuleRef == SrgsRuleRef.Garbage)
			{
				ruleRef = this._parser.Garbage;
			}
			else
			{
				ruleRef = this._parser.CreateRuleRef(parent, srgsRuleRef.Uri, srgsRuleRef.SemanticKey, srgsRuleRef.Params);
				flag = false;
			}
			if (flag)
			{
				this._parser.InitSpecialRuleRef(parent, ruleRef);
			}
			ruleRef.PostParse(parent);
			return ruleRef;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001F8F8 File Offset: 0x0001DAF8
		private IOneOf ParseOneOf(SrgsOneOf srgsOneOf, IElement parent, IRule rule)
		{
			IOneOf oneOf = this._parser.CreateOneOf(parent, rule);
			foreach (SrgsItem srgsItem in srgsOneOf.Items)
			{
				this.ProcessChildNodes(srgsItem, oneOf, rule);
			}
			oneOf.PostParse(parent);
			return oneOf;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001F960 File Offset: 0x0001DB60
		private IItem ParseItem(SrgsItem srgsItem, IElement parent, IRule rule)
		{
			IItem item = this._parser.CreateItem(parent, rule, srgsItem.MinRepeat, srgsItem.MaxRepeat, srgsItem.RepeatProbability, srgsItem.Weight);
			foreach (SrgsElement srgsElement in this.GetSortedTagElements(srgsItem.Elements))
			{
				this.ProcessChildNodes(srgsElement, item, rule);
			}
			item.PostParse(parent);
			return item;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001F9E4 File Offset: 0x0001DBE4
		private IToken ParseToken(SrgsToken srgsToken, IElement parent)
		{
			return this._parser.CreateToken(parent, srgsToken.Text, srgsToken.Pronunciation, srgsToken.Display, -1f);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001FA09 File Offset: 0x0001DC09
		private void ParseText(IElement parent, string sChars, string pronunciation, string display, float reqConfidence)
		{
			XmlParser.ParseText(parent, sChars, pronunciation, display, reqConfidence, new CreateTokenCallback(this._parser.CreateToken));
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001FA2C File Offset: 0x0001DC2C
		private ISubset ParseSubset(SrgsSubset srgsSubset, IElement parent)
		{
			MatchMode matchMode = MatchMode.Subsequence;
			switch (srgsSubset.MatchingMode)
			{
			case SubsetMatchingMode.Subsequence:
				matchMode = MatchMode.Subsequence;
				break;
			case SubsetMatchingMode.OrderedSubset:
				matchMode = MatchMode.OrderedSubset;
				break;
			case SubsetMatchingMode.SubsequenceContentRequired:
				matchMode = MatchMode.SubsequenceContentRequired;
				break;
			case SubsetMatchingMode.OrderedSubsetContentRequired:
				matchMode = MatchMode.OrderedSubsetContentRequired;
				break;
			}
			return this._parser.CreateSubset(parent, srgsSubset.Text, matchMode);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001FA7C File Offset: 0x0001DC7C
		private ISemanticTag ParseSemanticTag(SrgsSemanticInterpretationTag srgsTag, IElement parent)
		{
			ISemanticTag semanticTag = this._parser.CreateSemanticTag(parent);
			semanticTag.Content(parent, srgsTag.Script, 0);
			semanticTag.PostParse(parent);
			return semanticTag;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001FAAC File Offset: 0x0001DCAC
		private IPropertyTag ParseNameValueTag(SrgsNameValueTag srgsTag, IElement parent)
		{
			IPropertyTag propertyTag = this._parser.CreatePropertyTag(parent);
			propertyTag.NameValue(parent, srgsTag.Name, srgsTag.Value);
			propertyTag.PostParse(parent);
			return propertyTag;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
		private void ProcessChildNodes(SrgsElement srgsElement, IElement parent, IRule rule)
		{
			Type type = srgsElement.GetType();
			IElement element = null;
			IRule rule2 = parent as IRule;
			IItem item = parent as IItem;
			if (type == typeof(SrgsRuleRef))
			{
				element = this.ParseRuleRef((SrgsRuleRef)srgsElement, parent);
			}
			else if (type == typeof(SrgsOneOf))
			{
				element = this.ParseOneOf((SrgsOneOf)srgsElement, parent, rule);
			}
			else if (type == typeof(SrgsItem))
			{
				element = this.ParseItem((SrgsItem)srgsElement, parent, rule);
			}
			else if (type == typeof(SrgsToken))
			{
				element = this.ParseToken((SrgsToken)srgsElement, parent);
			}
			else if (type == typeof(SrgsNameValueTag))
			{
				element = this.ParseNameValueTag((SrgsNameValueTag)srgsElement, parent);
			}
			else if (type == typeof(SrgsSemanticInterpretationTag))
			{
				element = this.ParseSemanticTag((SrgsSemanticInterpretationTag)srgsElement, parent);
			}
			else if (type == typeof(SrgsSubset))
			{
				element = this.ParseSubset((SrgsSubset)srgsElement, parent);
			}
			else if (type == typeof(SrgsText))
			{
				SrgsText srgsText = (SrgsText)srgsElement;
				string text = srgsText.Text;
				IElementText elementText = this._parser.CreateText(parent, text);
				this.ParseText(parent, text, null, null, -1f);
				if (rule2 != null)
				{
					this._parser.AddElement(rule2, elementText);
				}
				else if (item != null)
				{
					this._parser.AddElement(item, elementText);
				}
				else
				{
					XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[0]);
				}
			}
			else
			{
				XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[0]);
			}
			IOneOf oneOf = parent as IOneOf;
			if (oneOf != null)
			{
				IItem item2 = element as IItem;
				if (item2 != null)
				{
					this._parser.AddItem(oneOf, item2);
					return;
				}
				XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[0]);
				return;
			}
			else
			{
				if (rule2 != null)
				{
					this._parser.AddElement(rule2, element);
					return;
				}
				if (item != null)
				{
					this._parser.AddElement(item, element);
					return;
				}
				XmlParser.ThrowSrgsException(SRID.InvalidElement, new object[0]);
				return;
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001FD04 File Offset: 0x0001DF04
		private IEnumerable<SrgsElement> GetSortedTagElements(Collection<SrgsElement> elements)
		{
			if (this._grammar.TagFormat == SrgsTagFormat.KeyValuePairs)
			{
				List<SrgsElement> list = new List<SrgsElement>();
				foreach (SrgsElement srgsElement in elements)
				{
					if (!(srgsElement is SrgsNameValueTag))
					{
						list.Add(srgsElement);
					}
				}
				foreach (SrgsElement srgsElement2 in elements)
				{
					if (srgsElement2 is SrgsNameValueTag)
					{
						list.Add(srgsElement2);
					}
				}
				return list;
			}
			return elements;
		}

		// Token: 0x0400059F RID: 1439
		private SrgsGrammar _grammar;

		// Token: 0x040005A0 RID: 1440
		private IElementFactory _parser;
	}
}
