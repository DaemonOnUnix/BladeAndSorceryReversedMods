using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000B4 RID: 180
	internal class RuleRef : ParseElement, IRuleRef, IElement
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000FB14 File Offset: 0x0000EB14
		private RuleRef(RuleRef.SpecialRuleRefType type, Rule rule)
			: base(rule)
		{
			this._type = type;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000FB24 File Offset: 0x0000EB24
		internal RuleRef(ParseElementCollection parent, Backend backend, Uri uri, List<Rule> undefRules, string semanticKey, string initParameters)
			: base(parent._rule)
		{
			string originalString = uri.OriginalString;
			int num = originalString.IndexOf('#');
			Rule rule;
			if (num == 0)
			{
				rule = RuleRef.GetRuleRef(backend, originalString.Substring(1), undefRules);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder("URL:");
				if (!string.IsNullOrEmpty(initParameters))
				{
					stringBuilder.Append((num > 0) ? originalString.Substring(0, num) : originalString);
					stringBuilder.Append('>');
					stringBuilder.Append(initParameters);
					if (num > 0)
					{
						stringBuilder.Append(originalString.Substring(num));
					}
				}
				else
				{
					stringBuilder.Append(originalString);
				}
				string text = stringBuilder.ToString();
				rule = backend.FindRule(text);
				if (rule == null)
				{
					rule = backend.CreateRule(text, SPCFGRULEATTRIBUTES.SPRAF_Import);
				}
			}
			Arc arc = backend.RuleTransition(rule, this._rule, 1f);
			if (!string.IsNullOrEmpty(semanticKey))
			{
				backend.AddPropertyTag(arc, arc, new CfgGrammar.CfgProperty
				{
					_pszName = "SemanticKey",
					_comValue = semanticKey,
					_comType = 0
				});
			}
			parent.AddArc(arc);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000FC30 File Offset: 0x0000EC30
		internal void InitSpecialRuleRef(Backend backend, ParseElementCollection parent)
		{
			switch (this._type)
			{
			case RuleRef.SpecialRuleRefType.Null:
				parent.AddArc(backend.EpsilonTransition(1f));
				return;
			case RuleRef.SpecialRuleRefType.Void:
			{
				Rule rule = backend.FindRule("VOID");
				if (rule == null)
				{
					rule = backend.CreateRule("VOID", (SPCFGRULEATTRIBUTES)0);
					((IElement)rule).PostParse(parent);
				}
				parent.AddArc(backend.RuleTransition(rule, parent._rule, 1f));
				return;
			}
			case RuleRef.SpecialRuleRefType.Garbage:
			{
				OneOf oneOf = new OneOf(parent._rule, backend);
				oneOf.AddArc(backend.RuleTransition(CfgGrammar.SPRULETRANS_WILDCARD, parent._rule, 0.5f));
				oneOf.AddArc(backend.EpsilonTransition(0.5f));
				((IElement)oneOf).PostParse(parent);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000FCE8 File Offset: 0x0000ECE8
		private static Rule GetRuleRef(Backend backend, string sRuleId, List<Rule> undefRules)
		{
			Rule rule = backend.FindRule(sRuleId);
			if (rule == null)
			{
				rule = backend.CreateRule(sRuleId, (SPCFGRULEATTRIBUTES)0);
				undefRules.Insert(0, rule);
			}
			return rule;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000FD12 File Offset: 0x0000ED12
		internal static IRuleRef Null
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Null, null);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000FD1B File Offset: 0x0000ED1B
		internal static IRuleRef Void
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Void, null);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000FD24 File Offset: 0x0000ED24
		internal static IRuleRef Garbage
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Garbage, null);
			}
		}

		// Token: 0x0400037A RID: 890
		private const string szSpecialVoid = "VOID";

		// Token: 0x0400037B RID: 891
		private RuleRef.SpecialRuleRefType _type;

		// Token: 0x020000B5 RID: 181
		private enum SpecialRuleRefType
		{
			// Token: 0x0400037D RID: 893
			Null,
			// Token: 0x0400037E RID: 894
			Void,
			// Token: 0x0400037F RID: 895
			Garbage
		}
	}
}
