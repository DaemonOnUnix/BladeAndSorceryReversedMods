using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001D8 RID: 472
	internal sealed class TypeSpecTable : MetadataTable<uint>
	{
		// Token: 0x06000932 RID: 2354 RVA: 0x00021134 File Offset: 0x0001F334
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
