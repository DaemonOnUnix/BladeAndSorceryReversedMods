using System;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001E7 RID: 487
	internal sealed class LocalVariableTable : MetadataTable<Row<VariableAttributes, ushort, uint>>
	{
		// Token: 0x06000953 RID: 2387 RVA: 0x00021898 File Offset: 0x0001FA98
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
