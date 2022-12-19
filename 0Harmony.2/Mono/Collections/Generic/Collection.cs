using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Collections.Generic
{
	// Token: 0x020001A7 RID: 423
	public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00017D4C File Offset: 0x00015F4C
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170001E8 RID: 488
		public T this[int index]
		{
			get
			{
				if (index >= this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.items[index];
			}
			set
			{
				this.CheckIndex(index);
				if (index == this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.OnSet(value, index);
				this.items[index] = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00017D9E File Offset: 0x00015F9E
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x00017DA8 File Offset: 0x00015FA8
		public int Capacity
		{
			get
			{
				return this.items.Length;
			}
			set
			{
				if (value < 0 || value < this.size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.Resize(value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00017DC4 File Offset: 0x00015FC4
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00017DC4 File Offset: 0x00015FC4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00017DC4 File Offset: 0x00015FC4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001ED RID: 493
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.CheckIndex(index);
				try
				{
					this[index] = (T)((object)value);
					return;
				}
				catch (InvalidCastException)
				{
				}
				catch (NullReferenceException)
				{
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00017E24 File Offset: 0x00016024
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00017DC4 File Offset: 0x00015FC4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00017E2C File Offset: 0x0001602C
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00017E2F File Offset: 0x0001602F
		public Collection()
		{
			this.items = Empty<T>.Array;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00017E42 File Offset: 0x00016042
		public Collection(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = ((capacity == 0) ? Empty<T>.Array : new T[capacity]);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00017E6C File Offset: 0x0001606C
		public Collection(ICollection<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.items = new T[items.Count];
			items.CopyTo(this.items, 0);
			this.size = this.items.Length;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00017EBC File Offset: 0x000160BC
		public void Add(T item)
		{
			if (this.size == this.items.Length)
			{
				this.Grow(1);
			}
			this.OnAdd(item, this.size);
			T[] array = this.items;
			int num = this.size;
			this.size = num + 1;
			array[num] = item;
			this.version++;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00017F18 File Offset: 0x00016118
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00017F27 File Offset: 0x00016127
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this.size);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00017F3C File Offset: 0x0001613C
		public void Insert(int index, T item)
		{
			this.CheckIndex(index);
			if (this.size == this.items.Length)
			{
				this.Grow(1);
			}
			this.OnInsert(item, index);
			this.Shift(index, 1);
			this.items[index] = item;
			this.version++;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00017F94 File Offset: 0x00016194
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
			T t = this.items[index];
			this.OnRemove(t, index);
			this.Shift(index, -1);
			this.version++;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00017FE0 File Offset: 0x000161E0
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			this.OnRemove(item, num);
			this.Shift(num, -1);
			this.version++;
			return true;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001801A File Offset: 0x0001621A
		public void Clear()
		{
			this.OnClear();
			Array.Clear(this.items, 0, this.size);
			this.size = 0;
			this.version++;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00018049 File Offset: 0x00016249
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.items, 0, array, arrayIndex, this.size);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00018060 File Offset: 0x00016260
		public T[] ToArray()
		{
			T[] array = new T[this.size];
			Array.Copy(this.items, 0, array, 0, this.size);
			return array;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001808E File Offset: 0x0001628E
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000180A4 File Offset: 0x000162A4
		private void Shift(int start, int delta)
		{
			if (delta < 0)
			{
				start -= delta;
			}
			if (start < this.size)
			{
				Array.Copy(this.items, start, this.items, start + delta, this.size - start);
			}
			this.size += delta;
			if (delta < 0)
			{
				Array.Clear(this.items, this.size, -delta);
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void OnAdd(T item, int index)
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void OnInsert(T item, int index)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void OnSet(T item, int index)
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void OnRemove(T item, int index)
		{
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void OnClear()
		{
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00018108 File Offset: 0x00016308
		internal virtual void Grow(int desired)
		{
			int num = this.size + desired;
			if (num <= this.items.Length)
			{
				return;
			}
			num = Math.Max(Math.Max(this.items.Length * 2, 4), num);
			this.Resize(num);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00018148 File Offset: 0x00016348
		protected void Resize(int new_size)
		{
			if (new_size == this.size)
			{
				return;
			}
			if (new_size < this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = this.items.Resize(new_size);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00018178 File Offset: 0x00016378
		int IList.Add(object value)
		{
			try
			{
				this.Add((T)((object)value));
				return this.size - 1;
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			throw new ArgumentException();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000181C8 File Offset: 0x000163C8
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000181D0 File Offset: 0x000163D0
		bool IList.Contains(object value)
		{
			return ((IList)this).IndexOf(value) > -1;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000181DC File Offset: 0x000163DC
		int IList.IndexOf(object value)
		{
			try
			{
				return this.IndexOf((T)((object)value));
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			return -1;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00018220 File Offset: 0x00016420
		void IList.Insert(int index, object value)
		{
			this.CheckIndex(index);
			try
			{
				this.Insert(index, (T)((object)value));
				return;
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			throw new ArgumentException();
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001826C File Offset: 0x0001646C
		void IList.Remove(object value)
		{
			try
			{
				this.Remove((T)((object)value));
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000182AC File Offset: 0x000164AC
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00018049 File Offset: 0x00016249
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.items, 0, array, index, this.size);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000182B5 File Offset: 0x000164B5
		public Collection<T>.Enumerator GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000182BD File Offset: 0x000164BD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000182BD File Offset: 0x000164BD
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x0400022E RID: 558
		internal T[] items;

		// Token: 0x0400022F RID: 559
		internal int size;

		// Token: 0x04000230 RID: 560
		private int version;

		// Token: 0x020001A8 RID: 424
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x170001F1 RID: 497
			// (get) Token: 0x0600071C RID: 1820 RVA: 0x000182CA File Offset: 0x000164CA
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x170001F2 RID: 498
			// (get) Token: 0x0600071D RID: 1821 RVA: 0x000182D2 File Offset: 0x000164D2
			object IEnumerator.Current
			{
				get
				{
					this.CheckState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException();
					}
					return this.current;
				}
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x000182F4 File Offset: 0x000164F4
			internal Enumerator(Collection<T> collection)
			{
				this = default(Collection<T>.Enumerator);
				this.collection = collection;
				this.version = collection.version;
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x00018310 File Offset: 0x00016510
			public bool MoveNext()
			{
				this.CheckState();
				if (this.next < 0)
				{
					return false;
				}
				if (this.next < this.collection.size)
				{
					T[] items = this.collection.items;
					int num = this.next;
					this.next = num + 1;
					this.current = items[num];
					return true;
				}
				this.next = -1;
				return false;
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x00018372 File Offset: 0x00016572
			public void Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x00018381 File Offset: 0x00016581
			private void CheckState()
			{
				if (this.collection == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.version != this.collection.version)
				{
					throw new InvalidOperationException();
				}
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x000183BF File Offset: 0x000165BF
			public void Dispose()
			{
				this.collection = null;
			}

			// Token: 0x04000231 RID: 561
			private Collection<T> collection;

			// Token: 0x04000232 RID: 562
			private T current;

			// Token: 0x04000233 RID: 563
			private int next;

			// Token: 0x04000234 RID: 564
			private readonly int version;
		}
	}
}
