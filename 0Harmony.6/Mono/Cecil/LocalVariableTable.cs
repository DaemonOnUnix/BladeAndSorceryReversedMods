using System;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F5 RID: 245
	internal sealed class LocalVariableTable : MetadataTable<Row<VariableAttributes, ushort, uint>>
	{
		// Token: 0x0600061B RID: 1563 RVA: 0x0001BA04 File Offset: 0x00019C04
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteUInt16(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
			}
		}
	}
}
