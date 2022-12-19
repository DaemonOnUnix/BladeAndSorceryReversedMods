using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CE RID: 462
	internal sealed class ClassLayoutTable : SortedTable<Row<ushort, uint, uint>>
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x00020D94 File Offset: 0x0001EF94
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteRID(this.rows[i].Col3, Table.TypeDef);
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00020DF8 File Offset: 0x0001EFF8
		public override int Compare(Row<ushort, uint, uint> x, Row<ushort, uint, uint> y)
		{
			return SortedTable<Row<ushort, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
