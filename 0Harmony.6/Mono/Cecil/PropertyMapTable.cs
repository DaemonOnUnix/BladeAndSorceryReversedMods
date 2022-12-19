using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E1 RID: 225
	internal sealed class PropertyMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x0001B0D0 File Offset: 0x000192D0
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
