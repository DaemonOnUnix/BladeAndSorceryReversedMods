using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D6 RID: 470
	internal sealed class MethodImplTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x000210A0 File Offset: 0x0001F2A0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.MethodDefOrRef);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.MethodDefOrRef);
			}
		}
	}
}
