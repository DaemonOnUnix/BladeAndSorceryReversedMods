using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C2 RID: 450
	internal sealed class ModuleTable : OneRowTable<Row<uint, uint>>
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x000207EC File Offset: 0x0001E9EC
		public override void Write(TableHeapBuffer buffer)
		{
			buffer.WriteUInt16(0);
			buffer.WriteString(this.row.Col1);
			buffer.WriteGuid(this.row.Col2);
			buffer.WriteUInt16(0);
			buffer.WriteUInt16(0);
		}
	}
}
