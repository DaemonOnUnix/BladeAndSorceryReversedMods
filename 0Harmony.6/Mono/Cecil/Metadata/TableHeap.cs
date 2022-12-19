using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B7 RID: 439
	internal sealed class TableHeap : Heap
	{
		// Token: 0x170002D6 RID: 726
		public TableInformation this[Table table]
		{
			get
			{
				return this.Tables[(int)table];
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002F5F2 File Offset: 0x0002D7F2
		public TableHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002F608 File Offset: 0x0002D808
		public bool HasTable(Table table)
		{
			return (this.Valid & (1L << (int)table)) != 0L;
		}

		// Token: 0x0400069D RID: 1693
		public long Valid;

		// Token: 0x0400069E RID: 1694
		public long Sorted;

		// Token: 0x0400069F RID: 1695
		public readonly TableInformation[] Tables = new TableInformation[58];
	}
}
