using System;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AC RID: 428
	internal sealed class OneOfElement : BuilderElements
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x000315E8 File Offset: 0x000305E8
		internal OneOfElement()
		{
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000315F0 File Offset: 0x000305F0
		internal override GrammarBuilderBase Clone()
		{
			OneOfElement oneOfElement = new OneOfElement();
			oneOfElement.CloneItems(this);
			return oneOfElement;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003160C File Offset: 0x0003060C
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IOneOf oneOf = elementFactory.CreateOneOf(parent, rule);
			foreach (GrammarBuilderBase grammarBuilderBase in base.Items)
			{
				ItemElement itemElement = grammarBuilderBase as ItemElement;
				if (itemElement == null)
				{
					itemElement = new ItemElement(grammarBuilderBase);
				}
				IItem item = (IItem)itemElement.CreateElement(elementFactory, oneOf, rule, ruleIds);
				item.PostParse(oneOf);
				elementFactory.AddItem(oneOf, item);
			}
			return oneOf;
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00031694 File Offset: 0x00030694
		internal override string DebugSummary
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (GrammarBuilderBase grammarBuilderBase in base.Items)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(grammarBuilderBase.DebugSummary);
				}
				return "[" + stringBuilder.ToString() + "]";
			}
		}
	}
}
