using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E4 RID: 228
	internal sealed class MethodImplTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x0001B20C File Offset: 0x0001940C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.MethodDefOrRef);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.MethodDefOrRef);
			}
		}
	}
}
