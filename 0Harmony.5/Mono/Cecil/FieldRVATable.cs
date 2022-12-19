using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001DA RID: 474
	internal sealed class FieldRVATable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x000211F8 File Offset: 0x0001F3F8
		public override void Write(TableHeapBuffer buffer)
		{
			this.position = buffer.position;
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00020E61 File Offset: 0x0001F061
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}

		// Token: 0x04000295 RID: 661
		internal int position;
	}
}
