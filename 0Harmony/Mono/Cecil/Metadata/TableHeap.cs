using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002AC RID: 684
	internal sealed class TableHeap : Heap
	{
		// Token: 0x170004A6 RID: 1190
		public TableInformation this[Table table]
		{
			get
			{
				return this.Tables[(int)table];
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0003701A File Offset: 0x0003521A
		public TableHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00037030 File Offset: 0x00035230
		public bool HasTable(Table table)
		{
			return (this.Valid & (1L << (int)table)) != 0L;
		}

		// Token: 0x040006D5 RID: 1749
		public long Valid;

		// Token: 0x040006D6 RID: 1750
		public long Sorted;

		// Token: 0x040006D7 RID: 1751
		public readonly TableInformation[] Tables = new TableInformation[58];
	}
}
