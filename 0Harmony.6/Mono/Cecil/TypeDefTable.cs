using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D2 RID: 210
	internal sealed class TypeDefTable : MetadataTable<Row<TypeAttributes, uint, uint, uint, uint, uint>>
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0001AA0C File Offset: 0x00018C0C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteCodedRID(this.rows[i].Col4, CodedIndex.TypeDefOrRef);
				buffer.WriteRID(this.rows[i].Col5, Table.Field);
				buffer.WriteRID(this.rows[i].Col6, Table.Method);
			}
		}
	}
}
