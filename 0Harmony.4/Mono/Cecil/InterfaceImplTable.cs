using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C8 RID: 456
	internal sealed class InterfaceImplTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x00020AEC File Offset: 0x0001ECEC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
