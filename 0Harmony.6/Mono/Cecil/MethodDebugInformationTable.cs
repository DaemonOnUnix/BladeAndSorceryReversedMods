using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F3 RID: 243
	internal sealed class MethodDebugInformationTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x0001B8F4 File Offset: 0x00019AF4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.Document);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
