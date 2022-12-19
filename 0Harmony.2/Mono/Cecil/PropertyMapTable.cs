using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D3 RID: 467
	internal sealed class PropertyMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x00020F64 File Offset: 0x0001F164
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.Property);
			}
		}
	}
}
