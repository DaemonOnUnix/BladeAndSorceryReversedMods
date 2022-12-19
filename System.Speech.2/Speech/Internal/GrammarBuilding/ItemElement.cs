using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A1 RID: 161
	internal sealed class ItemElement : BuilderElements
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x0001562B File Offset: 0x0001382B
		internal ItemElement(GrammarBuilderBase builder)
			: this(builder, 1, 1)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00015636 File Offset: 0x00013836
		internal ItemElement(int minRepeat, int maxRepeat)
			: this(null, minRepeat, maxRepeat)
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00015641 File Offset: 0x00013841
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

		// Token: 0x06000559 RID: 1369 RVA: 0x00015670 File Offset: 0x00013870
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

		// Token: 0x0600055A RID: 1370 RVA: 0x000156E8 File Offset: 0x000138E8
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

		// Token: 0x0600055B RID: 1371 RVA: 0x0001575C File Offset: 0x0001395C
		public override bool Equals(object obj)
		{
			ItemElement itemElement = obj as ItemElement;
			return itemElement != null && base.Equals(obj) && this._minRepeat == itemElement._minRepeat && this._maxRepeat == itemElement._maxRepeat;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001579E File Offset: 0x0001399E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000157A8 File Offset: 0x000139A8
		internal override GrammarBuilderBase Clone()
		{
			ItemElement itemElement = new ItemElement(this._minRepeat, this._maxRepeat);
			itemElement.CloneItems(this);
			return itemElement;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000157D0 File Offset: 0x000139D0
		internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
		{
			IItem item = elementFactory.CreateItem(parent, rule, this._minRepeat, this._maxRepeat, 0.5f, 1f);
			base.CreateChildrenElements(elementFactory, item, rule, ruleIds);
			return item;
		}

		// Token: 0x04000455 RID: 1109
		private readonly int _minRepeat;

		// Token: 0x04000456 RID: 1110
		private readonly int _maxRepeat;
	}
}
