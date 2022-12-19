using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E9 RID: 489
	internal sealed class ImportScopeTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x00021950 File Offset: 0x0001FB50
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
