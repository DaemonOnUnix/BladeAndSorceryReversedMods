using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D5 RID: 469
	internal sealed class MethodSemanticsTable : SortedTable<Row<MethodSemanticsAttributes, uint, uint>>
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x00021020 File Offset: 0x0001F220
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.HasSemantics);
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00021085 File Offset: 0x0001F285
		public override int Compare(Row<MethodSemanticsAttributes, uint, uint> x, Row<MethodSemanticsAttributes, uint, uint> y)
		{
			return SortedTable<Row<MethodSemanticsAttributes, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
