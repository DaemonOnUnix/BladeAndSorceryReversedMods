using System;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A2 RID: 162
	internal sealed class OneOfElement : BuilderElements
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x00015808 File Offset: 0x00013A08
		internal OneOfElement()
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00015810 File Offset: 0x00013A10
		internal override GrammarBuilderBase Clone()
		{
			OneOfElement oneOfElement = new OneOfElement();
			oneOfElement.CloneItems(this);
			return oneOfElement;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001582C File Offset: 0x00013A2C
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

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000158B8 File Offset: 0x00013AB8
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
