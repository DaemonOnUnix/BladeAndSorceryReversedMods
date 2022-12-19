using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D0 RID: 208
	internal sealed class ModuleTable : OneRowTable<Row<uint, uint>>
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x0001A958 File Offset: 0x00018B58
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
