using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F6 RID: 246
	internal sealed class LocalConstantTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x0001BA70 File Offset: 0x00019C70
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i].Col1);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
