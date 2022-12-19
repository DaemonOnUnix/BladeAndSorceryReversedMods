using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200007D RID: 125
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[DebuggerTypeProxy(typeof(SrgsOneOf.OneOfDebugDisplay))]
	[Serializable]
	public class SrgsOneOf : SrgsElement, IOneOf, IElement
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x00010E91 File Offset: 0x0000F091
		public SrgsOneOf()
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010EA4 File Offset: 0x0000F0A4
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

		// Token: 0x0600042F RID: 1071 RVA: 0x00010F00 File Offset: 0x0000F100
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

		// Token: 0x06000430 RID: 1072 RVA: 0x00010F56 File Offset: 0x0000F156
		public void Add(SrgsItem item)
		{
			Helpers.ThrowIfNull(item, "item");
			this.Items.Add(item);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00010F6F File Offset: 0x0000F16F
		public Collection<SrgsItem> Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00010F78 File Offset: 0x0000F178
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("one-of");
			foreach (SrgsItem srgsItem in this._items)
			{
				srgsItem.WriteSrgs(writer);
			}
			writer.WriteEndElement();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00010FD8 File Offset: 0x0000F1D8
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsOneOf Count = ");
			stringBuilder.Append(this._items.Count);
			return stringBuilder.ToString();
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00011008 File Offset: 0x0000F208
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

		// Token: 0x040003EA RID: 1002
		private SrgsItemList _items = new SrgsItemList();

		// Token: 0x02000180 RID: 384
		internal class OneOfDebugDisplay
		{
			// Token: 0x06000B5D RID: 2909 RVA: 0x0002D71C File Offset: 0x0002B91C
			public OneOfDebugDisplay(SrgsOneOf oneOf)
			{
				this._items = oneOf._items;
			}

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0002D730 File Offset: 0x0002B930
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
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

			// Token: 0x040008B7 RID: 2231
			private Collection<SrgsItem> _items;
		}
	}
}
