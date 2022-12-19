using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AB RID: 427
	internal sealed class ItemElement : BuilderElements
	{
		// Token: 0x06000BA8 RID: 2984 RVA: 0x0003140B File Offset: 0x0003040B
		internal ItemElement(GrammarBuilderBase builder)
			: this(builder, 1, 1)
		{
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00031416 File Offset: 0x00030416
		internal ItemElement(int minRepeat, int maxRepeat)
			: this(null, minRepeat, maxRepeat)
		{
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00031421 File Offset: 0x00030421
		internal ItemElement(GrammarBuilderBase builder, int minRepeat, int maxRepeat)
		{
			this._minRepeat = 1;
			this._maxRepeat = 1;
			base..ctor();
			if (builder != null)
			{
				base.Add(builder);
			}
			this._minRepeat = minRepeat;
			this._maxRepeat = maxRepeat;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00031450 File Offset: 0x00030450
		internal ItemElement(List<GrammarBuilderBase> builders, int minRepeat, int maxRepeat)
		{
			this._minRepeat = 1;
			this._maxRepeat = 1;
			base..ctor();
			foreach (GrammarBuilderBase grammarBuilderBase in builders)
			{
				base.Items.Add(grammarBuilderBase);
			}
			this._minRepeat = minRepeat;
			this._maxRepeat = maxRepeat;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000314C8 File Offset: 0x000304C8
		internal ItemElement(GrammarBuilder builders)
		{
			this._minRepeat = 1;
			this._maxRepeat = 1;
			base..ctor();
			foreach (GrammarBuilderBase grammarBuilderBase in builders.InternalBuilder.Items)
			{
				base.Items.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0003153C File Offset: 0x0003053C
		public override bool Equals(object obj)
		{
			ItemElement itemElement = obj as ItemElement;
			return itemElement != null && base.Equals(obj) && this._minRepeat == itemElement._minRepeat && this._maxRepeat == itemElement._maxRepeat;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0003157E File Offset: 0x0003057E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00031588 File Offset: 0x00030588
		internal override GrammarBuilderBase Clone()
		{
			ItemElement itemElement = new ItemElement(this._minRepeat, this._maxRepeat);
			itemElement.CloneItems(this);
			return itemElement;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000315B0 File Offset: 0x000305B0
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IItem item = elementFactory.CreateItem(parent, rule, this._minRepeat, this._maxRepeat, 0.5f, 1f);
			base.CreateChildrenElements(elementFactory, item, rule, ruleIds);
			return item;
		}

		// Token: 0x0400098B RID: 2443
		private readonly int _minRepeat;

		// Token: 0x0400098C RID: 2444
		private readonly int _maxRepeat;
	}
}
