using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D9 RID: 473
	internal sealed class ImplMapTable : SortedTable<Row<PInvokeAttributes, uint, uint, uint>>
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x00021160 File Offset: 0x0001F360
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

		// Token: 0x06000935 RID: 2357 RVA: 0x000211DD File Offset: 0x0001F3DD
		public override int Compare(Row<PInvokeAttributes, uint, uint, uint> x, Row<PInvokeAttributes, uint, uint, uint> y)
		{
			return SortedTable<Row<PInvokeAttributes, uint, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
