﻿using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001C4 RID: 452
	internal sealed class TypeDefTable : MetadataTable<Row<TypeAttributes, uint, uint, uint, uint, uint>>
	{
		// Token: 0x06000903 RID: 2307 RVA: 0x000208A0 File Offset: 0x0001EAA0
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
