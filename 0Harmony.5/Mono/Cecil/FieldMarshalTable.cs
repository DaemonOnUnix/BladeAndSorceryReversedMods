using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CC RID: 460
	internal sealed class FieldMarshalTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000915 RID: 2325 RVA: 0x00020CAC File Offset: 0x0001EEAC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasFieldMarshal);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00020CF9 File Offset: 0x0001EEF9
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
