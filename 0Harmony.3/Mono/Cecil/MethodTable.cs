using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D4 RID: 212
	internal sealed class MethodTable : MetadataTable<Row<uint, MethodImplAttributes, MethodAttributes, uint, uint, uint>>
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x0001AB34 File Offset: 0x00018D34
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteUInt16((ushort)this.rows[i].Col2);
				buffer.WriteUInt16((ushort)this.rows[i].Col3);
				buffer.WriteString(this.rows[i].Col4);
				buffer.WriteBlob(this.rows[i].Col5);
				buffer.WriteRID(this.rows[i].Col6, Table.Param);
			}
		}
	}
}
