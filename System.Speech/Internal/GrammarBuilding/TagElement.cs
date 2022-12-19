using System;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001B0 RID: 432
	internal sealed class TagElement : BuilderElements
	{
		// Token: 0x06000BD1 RID: 3025 RVA: 0x00031ABF File Offset: 0x00030ABF
		internal TagElement(object value)
		{
			this._value = value;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00031ACE File Offset: 0x00030ACE
		internal TagElement(GrammarBuilderBase builder, object value)
			: this(value)
		{
			base.Add(builder);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00031ADE File Offset: 0x00030ADE
		internal TagElement(GrammarBuilder builder, object value)
			: this(value)
		{
			base.Add(builder);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00031AF0 File Offset: 0x00030AF0
		public override bool Equals(object obj)
		{
			TagElement tagElement = obj as TagElement;
			return tagElement != null && base.Equals(obj) && this._value.Equals(tagElement._value);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00031B25 File Offset: 0x00030B25
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00031B30 File Offset: 0x00030B30
		internal override GrammarBuilderBase Clone()
		{
			TagElement tagElement = new TagElement(this._value);
			tagElement.CloneItems(this);
			return tagElement;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00031B54 File Offset: 0x00030B54
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00031BA0 File Offset: 0x00030BA0
		internal override string DebugSummary
		{
			get
			{
				return string.Concat(new object[] { base.DebugSummary, " {", this._value, "}" });
			}
		}

		// Token: 0x04000994 RID: 2452
		private readonly object _value;
	}
}
