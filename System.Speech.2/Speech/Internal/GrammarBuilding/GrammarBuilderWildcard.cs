using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009F RID: 159
	internal sealed class GrammarBuilderWildcard : GrammarBuilderBase
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00015566 File Offset: 0x00013766
		internal GrammarBuilderWildcard()
		{
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00015570 File Offset: 0x00013770
		public override bool Equals(object obj)
		{
			GrammarBuilderWildcard grammarBuilderWildcard = obj as GrammarBuilderWildcard;
			return grammarBuilderWildcard != null;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00014C39 File Offset: 0x00012E39
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00015588 File Offset: 0x00013788
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderWildcard();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00015590 File Offset: 0x00013790
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IRuleRef garbage = elementFactory.Garbage;
			elementFactory.InitSpecialRuleRef(parent, garbage);
			return garbage;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000155AD File Offset: 0x000137AD
		internal override string DebugSummary
		{
			get
			{
				return "*";
			}
		}
	}
}
