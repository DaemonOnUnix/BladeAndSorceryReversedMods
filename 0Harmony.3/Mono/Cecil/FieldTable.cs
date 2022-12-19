using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D3 RID: 211
	internal sealed class FieldTable : MetadataTable<Row<FieldAttributes, uint, uint>>
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x0001AAC8 File Offset: 0x00018CC8
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
