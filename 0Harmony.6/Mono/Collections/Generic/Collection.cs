using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Collections.Generic
{
	// Token: 0x020000B5 RID: 181
	public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00011EC0 File Offset: 0x000100C0
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000018 RID: 24
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00011F12 File Offset: 0x00010112
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00011F1C File Offset: 0x0001011C
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00011F38 File Offset: 0x00010138
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00011F38 File Offset: 0x00010138
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00011F38 File Offset: 0x00010138
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001D RID: 29
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00011F98 File Offset: 0x00010198
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00011F38 File Offset: 0x00010138
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00011FA0 File Offset: 0x000101A0
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00011FA3 File Offset: 0x000101A3
		public Collection()
		{
			this.items = Empty<T>.Array;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00011FB6 File Offset: 0x000101B6
		public Collection(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.items = ((capacity == 0) ? Empty<T>.Array : new T[capacity]);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00011FE0 File Offset: 0x000101E0
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

		// Token: 0x060003C9 RID: 969 RVA: 0x00012030 File Offset: 0x00010230
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

		// Token: 0x060003CA RID: 970 RVA: 0x0001208C File Offset: 0x0001028C
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001209B File Offset: 0x0001029B
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.items, item, 0, this.size);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000120B0 File Offset: 0x000102B0
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

		// Token: 0x060003CD RID: 973 RVA: 0x00012108 File Offset: 0x00010308
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

		// Token: 0x060003CE RID: 974 RVA: 0x00012154 File Offset: 0x00010354
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

		// Token: 0x060003CF RID: 975 RVA: 0x0001218E File Offset: 0x0001038E
		public void Clear()
		{
			this.OnClear();
			Array.Clear(this.items, 0, this.size);
			this.size = 0;
			this.version++;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000121BD File Offset: 0x000103BD
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.items, 0, array, arrayIndex, this.size);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000121D4 File Offset: 0x000103D4
		public T[] ToArray()
		{
			T[] array = new T[this.size];
			Array.Copy(this.items, 0, array, 0, this.size);
			return array;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00012202 File Offset: 0x00010402
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00012218 File Offset: 0x00010418
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

		// Token: 0x060003D4 RID: 980 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void OnAdd(T item, int index)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void OnInsert(T item, int index)
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void OnSet(T item, int index)
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void OnRemove(T item, int index)
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void OnClear()
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001227C File Offset: 0x0001047C
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

		// Token: 0x060003DA RID: 986 RVA: 0x000122BC File Offset: 0x000104BC
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

		// Token: 0x060003DB RID: 987 RVA: 0x000122EC File Offset: 0x000104EC
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

		// Token: 0x060003DC RID: 988 RVA: 0x0001233C File Offset: 0x0001053C
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00012344 File Offset: 0x00010544
		bool IList.Contains(object value)
		{
			return ((IList)this).IndexOf(value) > -1;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00012350 File Offset: 0x00010550
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

		// Token: 0x060003DF RID: 991 RVA: 0x00012394 File Offset: 0x00010594
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

		// Token: 0x060003E0 RID: 992 RVA: 0x000123E0 File Offset: 0x000105E0
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

		// Token: 0x060003E1 RID: 993 RVA: 0x00012420 File Offset: 0x00010620
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000121BD File Offset: 0x000103BD
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.items, 0, array, index, this.size);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00012429 File Offset: 0x00010629
		public Collection<T>.Enumerator GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00012431 File Offset: 0x00010631
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00012431 File Offset: 0x00010631
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Collection<T>.Enumerator(this);
		}

		// Token: 0x04000200 RID: 512
		internal T[] items;

		// Token: 0x04000201 RID: 513
		internal int size;

		// Token: 0x04000202 RID: 514
		private int version;

		// Token: 0x020000B6 RID: 182
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001243E File Offset: 0x0001063E
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060003E7 RID: 999 RVA: 0x00012446 File Offset: 0x00010646
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

			// Token: 0x060003E8 RID: 1000 RVA: 0x00012468 File Offset: 0x00010668
			internal Enumerator(Collection<T> collection)
			{
				this = default(Collection<T>.Enumerator);
				this.collection = collection;
				this.version = collection.version;
			}

			// Token: 0x060003E9 RID: 1001 RVA: 0x00012484 File Offset: 0x00010684
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

			// Token: 0x060003EA RID: 1002 RVA: 0x000124E6 File Offset: 0x000106E6
			public void Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x060003EB RID: 1003 RVA: 0x000124F5 File Offset: 0x000106F5
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

			// Token: 0x060003EC RID: 1004 RVA: 0x00012533 File Offset: 0x00010733
			public void Dispose()
			{
				this.collection = null;
			}

			// Token: 0x04000203 RID: 515
			private Collection<T> collection;

			// Token: 0x04000204 RID: 516
			private T current;

			// Token: 0x04000205 RID: 517
			private int next;

			// Token: 0x04000206 RID: 518
			private readonly int version;
		}
	}
}
