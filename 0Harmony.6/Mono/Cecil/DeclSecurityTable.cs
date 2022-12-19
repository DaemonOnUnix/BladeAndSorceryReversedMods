using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DB RID: 219
	internal sealed class DeclSecurityTable : SortedTable<Row<SecurityAction, uint, uint>>
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0001AE80 File Offset: 0x00019080
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasDeclSecurity);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001AEE4 File Offset: 0x000190E4
		public override int Compare(Row<SecurityAction, uint, uint> x, Row<SecurityAction, uint, uint> y)
		{
			return SortedTable<Row<SecurityAction, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
