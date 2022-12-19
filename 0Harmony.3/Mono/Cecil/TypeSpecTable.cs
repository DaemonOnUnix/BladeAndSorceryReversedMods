using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E6 RID: 230
	internal sealed class TypeSpecTable : MetadataTable<uint>
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x0001B2A0 File Offset: 0x000194A0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
