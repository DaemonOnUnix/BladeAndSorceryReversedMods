using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F9 RID: 249
	internal sealed class CustomDebugInformationTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x06000623 RID: 1571 RVA: 0x0001BB5C File Offset: 0x00019D5C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomDebugInformation);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001ADFA File Offset: 0x00018FFA
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
