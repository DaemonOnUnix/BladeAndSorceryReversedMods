using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D1 RID: 209
	internal sealed class TypeRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x0001A99C File Offset: 0x00018B9C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.ResolutionScope);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
			}
		}
	}
}
