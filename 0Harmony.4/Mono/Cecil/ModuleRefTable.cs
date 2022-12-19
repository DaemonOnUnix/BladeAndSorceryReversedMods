using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D7 RID: 471
	internal sealed class ModuleRefTable : MetadataTable<uint>
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x00021108 File Offset: 0x0001F308
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i]);
			}
		}
	}
}
