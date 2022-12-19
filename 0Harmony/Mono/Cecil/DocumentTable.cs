using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E4 RID: 484
	internal sealed class DocumentTable : MetadataTable<Row<uint, uint, uint, uint>>
	{
		// Token: 0x0600094D RID: 2381 RVA: 0x00021704 File Offset: 0x0001F904
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
