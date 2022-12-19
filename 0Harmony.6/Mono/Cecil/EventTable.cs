using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E0 RID: 224
	internal sealed class EventTable : MetadataTable<Row<EventAttributes, uint, uint>>
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0001B064 File Offset: 0x00019264
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
