using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DC RID: 220
	internal sealed class ClassLayoutTable : SortedTable<Row<ushort, uint, uint>>
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x0001AF00 File Offset: 0x00019100
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteRID(this.rows[i].Col3, Table.TypeDef);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001AF64 File Offset: 0x00019164
		public override int Compare(Row<ushort, uint, uint> x, Row<ushort, uint, uint> y)
		{
			return SortedTable<Row<ushort, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
