using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D8 RID: 216
	internal sealed class ConstantTable : SortedTable<Row<ElementType, uint, uint>>
	{
		// Token: 0x060005D7 RID: 1495 RVA: 0x0001AD14 File Offset: 0x00018F14
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasConstant);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001AD78 File Offset: 0x00018F78
		public override int Compare(Row<ElementType, uint, uint> x, Row<ElementType, uint, uint> y)
		{
			return SortedTable<Row<ElementType, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
