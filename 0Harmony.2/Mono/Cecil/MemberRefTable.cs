using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C9 RID: 457
	internal sealed class MemberRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600090D RID: 2317 RVA: 0x00020B44 File Offset: 0x0001ED44
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.MemberRefParent);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
