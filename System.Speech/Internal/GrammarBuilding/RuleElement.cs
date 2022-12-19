using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AD RID: 429
	internal sealed class RuleElement : BuilderElements
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x00031720 File Offset: 0x00030720
		internal RuleElement(string name)
		{
			this._name = name;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0003172F File Offset: 0x0003072F
		internal RuleElement(GrammarBuilderBase builder, string name)
			: this(name)
		{
			base.Add(builder);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00031740 File Offset: 0x00030740
		public override bool Equals(object obj)
		{
			RuleElement ruleElement = obj as RuleElement;
			return ruleElement != null && base.Equals(obj) && this._name == ruleElement._name;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00031775 File Offset: 0x00030775
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00031780 File Offset: 0x00030780
		internal override GrammarBuilderBase Clone()
		{
			RuleElement ruleElement = new RuleElement(this._name);
			ruleElement.CloneItems(this);
			return ruleElement;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000317A4 File Offset: 0x000307A4
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

		// Token: 0x06000BBB RID: 3003 RVA: 0x00031809 File Offset: 0x00030809
		internal override int CalcCount(BuilderElements parent)
		{
			this._rule = null;
			return base.CalcCount(parent);
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00031819 File Offset: 0x00030819
		internal override string DebugSummary
		{
			get
			{
				return this._name + "=" + base.DebugSummary;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00031831 File Offset: 0x00030831
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00031839 File Offset: 0x00030839
		internal string RuleName
		{
			get
			{
				return this._ruleName;
			}
		}

		// Token: 0x0400098D RID: 2445
		private readonly string _name;

		// Token: 0x0400098E RID: 2446
		private string _ruleName;

		// Token: 0x0400098F RID: 2447
		private IRule _rule;
	}
}
