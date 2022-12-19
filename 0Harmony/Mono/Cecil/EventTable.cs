using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D2 RID: 466
	internal sealed class EventTable : MetadataTable<Row<EventAttributes, uint, uint>>
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x00020EF8 File Offset: 0x0001F0F8
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
