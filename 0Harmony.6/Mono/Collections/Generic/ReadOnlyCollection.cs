using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Mono.Collections.Generic
{
	// Token: 0x020000B7 RID: 183
	internal sealed class ReadOnlyCollection<T> : Collection<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001253C File Offset: 0x0001073C
		public static ReadOnlyCollection<T> Empty
		{
			get
			{
				if (ReadOnlyCollection<T>.empty != null)
				{
					return ReadOnlyCollection<T>.empty;
				}
				Interlocked.CompareExchange<ReadOnlyCollection<T>>(ref ReadOnlyCollection<T>.empty, new ReadOnlyCollection<T>(), null);
				return ReadOnlyCollection<T>.empty;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00012561 File Offset: 0x00010761
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00012561 File Offset: 0x00010761
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00012561 File Offset: 0x00010761
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00012564 File Offset: 0x00010764
		private ReadOnlyCollection()
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001256C File Offset: 0x0001076C
		public ReadOnlyCollection(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(array, array.Length);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00012587 File Offset: 0x00010787
		public ReadOnlyCollection(Collection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(collection.items, collection.size);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000125AA File Offset: 0x000107AA
		private void Initialize(T[] items, int size)
		{
			this.items = new T[size];
			Array.Copy(items, 0, this.items, 0, size);
			this.size = size;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000125CE File Offset: 0x000107CE
		internal override void Grow(int desired)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000125CE File Offset: 0x000107CE
		protected override void OnAdd(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000125CE File Offset: 0x000107CE
		protected override void OnClear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000125CE File Offset: 0x000107CE
		protected override void OnInsert(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000125CE File Offset: 0x000107CE
		protected override void OnRemove(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x000125CE File Offset: 0x000107CE
		protected override void OnSet(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x04000207 RID: 519
		private static ReadOnlyCollection<T> empty;
	}
}
