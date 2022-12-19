using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001A9 RID: 425
	internal sealed class GrammarBuilderWildcard : GrammarBuilderBase
	{
		// Token: 0x06000BA0 RID: 2976 RVA: 0x0003133A File Offset: 0x0003033A
		internal GrammarBuilderWildcard()
		{
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00031344 File Offset: 0x00030344
		public override bool Equals(object obj)
		{
			GrammarBuilderWildcard grammarBuilderWildcard = obj as GrammarBuilderWildcard;
			return grammarBuilderWildcard != null;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0003135F File Offset: 0x0003035F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00031367 File Offset: 0x00030367
		internal override GrammarBuilderBase Clone()
		{
			return new GrammarBuilderWildcard();
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00031370 File Offset: 0x00030370
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IRuleRef garbage = elementFactory.Garbage;
			elementFactory.InitSpecialRuleRef(parent, garbage);
			return garbage;
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0003138D File Offset: 0x0003038D
		internal override string DebugSummary
		{
			get
			{
				return "*";
			}
		}
	}
}
