using System;
using System.Diagnostics;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A4 RID: 164
	[DebuggerDisplay("{DebugSummary}")]
	internal sealed class RuleRefElement : GrammarBuilderBase
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00015A5D File Offset: 0x00013C5D
		internal RuleRefElement(RuleElement rule)
		{
			this._rule = rule;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00015A6C File Offset: 0x00013C6C
		internal RuleRefElement(RuleElement rule, string semanticKey)
		{
			this._rule = rule;
			this._semanticKey = semanticKey;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00015A84 File Offset: 0x00013C84
		public override bool Equals(object obj)
		{
			RuleRefElement ruleRefElement = obj as RuleRefElement;
			return ruleRefElement != null && this._semanticKey == ruleRefElement._semanticKey && this._rule.Equals(ruleRefElement._rule);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00014C39 File Offset: 0x00012E39
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00015AC3 File Offset: 0x00013CC3
		internal void Add(GrammarBuilderBase item)
		{
			this._rule.Add(item);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00015AD1 File Offset: 0x00013CD1
		internal override GrammarBuilderBase Clone()
		{
			return new RuleRefElement(this._rule, this._semanticKey);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00015AE4 File Offset: 0x00013CE4
		internal void CloneItems(RuleRefElement builders)
		{
			this._rule.CloneItems(builders._rule);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00015AF7 File Offset: 0x00013CF7
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			return elementFactory.CreateRuleRef(parent, new Uri("#" + this.Rule.RuleName, UriKind.Relative), this._semanticKey, null);
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00015B22 File Offset: 0x00013D22
		internal RuleElement Rule
		{
			get
			{
				return this._rule;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00015B2A File Offset: 0x00013D2A
		internal override string DebugSummary
		{
			get
			{
				return "#" + this.Rule.Name + ((this._semanticKey != null) ? (":" + this._semanticKey) : "");
			}
		}

		// Token: 0x0400045A RID: 1114
		private readonly RuleElement _rule;

		// Token: 0x0400045B RID: 1115
		private readonly string _semanticKey;
	}
}
