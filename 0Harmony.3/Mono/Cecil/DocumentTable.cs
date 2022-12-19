using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F2 RID: 242
	internal sealed class DocumentTable : MetadataTable<Row<uint, uint, uint, uint>>
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x0001B870 File Offset: 0x00019A70
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i].Col1);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
				buffer.WriteGuid(this.rows[i].Col4);
			}
		}
	}
}
