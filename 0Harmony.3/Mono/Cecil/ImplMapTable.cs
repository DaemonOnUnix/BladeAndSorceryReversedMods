using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E7 RID: 231
	internal sealed class ImplMapTable : SortedTable<Row<PInvokeAttributes, uint, uint, uint>>
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x0001B2CC File Offset: 0x000194CC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.MemberForwarded);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteRID(this.rows[i].Col4, Table.ModuleRef);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001B349 File Offset: 0x00019549
		public override int Compare(Row<PInvokeAttributes, uint, uint, uint> x, Row<PInvokeAttributes, uint, uint, uint> y)
		{
			return SortedTable<Row<PInvokeAttributes, uint, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
