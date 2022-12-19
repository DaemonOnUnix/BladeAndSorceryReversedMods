using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E2 RID: 482
	internal sealed class MethodSpecTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x00021664 File Offset: 0x0001F864
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
