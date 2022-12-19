using System;
using System.Diagnostics;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AE RID: 430
	[DebuggerDisplay("{DebugSummary}")]
	internal sealed class RuleRefElement : GrammarBuilderBase
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x00031841 File Offset: 0x00030841
		internal RuleRefElement(RuleElement rule)
		{
			this._rule = rule;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00031850 File Offset: 0x00030850
		internal RuleRefElement(RuleElement rule, string semanticKey)
		{
			this._rule = rule;
			this._semanticKey = semanticKey;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00031868 File Offset: 0x00030868
		public override bool Equals(object obj)
		{
			RuleRefElement ruleRefElement = obj as RuleRefElement;
			return ruleRefElement != null && this._semanticKey == ruleRefElement._semanticKey && this._rule.Equals(ruleRefElement._rule);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000318A7 File Offset: 0x000308A7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000318AF File Offset: 0x000308AF
		internal void Add(GrammarBuilderBase item)
		{
			this._rule.Add(item);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000318BD File Offset: 0x000308BD
		internal override GrammarBuilderBase Clone()
		{
			return new RuleRefElement(this._rule, this._semanticKey);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000318D0 File Offset: 0x000308D0
		internal void CloneItems(RuleRefElement builders)
		{
			this._rule.CloneItems(builders._rule);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000318E3 File Offset: 0x000308E3
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return elementFactory.CreateRuleRef(parent, new Uri("#" + this.Rule.RuleName, 2), this._semanticKey, null);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0003190E File Offset: 0x0003090E
		internal RuleElement Rule
		{
			get
			{
				return this._rule;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00031916 File Offset: 0x00030916
		internal override string DebugSummary
		{
			get
			{
				return "#" + this.Rule.Name + ((this._semanticKey != null) ? (":" + this._semanticKey) : "");
			}
		}

		// Token: 0x04000990 RID: 2448
		private readonly RuleElement _rule;

		// Token: 0x04000991 RID: 2449
		private readonly string _semanticKey;
	}
}
