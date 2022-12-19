using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E8 RID: 232
	internal sealed class FieldRVATable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0001B364 File Offset: 0x00019564
		public override void Write(TableHeapBuffer buffer)
		{
			this.position = buffer.position;
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001AFCD File Offset: 0x000191CD
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}

		// Token: 0x04000263 RID: 611
		internal int position;
	}
}
