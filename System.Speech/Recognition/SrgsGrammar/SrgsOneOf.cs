using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200011D RID: 285
	[DebuggerTypeProxy(typeof(SrgsOneOf.OneOfDebugDisplay))]
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsOneOf : SrgsElement, IOneOf, IElement
	{
		// Token: 0x0600077C RID: 1916 RVA: 0x00021783 File Offset: 0x00020783
		public SrgsOneOf()
		{
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00021798 File Offset: 0x00020798
		public SrgsOneOf(params string[] items)
			: this()
		{
			Helpers.ThrowIfNull(items, "items");
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] == null)
				{
					throw new ArgumentNullException("items", SR.Get(SRID.ParamsEntryNullIllegal, new object[0]));
				}
				this._items.Add(new SrgsItem(items[i]));
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000217F4 File Offset: 0x000207F4
		public SrgsOneOf(params SrgsItem[] items)
			: this()
		{
			Helpers.ThrowIfNull(items, "items");
			foreach (SrgsItem srgsItem in items)
			{
				if (srgsItem == null)
				{
					throw new ArgumentNullException("items", SR.Get(SRID.ParamsEntryNullIllegal, new object[0]));
				}
				this._items.Add(srgsItem);
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002184A File Offset: 0x0002084A
		public void Add(SrgsItem item)
		{
			Helpers.ThrowIfNull(item, "item");
			this.Items.Add(item);
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00021863 File Offset: 0x00020863
		public Collection<SrgsItem> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002186C File Offset: 0x0002086C
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("one-of");
			foreach (SrgsItem srgsItem in this._items)
			{
				srgsItem.WriteSrgs(writer);
			}
			writer.WriteEndElement();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000218CC File Offset: 0x000208CC
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsOneOf Count = ");
			stringBuilder.Append(this._items.Count);
			return stringBuilder.ToString();
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000218FC File Offset: 0x000208FC
		internal override SrgsElement[] Children
		{
			get
			{
				SrgsElement[] array = new SrgsElement[this._items.Count];
				int num = 0;
				foreach (SrgsItem srgsItem in this._items)
				{
					array[num++] = srgsItem;
				}
				return array;
			}
		}

		// Token: 0x04000573 RID: 1395
		private SrgsItemList _items = new SrgsItemList();

		// Token: 0x0200011E RID: 286
		internal class OneOfDebugDisplay
		{
			// Token: 0x06000784 RID: 1924 RVA: 0x00021960 File Offset: 0x00020960
			public OneOfDebugDisplay(SrgsOneOf oneOf)
			{
				this._items = oneOf._items;
			}

			// Token: 0x1700012B RID: 299
			// (get) Token: 0x06000785 RID: 1925 RVA: 0x00021974 File Offset: 0x00020974
			[DebuggerBrowsable(3)]
			public SrgsItem[] AKeys
			{
				get
				{
					SrgsItem[] array = new SrgsItem[this._items.Count];
					for (int i = 0; i < this._items.Count; i++)
					{
						array[i] = this._items[i];
					}
					return array;
				}
			}

			// Token: 0x04000574 RID: 1396
			private Collection<SrgsItem> _items;
		}
	}
}
