using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000119 RID: 281
	[DebuggerTypeProxy(typeof(SrgsItem.SrgsItemDebugDisplay))]
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsItem : SrgsElement, IItem, IElement
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x00020D40 File Offset: 0x0001FD40
		public SrgsItem()
		{
			this._elements = new SrgsElementList();
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00020D77 File Offset: 0x0001FD77
		public SrgsItem(string text)
			: this()
		{
			Helpers.ThrowIfEmptyOrNull(text, "text");
			this._elements.Add(new SrgsText(text));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00020D9C File Offset: 0x0001FD9C
		public SrgsItem(params SrgsElement[] elements)
			: this()
		{
			Helpers.ThrowIfNull(elements, "elements");
			for (int i = 0; i < elements.Length; i++)
			{
				if (elements[i] == null)
				{
					throw new ArgumentNullException("elements", SR.Get(SRID.ParamsEntryNullIllegal, new object[0]));
				}
				this._elements.Add(elements[i]);
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00020DF2 File Offset: 0x0001FDF2
		public SrgsItem(int repeatCount)
			: this()
		{
			this.SetRepeat(repeatCount);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00020E01 File Offset: 0x0001FE01
		public SrgsItem(int min, int max)
			: this()
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00020E11 File Offset: 0x0001FE11
		public SrgsItem(int min, int max, string text)
			: this(text)
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00020E22 File Offset: 0x0001FE22
		public SrgsItem(int min, int max, params SrgsElement[] elements)
			: this(elements)
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00020E34 File Offset: 0x0001FE34
		public void SetRepeat(int count)
		{
			if (count < 0 || count > 255)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._maxRepeat = count;
			this._minRepeat = count;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00020E68 File Offset: 0x0001FE68
		public void SetRepeat(int minRepeat, int maxRepeat)
		{
			if (minRepeat < 0 || minRepeat > 255)
			{
				throw new ArgumentOutOfRangeException("minRepeat", SR.Get(SRID.InvalidMinRepeat, new object[] { minRepeat }));
			}
			if (maxRepeat != 2147483647 && (maxRepeat < 0 || maxRepeat > 255))
			{
				throw new ArgumentOutOfRangeException("maxRepeat", SR.Get(SRID.InvalidMinRepeat, new object[] { maxRepeat }));
			}
			if (minRepeat > maxRepeat)
			{
				throw new ArgumentException(SR.Get(SRID.MinGreaterThanMax, new object[0]));
			}
			this._minRepeat = minRepeat;
			this._maxRepeat = maxRepeat;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00020F00 File Offset: 0x0001FF00
		public void Add(SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			this.Elements.Add(element);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00020F19 File Offset: 0x0001FF19
		public Collection<SrgsElement> Elements
		{
			get
			{
				return this._elements;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00020F21 File Offset: 0x0001FF21
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x00020F2C File Offset: 0x0001FF2C
		public float RepeatProbability
		{
			get
			{
				return this._repeatProbability;
			}
			set
			{
				if (value < 0f || value > 1f)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.InvalidRepeatProbability, new object[] { value }));
				}
				this._repeatProbability = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00020F73 File Offset: 0x0001FF73
		public int MinRepeat
		{
			get
			{
				if (this._minRepeat != -1)
				{
					return this._minRepeat;
				}
				return 1;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00020F86 File Offset: 0x0001FF86
		public int MaxRepeat
		{
			get
			{
				if (this._maxRepeat != -1)
				{
					return this._maxRepeat;
				}
				return 1;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00020F99 File Offset: 0x0001FF99
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00020FA4 File Offset: 0x0001FFA4
		public float Weight
		{
			get
			{
				return this._weight;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.InvalidWeight, new object[] { value }));
				}
				this._weight = value;
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00020FE4 File Offset: 0x0001FFE4
		internal override void WriteSrgs(XmlWriter writer)
		{
			writer.WriteStartElement("item");
			if (!this._weight.Equals(1f))
			{
				writer.WriteAttributeString("weight", this._weight.ToString("0.########", CultureInfo.InvariantCulture));
			}
			if (!this._repeatProbability.Equals(0.5f))
			{
				writer.WriteAttributeString("repeat-prob", this._repeatProbability.ToString("0.########", CultureInfo.InvariantCulture));
			}
			if (this._minRepeat == this._maxRepeat)
			{
				if (this._minRepeat != -1)
				{
					writer.WriteAttributeString("repeat", string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { this._minRepeat }));
				}
			}
			else if (this._maxRepeat == 2147483647 || this._maxRepeat == -1)
			{
				writer.WriteAttributeString("repeat", string.Format(CultureInfo.InvariantCulture, "{0}-", new object[] { this._minRepeat }));
			}
			else
			{
				int num = ((this._minRepeat == -1) ? 1 : this._minRepeat);
				writer.WriteAttributeString("repeat", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", new object[] { num, this._maxRepeat }));
			}
			Type type = null;
			foreach (SrgsElement srgsElement in this._elements)
			{
				Type type2 = srgsElement.GetType();
				if (type2 == typeof(SrgsText) && type2 == type)
				{
					writer.WriteString(" ");
				}
				type = type2;
				srgsElement.WriteSrgs(writer);
			}
			writer.WriteEndElement();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x000211C0 File Offset: 0x000201C0
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._elements.Count > 7)
			{
				stringBuilder.Append("SrgsItem Count = ");
				stringBuilder.Append(this._elements.Count.ToString(CultureInfo.InstalledUICulture));
			}
			else
			{
				if (this._minRepeat != this._maxRepeat || this._maxRepeat != -1)
				{
					stringBuilder.Append("[");
					if (this._minRepeat == this._maxRepeat)
					{
						stringBuilder.Append(this._minRepeat.ToString(CultureInfo.InvariantCulture));
					}
					else if (this._maxRepeat == 2147483647 || this._maxRepeat == -1)
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0},-", new object[] { this._minRepeat }));
					}
					else
					{
						int num = ((this._minRepeat == -1) ? 1 : this._minRepeat);
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[] { num, this._maxRepeat }));
					}
					stringBuilder.Append("] ");
				}
				bool flag = true;
				foreach (SrgsElement srgsElement in this._elements)
				{
					if (!flag)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("{");
					stringBuilder.Append(srgsElement.DebuggerDisplayString());
					stringBuilder.Append("}");
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00021380 File Offset: 0x00020380
		internal override SrgsElement[] Children
		{
			get
			{
				SrgsElement[] array = new SrgsElement[this._elements.Count];
				int num = 0;
				foreach (SrgsElement srgsElement in this._elements)
				{
					array[num++] = srgsElement;
				}
				return array;
			}
		}

		// Token: 0x04000566 RID: 1382
		private const int NotSet = -1;

		// Token: 0x04000567 RID: 1383
		private float _weight = 1f;

		// Token: 0x04000568 RID: 1384
		private float _repeatProbability = 0.5f;

		// Token: 0x04000569 RID: 1385
		private int _minRepeat = -1;

		// Token: 0x0400056A RID: 1386
		private int _maxRepeat = -1;

		// Token: 0x0400056B RID: 1387
		private SrgsElementList _elements;

		// Token: 0x0200011A RID: 282
		internal class SrgsItemDebugDisplay
		{
			// Token: 0x06000767 RID: 1895 RVA: 0x000213E4 File Offset: 0x000203E4
			public SrgsItemDebugDisplay(SrgsItem item)
			{
				this._weight = item._weight;
				this._repeatProbability = item._repeatProbability;
				this._minRepeat = item._minRepeat;
				this._maxRepeat = item._maxRepeat;
				this._elements = item._elements;
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x06000768 RID: 1896 RVA: 0x00021457 File Offset: 0x00020457
			public object Weigth
			{
				get
				{
					return this._weight;
				}
			}

			// Token: 0x17000122 RID: 290
			// (get) Token: 0x06000769 RID: 1897 RVA: 0x00021464 File Offset: 0x00020464
			public object MinRepeat
			{
				get
				{
					return this._minRepeat;
				}
			}

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x0600076A RID: 1898 RVA: 0x00021471 File Offset: 0x00020471
			public object MaxRepeat
			{
				get
				{
					return this._maxRepeat;
				}
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x0600076B RID: 1899 RVA: 0x0002147E File Offset: 0x0002047E
			public object RepeatProbability
			{
				get
				{
					return this._repeatProbability;
				}
			}

			// Token: 0x17000125 RID: 293
			// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002148B File Offset: 0x0002048B
			public object Count
			{
				get
				{
					return this._elements.Count;
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x0600076D RID: 1901 RVA: 0x000214A0 File Offset: 0x000204A0
			[DebuggerBrowsable(3)]
			public SrgsElement[] AKeys
			{
				get
				{
					SrgsElement[] array = new SrgsElement[this._elements.Count];
					for (int i = 0; i < this._elements.Count; i++)
					{
						array[i] = this._elements[i];
					}
					return array;
				}
			}

			// Token: 0x0400056C RID: 1388
			private float _weight = 1f;

			// Token: 0x0400056D RID: 1389
			private float _repeatProbability = 0.5f;

			// Token: 0x0400056E RID: 1390
			private int _minRepeat = -1;

			// Token: 0x0400056F RID: 1391
			private int _maxRepeat = -1;

			// Token: 0x04000570 RID: 1392
			private SrgsElementList _elements;
		}
	}
}
