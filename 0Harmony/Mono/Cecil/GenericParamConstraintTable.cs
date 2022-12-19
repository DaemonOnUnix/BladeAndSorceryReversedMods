using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E3 RID: 483
	internal sealed class GenericParamConstraintTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x000216B4 File Offset: 0x0001F8B4
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
