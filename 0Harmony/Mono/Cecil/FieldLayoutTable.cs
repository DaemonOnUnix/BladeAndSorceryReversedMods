using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CF RID: 463
	internal sealed class FieldLayoutTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x0600091E RID: 2334 RVA: 0x00020E14 File Offset: 0x0001F014
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00020E61 File Offset: 0x0001F061
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
