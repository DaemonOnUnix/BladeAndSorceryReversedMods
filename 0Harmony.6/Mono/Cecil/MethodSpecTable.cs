using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F0 RID: 240
	internal sealed class MethodSpecTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0001B7D0 File Offset: 0x000199D0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.MethodDefOrRef);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
