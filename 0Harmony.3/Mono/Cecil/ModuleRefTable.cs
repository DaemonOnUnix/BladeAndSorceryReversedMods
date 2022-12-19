using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E5 RID: 229
	internal sealed class ModuleRefTable : MetadataTable<uint>
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0001B274 File Offset: 0x00019474
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i]);
			}
		}
	}
}
