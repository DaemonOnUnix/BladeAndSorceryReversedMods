using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;
using System.Text;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x02000190 RID: 400
	internal abstract class BuilderElements : GrammarBuilderBase
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0002B600 File Offset: 0x0002A600
		internal BuilderElements()
		{
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002B614 File Offset: 0x0002A614
		public override bool Equals(object obj)
		{
			BuilderElements builderElements = obj as BuilderElements;
			if (builderElements == null)
			{
				return false;
			}
			if (builderElements.Count != this.Count || builderElements.Items.Count != this.Items.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (!this.Items[i].Equals(builderElements.Items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002B68D File Offset: 0x0002A68D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0002B698 File Offset: 0x0002A698
		protected void Optimize(Collection<RuleElement> newRules)
		{
			SortedDictionary<int, Collection<BuilderElements>> sortedDictionary = new SortedDictionary<int, Collection<BuilderElements>>();
			this.GetDictionaryElements(sortedDictionary);
			int[] array = new int[sortedDictionary.Keys.Count];
			int num = array.Length - 1;
			foreach (int num2 in sortedDictionary.Keys)
			{
				array[num--] = num2;
			}
			int num3 = 0;
			while (num3 < array.Length && array[num3] >= 3)
			{
				Collection<BuilderElements> collection = sortedDictionary[array[num3]];
				for (int i = 0; i < collection.Count; i++)
				{
					RuleElement ruleElement = null;
					RuleRefElement ruleRefElement = null;
					for (int j = i + 1; j < collection.Count; j++)
					{
						if (collection[i] != null && collection[i].Equals(collection[j]))
						{
							BuilderElements builderElements = collection[j];
							BuilderElements parent = builderElements.Parent;
							if (builderElements is SemanticKeyElement)
							{
								parent.Items[parent.Items.IndexOf(builderElements)] = collection[i];
							}
							else
							{
								if (ruleElement == null)
								{
									ruleElement = new RuleElement(builderElements, "_");
									newRules.Add(ruleElement);
								}
								if (ruleRefElement == null)
								{
									ruleRefElement = new RuleRefElement(ruleElement);
									collection[i].Parent.Items[collection[i].Parent.Items.IndexOf(collection[i])] = ruleRefElement;
								}
								parent.Items[builderElements.Parent.Items.IndexOf(builderElements)] = ruleRefElement;
							}
							builderElements.RemoveDictionaryElements(sortedDictionary);
							collection[j] = null;
						}
					}
				}
				num3++;
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0002B884 File Offset: 0x0002A884
		internal void Add(string phrase)
		{
			this._items.Add(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0002B898 File Offset: 0x0002A898
		internal void Add(GrammarBuilder builder)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builder.InternalBuilder.Items)
			{
				this._items.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0002B8F8 File Offset: 0x0002A8F8
		internal void Add(GrammarBuilderBase item)
		{
			this._items.Add(item);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0002B908 File Offset: 0x0002A908
		internal void CloneItems(BuilderElements builders)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builders.Items)
			{
				this._items.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0002B960 File Offset: 0x0002A960
		internal void CreateChildrenElements(IElementFactory elementFactory, IRule parent, IdentifierCollection ruleIds)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in this.Items)
			{
				IElement element = grammarBuilderBase.CreateElement(elementFactory, parent, parent, ruleIds);
				if (element != null)
				{
					element.PostParse(parent);
					elementFactory.AddElement(parent, element);
				}
			}
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002B9CC File Offset: 0x0002A9CC
		internal void CreateChildrenElements(IElementFactory elementFactory, IItem parent, IRule rule, IdentifierCollection ruleIds)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in this.Items)
			{
				IElement element = grammarBuilderBase.CreateElement(elementFactory, parent, rule, ruleIds);
				if (element != null)
				{
					element.PostParse(parent);
					elementFactory.AddElement(parent, element);
				}
			}
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0002BA38 File Offset: 0x0002AA38
		internal override int CalcCount(BuilderElements parent)
		{
			base.CalcCount(parent);
			int num = 1;
			foreach (GrammarBuilderBase grammarBuilderBase in this.Items)
			{
				num += grammarBuilderBase.CalcCount(this);
			}
			this.Count = num;
			return num;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002BAA0 File Offset: 0x0002AAA0
		internal List<GrammarBuilderBase> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002BAA8 File Offset: 0x0002AAA8
		internal override string DebugSummary
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (GrammarBuilderBase grammarBuilderBase in this._items)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(grammarBuilderBase.DebugSummary);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0002BB24 File Offset: 0x0002AB24
		private void GetDictionaryElements(SortedDictionary<int, Collection<BuilderElements>> dict)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in this.Items)
			{
				BuilderElements builderElements = grammarBuilderBase as BuilderElements;
				if (builderElements != null)
				{
					if (!dict.ContainsKey(builderElements.Count))
					{
						dict.Add(builderElements.Count, new Collection<BuilderElements>());
					}
					dict[builderElements.Count].Add(builderElements);
					builderElements.GetDictionaryElements(dict);
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002BBB4 File Offset: 0x0002ABB4
		private void RemoveDictionaryElements(SortedDictionary<int, Collection<BuilderElements>> dict)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in this.Items)
			{
				BuilderElements builderElements = grammarBuilderBase as BuilderElements;
				if (builderElements != null)
				{
					builderElements.RemoveDictionaryElements(dict);
					dict[builderElements.Count].Remove(builderElements);
				}
			}
		}

		// Token: 0x04000907 RID: 2311
		private readonly List<GrammarBuilderBase> _items = new List<GrammarBuilderBase>();
	}
}
