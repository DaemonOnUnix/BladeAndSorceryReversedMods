using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CA RID: 458
	internal sealed class ConstantTable : SortedTable<Row<ElementType, uint, uint>>
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x00020BA8 File Offset: 0x0001EDA8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasConstant);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00020C0C File Offset: 0x0001EE0C
		public override int Compare(Row<ElementType, uint, uint> x, Row<ElementType, uint, uint> y)
		{
			return SortedTable<Row<ElementType, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
