using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C3 RID: 451
	internal sealed class TypeRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x00020830 File Offset: 0x0001EA30
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
