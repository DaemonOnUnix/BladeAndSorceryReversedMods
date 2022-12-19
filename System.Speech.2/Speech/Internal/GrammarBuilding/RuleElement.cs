using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A3 RID: 163
	internal sealed class RuleElement : BuilderElements
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00015944 File Offset: 0x00013B44
		internal RuleElement(string name)
		{
			this._name = name;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00015953 File Offset: 0x00013B53
		internal RuleElement(GrammarBuilderBase builder, string name)
			: this(name)
		{
			base.Add(builder);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00015964 File Offset: 0x00013B64
		public override bool Equals(object obj)
		{
			RuleElement ruleElement = obj as RuleElement;
			return ruleElement != null && base.Equals(obj) && this._name == ruleElement._name;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001579E File Offset: 0x0001399E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001599C File Offset: 0x00013B9C
		internal override GrammarBuilderBase Clone()
		{
			RuleElement ruleElement = new RuleElement(this._name);
			ruleElement.CloneItems(this);
			return ruleElement;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000159C0 File Offset: 0x00013BC0
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			if (this._rule == null)
			{
				IGrammar grammar = elementFactory.Grammar;
				this._ruleName = ruleIds.CreateNewIdentifier(this.Name);
				this._rule = grammar.CreateRule(this._ruleName, RulePublic.False, RuleDynamic.NotSet, false);
				base.CreateChildrenElements(elementFactory, this._rule, ruleIds);
				this._rule.PostParse(grammar);
			}
			return this._rule;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00015A25 File Offset: 0x00013C25
		internal override int CalcCount(BuilderElements parent)
		{
			this._rule = null;
			return base.CalcCount(parent);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00015A35 File Offset: 0x00013C35
		internal override string DebugSummary
		{
			get
			{
				return this._name + "=" + base.DebugSummary;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00015A4D File Offset: 0x00013C4D
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00015A55 File Offset: 0x00013C55
		internal string RuleName
		{
			get
			{
				return this._ruleName;
			}
		}

		// Token: 0x04000457 RID: 1111
		private readonly string _name;

		// Token: 0x04000458 RID: 1112
		private string _ruleName;

		// Token: 0x04000459 RID: 1113
		private IRule _rule;
	}
}
