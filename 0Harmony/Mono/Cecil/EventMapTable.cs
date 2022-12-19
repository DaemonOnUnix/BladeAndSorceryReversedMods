using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D1 RID: 465
	internal sealed class EventMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000923 RID: 2339 RVA: 0x00020EA8 File Offset: 0x0001F0A8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.Event);
			}
		}
	}
}
