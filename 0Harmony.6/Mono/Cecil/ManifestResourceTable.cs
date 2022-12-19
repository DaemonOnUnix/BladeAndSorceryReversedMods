using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000ED RID: 237
	internal sealed class ManifestResourceTable : MetadataTable<Row<uint, ManifestResourceAttributes, uint, uint>>
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x0001B678 File Offset: 0x00019878
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteUInt32((uint)this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteCodedRID(this.rows[i].Col4, CodedIndex.Implementation);
			}
		}
	}
}
