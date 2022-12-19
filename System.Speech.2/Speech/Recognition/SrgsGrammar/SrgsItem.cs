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
	// Token: 0x0200007A RID: 122
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[DebuggerTypeProxy(typeof(SrgsItem.SrgsItemDebugDisplay))]
	[Serializable]
	public class SrgsItem : SrgsElement, IItem, IElement
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x0001058A File Offset: 0x0000E78A
		public SrgsItem()
		{
			this._elements = new SrgsElementList();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000105C1 File Offset: 0x0000E7C1
		public SrgsItem(string text)
			: this()
		{
			Helpers.ThrowIfEmptyOrNull(text, "text");
			this._elements.Add(new SrgsText(text));
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000105E8 File Offset: 0x0000E7E8
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

		// Token: 0x0600040E RID: 1038 RVA: 0x0001063E File Offset: 0x0000E83E
		public SrgsItem(int repeatCount)
			: this()
		{
			this.SetRepeat(repeatCount);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001064D File Offset: 0x0000E84D
		public SrgsItem(int min, int max)
			: this()
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001065D File Offset: 0x0000E85D
		public SrgsItem(int min, int max, string text)
			: this(text)
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001066E File Offset: 0x0000E86E
		public SrgsItem(int min, int max, params SrgsElement[] elements)
			: this(elements)
		{
			this.SetRepeat(min, max);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00010680 File Offset: 0x0000E880
		public void SetRepeat(int count)
		{
			if (count < 0 || count > 255)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._maxRepeat = count;
			this._minRepeat = count;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000106B4 File Offset: 0x0000E8B4
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

		// Token: 0x06000414 RID: 1044 RVA: 0x00010748 File Offset: 0x0000E948
		public void Add(SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			this.Elements.Add(element);
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00010761 File Offset: 0x0000E961
		public Collection<SrgsElement> Elements
		{
			get
			{
				return this._elements;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00010769 File Offset: 0x0000E969
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x00010771 File Offset: 0x0000E971
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x000107AB File Offset: 0x0000E9AB
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000107BE File Offset: 0x0000E9BE
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x000107D1 File Offset: 0x0000E9D1
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x000107D9 File Offset: 0x0000E9D9
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

		// Token: 0x0600041C RID: 1052 RVA: 0x0001080C File Offset: 0x0000EA0C
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

		// Token: 0x0600041D RID: 1053 RVA: 0x000109E0 File Offset: 0x0000EBE0
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._elements.Count > 7)
			{
				stringBuilder.Append("SrgsItem Count = ");
				stringBuilder.Append(this._elements.Count.ToString(CultureInfo.InvariantCulture));
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00010B94 File Offset: 0x0000ED94
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

		// Token: 0x040003E2 RID: 994
		private float _weight = 1f;

		// Token: 0x040003E3 RID: 995
		private float _repeatProbability = 0.5f;

		// Token: 0x040003E4 RID: 996
		private int _minRepeat = -1;

		// Token: 0x040003E5 RID: 997
		private int _maxRepeat = -1;

		// Token: 0x040003E6 RID: 998
		private SrgsElementList _elements;

		// Token: 0x040003E7 RID: 999
		private const int NotSet = -1;

		// Token: 0x0200017F RID: 383
		internal class SrgsItemDebugDisplay
		{
			// Token: 0x06000B56 RID: 2902 RVA: 0x0002D61C File Offset: 0x0002B81C
			public SrgsItemDebugDisplay(SrgsItem item)
			{
				this._weight = item._weight;
				this._repeatProbability = item._repeatProbability;
				this._minRepeat = item._minRepeat;
				this._maxRepeat = item._maxRepeat;
				this._elements = item._elements;
			}

			// Token: 0x17000205 RID: 517
			// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0002D68F File Offset: 0x0002B88F
			public object Weigth
			{
				get
				{
					return this._weight;
				}
			}

			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002D69C File Offset: 0x0002B89C
			public object MinRepeat
			{
				get
				{
					return this._minRepeat;
				}
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0002D6A9 File Offset: 0x0002B8A9
			public object MaxRepeat
			{
				get
				{
					return this._maxRepeat;
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0002D6B6 File Offset: 0x0002B8B6
			public object RepeatProbability
			{
				get
				{
					return this._repeatProbability;
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0002D6C3 File Offset: 0x0002B8C3
			public object Count
			{
				get
				{
					return this._elements.Count;
				}
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
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

			// Token: 0x040008B2 RID: 2226
			private float _weight = 1f;

			// Token: 0x040008B3 RID: 2227
			private float _repeatProbability = 0.5f;

			// Token: 0x040008B4 RID: 2228
			private int _minRepeat = -1;

			// Token: 0x040008B5 RID: 2229
			private int _maxRepeat = -1;

			// Token: 0x040008B6 RID: 2230
			private SrgsElementList _elements;
		}
	}
}
