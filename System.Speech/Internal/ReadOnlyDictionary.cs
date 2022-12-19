using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Speech.Internal
{
	// Token: 0x0200017D RID: 381
	internal class ReadOnlyDictionary<K, V> : IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x00028C0C File Offset: 0x00027C0C
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00028C19 File Offset: 0x00027C19
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x170001BA RID: 442
		public V this[K key]
		{
			get
			{
				return this._dictionary[key];
			}
			set
			{
				throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00028C4C File Offset: 0x00027C4C
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00028C4F File Offset: 0x00027C4F
		public bool Contains(KeyValuePair<K, V> key)
		{
			return this._dictionary.ContainsKey(key.Key);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00028C63 File Offset: 0x00027C63
		public bool ContainsKey(K key)
		{
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00028C71 File Offset: 0x00027C71
		public void CopyTo(KeyValuePair<K, V>[] array, int index)
		{
			this._dictionary.CopyTo(array, index);
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00028C80 File Offset: 0x00027C80
		public ICollection<K> Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00028C8D File Offset: 0x00027C8D
		public ICollection<V> Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00028C9A File Offset: 0x00027C9A
		public void Add(KeyValuePair<K, V> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00028CAD File Offset: 0x00027CAD
		public void Add(K key, V value)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00028CC0 File Offset: 0x00027CC0
		public void Clear()
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00028CD3 File Offset: 0x00027CD3
		public bool Remove(KeyValuePair<K, V> key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00028CE6 File Offset: 0x00027CE6
		public bool Remove(K key)
		{
			throw new NotSupportedException(SR.Get(SRID.CollectionReadOnly, new object[0]));
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00028CF9 File Offset: 0x00027CF9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00028D01 File Offset: 0x00027D01
		bool IDictionary<K, V>.TryGetValue(K key, out V value)
		{
			return this.InternalDictionary.TryGetValue(key, ref value);
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00028D10 File Offset: 0x00027D10
		internal Dictionary<K, V> InternalDictionary
		{
			get
			{
				return this._dictionary;
			}
		}

		// Token: 0x040008CD RID: 2253
		private Dictionary<K, V> _dictionary = new Dictionary<K, V>();
	}
}
