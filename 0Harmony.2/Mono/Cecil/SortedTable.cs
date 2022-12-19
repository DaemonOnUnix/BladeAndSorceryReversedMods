using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001C1 RID: 449
	internal abstract class SortedTable<TRow> : MetadataTable<TRow>, IComparer<TRow> where TRow : struct
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x000207C0 File Offset: 0x0001E9C0
		public sealed override void Sort()
		{
			MergeSort<TRow>.Sort(this.rows, 0, this.length, this);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000207D5 File Offset: 0x0001E9D5
		protected static int Compare(uint x, uint y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x <= y)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060008FD RID: 2301
		public abstract int Compare(TRow x, TRow y);
	}
}
