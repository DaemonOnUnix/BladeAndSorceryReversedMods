using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000CF RID: 207
	internal abstract class SortedTable<TRow> : MetadataTable<TRow>, IComparer<TRow> where TRow : struct
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001A92C File Offset: 0x00018B2C
		public sealed override void Sort()
		{
			MergeSort<TRow>.Sort(this.rows, 0, this.length, this);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001A941 File Offset: 0x00018B41
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

		// Token: 0x060005C5 RID: 1477
		public abstract int Compare(TRow x, TRow y);
	}
}
