using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F7 RID: 247
	internal class RuleRef : ParseElement, IRuleRef, IElement
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x00027804 File Offset: 0x00025A04
		private RuleRef(RuleRef.SpecialRuleRefType type, Rule rule)
			: base(rule)
		{
			this._type = type;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00027814 File Offset: 0x00025A14
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
					_comType = VarEnum.VT_EMPTY
				});
			}
			parent.AddArc(arc);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00027924 File Offset: 0x00025B24
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

		// Token: 0x060008BA RID: 2234 RVA: 0x000279DC File Offset: 0x00025BDC
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00027A06 File Offset: 0x00025C06
		internal static IRuleRef Null
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Null, null);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00027A0F File Offset: 0x00025C0F
		internal static IRuleRef Void
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Void, null);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00027A18 File Offset: 0x00025C18
		internal static IRuleRef Garbage
		{
			get
			{
				return new RuleRef(RuleRef.SpecialRuleRefType.Garbage, null);
			}
		}

		// Token: 0x04000628 RID: 1576
		private RuleRef.SpecialRuleRefType _type;

		// Token: 0x04000629 RID: 1577
		private const string szSpecialVoid = "VOID";

		// Token: 0x020001A8 RID: 424
		private enum SpecialRuleRefType
		{
			// Token: 0x040009A0 RID: 2464
			Null,
			// Token: 0x040009A1 RID: 2465
			Void,
			// Token: 0x040009A2 RID: 2466
			Garbage
		}
	}
}
