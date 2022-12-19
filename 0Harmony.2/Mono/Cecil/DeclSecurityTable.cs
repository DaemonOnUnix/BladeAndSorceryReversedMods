using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001CD RID: 461
	internal sealed class DeclSecurityTable : SortedTable<Row<SecurityAction, uint, uint>>
	{
		// Token: 0x06000918 RID: 2328 RVA: 0x00020D14 File Offset: 0x0001EF14
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasDeclSecurity);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00020D78 File Offset: 0x0001EF78
		public override int Compare(Row<SecurityAction, uint, uint> x, Row<SecurityAction, uint, uint> y)
		{
			return SortedTable<Row<SecurityAction, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
