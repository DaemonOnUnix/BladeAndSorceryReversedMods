using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E5 RID: 485
	internal sealed class MethodDebugInformationTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x00021788 File Offset: 0x0001F988
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
