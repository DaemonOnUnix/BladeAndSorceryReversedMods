using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001DD RID: 477
	internal sealed class FileTable : MetadataTable<Row<FileAttributes, uint, uint>>
	{
		// Token: 0x0600093E RID: 2366 RVA: 0x00021400 File Offset: 0x0001F600
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
