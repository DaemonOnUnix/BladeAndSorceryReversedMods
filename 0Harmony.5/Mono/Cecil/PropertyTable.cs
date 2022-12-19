using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D4 RID: 468
	internal sealed class PropertyTable : MetadataTable<Row<PropertyAttributes, uint, uint>>
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x00020FB4 File Offset: 0x0001F1B4
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
