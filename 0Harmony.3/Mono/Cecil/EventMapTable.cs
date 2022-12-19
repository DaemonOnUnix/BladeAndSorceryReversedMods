using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DF RID: 223
	internal sealed class EventMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x0001B014 File Offset: 0x00019214
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
