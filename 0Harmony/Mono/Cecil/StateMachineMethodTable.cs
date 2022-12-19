using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001EA RID: 490
	internal sealed class StateMachineMethodTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x000219A0 File Offset: 0x0001FBA0
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
