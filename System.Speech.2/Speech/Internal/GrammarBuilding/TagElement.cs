using System;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A6 RID: 166
	internal sealed class TagElement : BuilderElements
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x00015CCB File Offset: 0x00013ECB
		internal TagElement(object value)
		{
			this._value = value;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00015CDA File Offset: 0x00013EDA
		internal TagElement(GrammarBuilderBase builder, object value)
			: this(value)
		{
			base.Add(builder);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00015CEA File Offset: 0x00013EEA
		internal TagElement(GrammarBuilder builder, object value)
			: this(value)
		{
			base.Add(builder);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00015CFC File Offset: 0x00013EFC
		public override bool Equals(object obj)
		{
			TagElement tagElement = obj as TagElement;
			return tagElement != null && base.Equals(obj) && this._value.Equals(tagElement._value);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001579E File Offset: 0x0001399E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00015D34 File Offset: 0x00013F34
		internal override GrammarBuilderBase Clone()
		{
			TagElement tagElement = new TagElement(this._value);
			tagElement.CloneItems(this);
			return tagElement;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00015D58 File Offset: 0x00013F58
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IItem item = parent as IItem;
			if (item != null)
			{
				base.CreateChildrenElements(elementFactory, item, rule, ruleIds);
			}
			else if (parent == rule)
			{
				base.CreateChildrenElements(elementFactory, rule, ruleIds);
			}
			IPropertyTag propertyTag = elementFactory.CreatePropertyTag(parent);
			propertyTag.NameValue(parent, null, this._value);
			return propertyTag;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00015DA1 File Offset: 0x00013FA1
		internal override string DebugSummary
		{
			get
			{
				return string.Concat(new object[] { base.DebugSummary, " {", this._value, "}" });
			}
		}

		// Token: 0x0400045E RID: 1118
		private readonly object _value;
	}
}
