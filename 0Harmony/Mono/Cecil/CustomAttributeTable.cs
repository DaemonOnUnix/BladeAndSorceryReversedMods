using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CB RID: 459
	internal sealed class CustomAttributeTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x00020C28 File Offset: 0x0001EE28
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomAttribute);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.CustomAttributeType);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00020C8E File Offset: 0x0001EE8E
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
