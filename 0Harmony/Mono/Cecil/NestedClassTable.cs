using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E0 RID: 480
	internal sealed class NestedClassTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x00021590 File Offset: 0x0001F790
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.TypeDef);
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00020CF9 File Offset: 0x0001EEF9
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
