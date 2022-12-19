using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D6 RID: 214
	internal sealed class InterfaceImplTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005D3 RID: 1491 RVA: 0x0001AC58 File Offset: 0x00018E58
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
