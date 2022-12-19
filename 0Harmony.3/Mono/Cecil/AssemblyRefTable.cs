using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EA RID: 234
	internal sealed class AssemblyRefTable : MetadataTable<Row<ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint, uint>>
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0001B470 File Offset: 0x00019670
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt16(this.rows[i].Col2);
				buffer.WriteUInt16(this.rows[i].Col3);
				buffer.WriteUInt16(this.rows[i].Col4);
				buffer.WriteUInt32((uint)this.rows[i].Col5);
				buffer.WriteBlob(this.rows[i].Col6);
				buffer.WriteString(this.rows[i].Col7);
				buffer.WriteString(this.rows[i].Col8);
				buffer.WriteBlob(this.rows[i].Col9);
			}
		}
	}
}
