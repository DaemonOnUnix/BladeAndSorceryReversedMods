using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E3 RID: 227
	internal sealed class MethodSemanticsTable : SortedTable<Row<MethodSemanticsAttributes, uint, uint>>
	{
		// Token: 0x060005F3 RID: 1523 RVA: 0x0001B18C File Offset: 0x0001938C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.HasSemantics);
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001B1F1 File Offset: 0x000193F1
		public override int Compare(Row<MethodSemanticsAttributes, uint, uint> x, Row<MethodSemanticsAttributes, uint, uint> y)
		{
			return SortedTable<Row<MethodSemanticsAttributes, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
