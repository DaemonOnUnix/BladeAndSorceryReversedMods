using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EC RID: 236
	internal sealed class ExportedTypeTable : MetadataTable<Row<TypeAttributes, uint, uint, uint, uint>>
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x0001B5D8 File Offset: 0x000197D8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteString(this.rows[i].Col4);
				buffer.WriteCodedRID(this.rows[i].Col5, CodedIndex.Implementation);
			}
		}
	}
}
