using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D7 RID: 215
	internal sealed class MemberRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x0001ACB0 File Offset: 0x00018EB0
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
