using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Mono.Collections.Generic
{
	// Token: 0x020001A9 RID: 425
	internal sealed class ReadOnlyCollection<T> : Collection<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x000183C8 File Offset: 0x000165C8
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

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x000183ED File Offset: 0x000165ED
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x000183ED File Offset: 0x000165ED
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x000183ED File Offset: 0x000165ED
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000183F0 File Offset: 0x000165F0
		private ReadOnlyCollection()
		{
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000183F8 File Offset: 0x000165F8
		public ReadOnlyCollection(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(array, array.Length);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018413 File Offset: 0x00016613
		public ReadOnlyCollection(Collection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException();
			}
			this.Initialize(collection.items, collection.size);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00018436 File Offset: 0x00016636
		private void Initialize(T[] items, int size)
		{
			this.items = new T[size];
			Array.Copy(items, 0, this.items, 0, size);
			this.size = size;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001845A File Offset: 0x0001665A
		internal override void Grow(int desired)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001845A File Offset: 0x0001665A
		protected override void OnAdd(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001845A File Offset: 0x0001665A
		protected override void OnClear()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001845A File Offset: 0x0001665A
		protected override void OnInsert(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001845A File Offset: 0x0001665A
		protected override void OnRemove(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001845A File Offset: 0x0001665A
		protected override void OnSet(T item, int index)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x04000235 RID: 565
		private static ReadOnlyCollection<T> empty;
	}
}
