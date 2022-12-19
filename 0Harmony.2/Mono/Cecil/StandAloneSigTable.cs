using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D0 RID: 464
	internal sealed class StandAloneSigTable : MetadataTable<uint>
	{
		// Token: 0x06000921 RID: 2337 RVA: 0x00020E74 File Offset: 0x0001F074
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
