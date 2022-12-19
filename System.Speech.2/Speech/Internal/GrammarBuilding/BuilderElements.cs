using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;
using System.Text;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009A RID: 154
	internal abstract class BuilderElements : GrammarBuilderBase
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00014BAA File Offset: 0x00012DAA
		internal BuilderElements()
		{
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00014BC0 File Offset: 0x00012DC0
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

		// Token: 0x0600051D RID: 1309 RVA: 0x00014C39 File Offset: 0x00012E39
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014C44 File Offset: 0x00012E44
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

		// Token: 0x0600051F RID: 1311 RVA: 0x00014E30 File Offset: 0x00013030
		internal void Add(string phrase)
		{
			this._items.Add(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00014E44 File Offset: 0x00013044
		internal void Add(GrammarBuilder builder)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builder.InternalBuilder.Items)
			{
				this._items.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00014EA4 File Offset: 0x000130A4
		internal void Add(GrammarBuilderBase item)
		{
			this._items.Add(item);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00014EB4 File Offset: 0x000130B4
		internal void CloneItems(BuilderElements builders)
		{
			foreach (GrammarBuilderBase grammarBuilderBase in builders.Items)
			{
				this._items.Add(grammarBuilderBase);
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00014F0C File Offset: 0x0001310C
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

		// Token: 0x06000524 RID: 1316 RVA: 0x00014F78 File Offset: 0x00013178
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

		// Token: 0x06000525 RID: 1317 RVA: 0x00014FE4 File Offset: 0x000131E4
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001504C File Offset: 0x0001324C
		internal List<GrammarBuilderBase> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00015054 File Offset: 0x00013254
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

		// Token: 0x06000528 RID: 1320 RVA: 0x000150D0 File Offset: 0x000132D0
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

		// Token: 0x06000529 RID: 1321 RVA: 0x00015160 File Offset: 0x00013360
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

		// Token: 0x0400044B RID: 1099
		private readonly List<GrammarBuilderBase> _items = new List<GrammarBuilderBase>();
	}
}
