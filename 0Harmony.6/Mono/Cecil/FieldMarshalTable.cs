using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DA RID: 218
	internal sealed class FieldMarshalTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0001AE18 File Offset: 0x00019018
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasFieldMarshal);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001AE65 File Offset: 0x00019065
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
