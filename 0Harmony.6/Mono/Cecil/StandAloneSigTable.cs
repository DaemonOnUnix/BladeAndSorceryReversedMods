using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DE RID: 222
	internal sealed class StandAloneSigTable : MetadataTable<uint>
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0001AFE0 File Offset: 0x000191E0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
