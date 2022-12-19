using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E8 RID: 488
	internal sealed class LocalConstantTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00021904 File Offset: 0x0001FB04
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
