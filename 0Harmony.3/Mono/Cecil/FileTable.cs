using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EB RID: 235
	internal sealed class FileTable : MetadataTable<Row<FileAttributes, uint, uint>>
	{
		// Token: 0x06000606 RID: 1542 RVA: 0x0001B56C File Offset: 0x0001976C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
