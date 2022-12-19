using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F7 RID: 247
	internal sealed class ImportScopeTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600061F RID: 1567 RVA: 0x0001BABC File Offset: 0x00019CBC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.ImportScope);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
