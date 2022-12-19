using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D9 RID: 217
	internal sealed class CustomAttributeTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005DA RID: 1498 RVA: 0x0001AD94 File Offset: 0x00018F94
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomAttribute);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.CustomAttributeType);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001ADFA File Offset: 0x00018FFA
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
