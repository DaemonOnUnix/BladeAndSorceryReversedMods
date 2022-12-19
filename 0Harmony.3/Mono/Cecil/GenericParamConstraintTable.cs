using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F1 RID: 241
	internal sealed class GenericParamConstraintTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x0001B820 File Offset: 0x00019A20
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.GenericParam);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
