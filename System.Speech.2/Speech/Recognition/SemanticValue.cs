using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x02000055 RID: 85
	[DebuggerDisplay("'{_keyName}'= {Value}  -  Children = {_dictionary.Count}")]
	[DebuggerTypeProxy(typeof(SemanticValue.SemanticValueDebugDisplay))]
	[Serializable]
	public sealed class SemanticValue : IDictionary<string, SemanticValue>, ICollection<KeyValuePair<string, SemanticValue>>, IEnumerable<KeyValuePair<string, SemanticValue>>, IEnumerable
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x000091A1 File Offset: 0x000073A1
		public SemanticValue(string keyName, object value, float confidence)
		{
			Helpers.ThrowIfNull(keyName, "keyName");
			this._dictionary = new Dictionary<string, SemanticValue>();
			this._confidence = confidence;
			this._keyName = keyName;
			this._value = value;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000091D4 File Offset: 0x000073D4
		public SemanticValue(object value)
			: this(string.Empty, value, -1f)
		{
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000091E8 File Offset: 0x000073E8
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

		// Token: 0x060001D7 RID: 471 RVA: 0x000092B0 File Offset: 0x000074B0
		public override int GetHashCode()
		{
			return this.Count;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000092B8 File Offset: 0x000074B8
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x000092C0 File Offset: 0x000074C0
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001DA RID: 474 RVA: 0x000092C9 File Offset: 0x000074C9
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x17000084 RID: 132
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

		// Token: 0x060001DD RID: 477 RVA: 0x000092F2 File Offset: 0x000074F2
		public bool Contains(KeyValuePair<string, SemanticValue> item)
		{
			return this._dictionary.ContainsKey(item.Key) && this._dictionary.ContainsValue(item.Value);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000931C File Offset: 0x0000751C
		public bool ContainsKey(string key)
		{
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000932A File Offset: 0x0000752A
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009337 File Offset: 0x00007537
		void ICollection<KeyValuePair<string, SemanticValue>>.Add(KeyValuePair<string, SemanticValue> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009337 File Offset: 0x00007537
		void IDictionary<string, SemanticValue>.Add(string key, SemanticValue value)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009337 File Offset: 0x00007537
		void ICollection<KeyValuePair<string, SemanticValue>>.Clear()
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009337 File Offset: 0x00007537
		bool ICollection<KeyValuePair<string, SemanticValue>>.Remove(KeyValuePair<string, SemanticValue> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009337 File Offset: 0x00007537
		bool IDictionary<string, SemanticValue>.Remove(string key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000934A File Offset: 0x0000754A
		void ICollection<KeyValuePair<string, SemanticValue>>.CopyTo(KeyValuePair<string, SemanticValue>[] array, int index)
		{
			((ICollection<KeyValuePair<string, SemanticValue>>)this._dictionary).CopyTo(array, index);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009359 File Offset: 0x00007559
		IEnumerator<KeyValuePair<string, SemanticValue>> IEnumerable<KeyValuePair<string, SemanticValue>>.GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000936B File Offset: 0x0000756B
		bool ICollection<KeyValuePair<string, SemanticValue>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000936E File Offset: 0x0000756E
		ICollection<string> IDictionary<string, SemanticValue>.Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000937B File Offset: 0x0000757B
		ICollection<SemanticValue> IDictionary<string, SemanticValue>.Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009388 File Offset: 0x00007588
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, SemanticValue>>)this).GetEnumerator();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009390 File Offset: 0x00007590
		bool IDictionary<string, SemanticValue>.TryGetValue(string key, out SemanticValue value)
		{
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000939F File Offset: 0x0000759F
		internal string KeyName
		{
			get
			{
				return this._keyName;
			}
		}

		// Token: 0x04000329 RID: 809
		internal Dictionary<string, SemanticValue> _dictionary;

		// Token: 0x0400032A RID: 810
		internal bool _valueFieldSet;

		// Token: 0x0400032B RID: 811
		private string _keyName;

		// Token: 0x0400032C RID: 812
		private float _confidence;

		// Token: 0x0400032D RID: 813
		private object _value;

		// Token: 0x02000179 RID: 377
		internal class SemanticValueDebugDisplay
		{
			// Token: 0x06000B42 RID: 2882 RVA: 0x0002D2D8 File Offset: 0x0002B4D8
			public SemanticValueDebugDisplay(SemanticValue value)
			{
				this._value = value.Value;
				this._dictionary = value._dictionary;
				this._name = value.KeyName;
				this._confidence = value.Confidence;
			}

			// Token: 0x170001FE RID: 510
			// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002D310 File Offset: 0x0002B510
			public object Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x170001FF RID: 511
			// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0002D318 File Offset: 0x0002B518
			public object Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			// Token: 0x17000200 RID: 512
			// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002D32A File Offset: 0x0002B52A
			public object KeyName
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000201 RID: 513
			// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0002D332 File Offset: 0x0002B532
			public object Confidence
			{
				get
				{
					return this._confidence;
				}
			}

			// Token: 0x17000202 RID: 514
			// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0002D340 File Offset: 0x0002B540
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
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

			// Token: 0x040008A8 RID: 2216
			private object _name;

			// Token: 0x040008A9 RID: 2217
			private object _value;

			// Token: 0x040008AA RID: 2218
			private float _confidence;

			// Token: 0x040008AB RID: 2219
			private IDictionary<string, SemanticValue> _dictionary;
		}
	}
}
