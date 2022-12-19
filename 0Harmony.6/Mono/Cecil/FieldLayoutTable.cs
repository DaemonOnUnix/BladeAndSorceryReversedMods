using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DD RID: 221
	internal sealed class FieldLayoutTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x0001AF80 File Offset: 0x00019180
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001AFCD File Offset: 0x000191CD
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
