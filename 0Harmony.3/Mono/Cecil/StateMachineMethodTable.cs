using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F8 RID: 248
	internal sealed class StateMachineMethodTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000621 RID: 1569 RVA: 0x0001BB0C File Offset: 0x00019D0C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.Method);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
			}
		}
	}
}
