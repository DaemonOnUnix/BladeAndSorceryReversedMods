using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EE RID: 238
	internal sealed class NestedClassTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x0001B6FC File Offset: 0x000198FC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.TypeDef);
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001AE65 File Offset: 0x00019065
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
