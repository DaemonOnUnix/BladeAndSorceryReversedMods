using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C5 RID: 453
	internal sealed class FieldTable : MetadataTable<Row<FieldAttributes, uint, uint>>
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x0002095C File Offset: 0x0001EB5C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
