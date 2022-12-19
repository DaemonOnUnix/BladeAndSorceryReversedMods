using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x02000136 RID: 310
	[DebuggerDisplay("'{_keyName}'= {Value}  -  Children = {_dictionary.Count}")]
	[DebuggerTypeProxy(typeof(SemanticValue.SemanticValueDebugDisplay))]
	[Serializable]
	public sealed class SemanticValue : IDictionary<string, SemanticValue>, ICollection<KeyValuePair<string, SemanticValue>>, IEnumerable<KeyValuePair<string, SemanticValue>>, IEnumerable
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00025651 File Offset: 0x00024651
		public SemanticValue(string keyName, object value, float confidence)
		{
			Helpers.ThrowIfNull(keyName, "keyName");
			this._dictionary = new Dictionary<string, SemanticValue>();
			this._confidence = confidence;
			this._keyName = keyName;
			this._value = value;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00025684 File Offset: 0x00024684
		public SemanticValue(object value)
			: this(string.Empty, value, -1f)
		{
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00025698 File Offset: 0x00024698
		public override bool Equals(object obj)
		{
			SemanticValue semanticValue = obj as SemanticValue;
			if (semanticValue == null || semanticValue.Count != this.Count || (semanticValue.Value == null && this.Value != null) || (semanticValue.Value != null && !semanticValue.Value.Equals(this.Value)))
			{
				return false;
			}
			foreach (KeyValuePair<string, SemanticValue> keyValuePair in this._dictionary)
			{
				if (!semanticValue.ContainsKey(keyValuePair.Key) || !semanticValue[keyValuePair.Key].Equals(this[keyValuePair.Key]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00025760 File Offset: 0x00024760
		public override int GetHashCode()
		{
			return this.Count;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00025768 File Offset: 0x00024768
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x00025770 File Offset: 0x00024770
		public object Value
		{
			get
			{
				return this._value;
			}
			internal set
			{
				this._value = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00025779 File Offset: 0x00024779
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x1700016A RID: 362
		public SemanticValue this[string key]
		{
			get
			{
				return this._dictionary[key];
			}
			set
			{
				throw new InvalidOperationException(SR.Get(SRID.CollectionReadOnly, new object[0]));
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000257A2 File Offset: 0x000247A2
		public bool Contains(KeyValuePair<string, SemanticValue> item)
		{
			return this._dictionary.ContainsKey(item.Key) && this._dictionary.ContainsValue(item.Value);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000257CC File Offset: 0x000247CC
		public bool ContainsKey(string key)
		{
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x000257DA File Offset: 0x000247DA
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000257E7 File Offset: 0x000247E7
		void ICollection<KeyValuePair<string, SemanticValue>>.Add(KeyValuePair<string, SemanticValue> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000257FA File Offset: 0x000247FA
		void IDictionary<string, SemanticValue>.Add(string key, SemanticValue value)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002580D File Offset: 0x0002480D
		void ICollection<KeyValuePair<string, SemanticValue>>.Clear()
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00025820 File Offset: 0x00024820
		bool ICollection<KeyValuePair<string, SemanticValue>>.Remove(KeyValuePair<string, SemanticValue> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00025833 File Offset: 0x00024833
		bool IDictionary<string, SemanticValue>.Remove(string key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00025846 File Offset: 0x00024846
		void ICollection<KeyValuePair<string, SemanticValue>>.CopyTo(KeyValuePair<string, SemanticValue>[] array, int index)
		{
			this._dictionary.CopyTo(array, index);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00025855 File Offset: 0x00024855
		IEnumerator<KeyValuePair<string, SemanticValue>> IEnumerable<KeyValuePair<string, SemanticValue>>.GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x00025867 File Offset: 0x00024867
		bool ICollection<KeyValuePair<string, SemanticValue>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0002586A File Offset: 0x0002486A
		ICollection<string> IDictionary<string, SemanticValue>.Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00025877 File Offset: 0x00024877
		ICollection<SemanticValue> IDictionary<string, SemanticValue>.Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00025884 File Offset: 0x00024884
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002588C File Offset: 0x0002488C
		bool IDictionary<string, SemanticValue>.TryGetValue(string key, out SemanticValue value)
		{
			return this._dictionary.TryGetValue(key, ref value);
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0002589B File Offset: 0x0002489B
		internal string KeyName
		{
			get
			{
				return this._keyName;
			}
		}

		// Token: 0x040005DF RID: 1503
		internal Dictionary<string, SemanticValue> _dictionary;

		// Token: 0x040005E0 RID: 1504
		internal bool _valueFieldSet;

		// Token: 0x040005E1 RID: 1505
		private string _keyName;

		// Token: 0x040005E2 RID: 1506
		private float _confidence;

		// Token: 0x040005E3 RID: 1507
		private object _value;

		// Token: 0x02000137 RID: 311
		internal class SemanticValueDebugDisplay
		{
			// Token: 0x06000850 RID: 2128 RVA: 0x000258A3 File Offset: 0x000248A3
			public SemanticValueDebugDisplay(SemanticValue value)
			{
				this._value = value.Value;
				this._dictionary = value._dictionary;
				this._name = value.KeyName;
				this._confidence = value.Confidence;
			}

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x06000851 RID: 2129 RVA: 0x000258DB File Offset: 0x000248DB
			public object Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x06000852 RID: 2130 RVA: 0x000258E3 File Offset: 0x000248E3
			public object Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x06000853 RID: 2131 RVA: 0x000258F5 File Offset: 0x000248F5
			public object KeyName
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x06000854 RID: 2132 RVA: 0x000258FD File Offset: 0x000248FD
			public object Confidence
			{
				get
				{
					return this._confidence;
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x06000855 RID: 2133 RVA: 0x0002590C File Offset: 0x0002490C
			[DebuggerBrowsable(3)]
			public SemanticValue[] AKeys
			{
				get
				{
					SemanticValue[] array = new SemanticValue[this._dictionary.Count];
					int num = 0;
					foreach (KeyValuePair<string, SemanticValue> keyValuePair in this._dictionary)
					{
						array[num++] = keyValuePair.Value;
					}
					return array;
				}
			}

			// Token: 0x040005E4 RID: 1508
			private object _name;

			// Token: 0x040005E5 RID: 1509
			private object _value;

			// Token: 0x040005E6 RID: 1510
			private float _confidence;

			// Token: 0x040005E7 RID: 1511
			private IDictionary<string, SemanticValue> _dictionary;
		}
	}
}
