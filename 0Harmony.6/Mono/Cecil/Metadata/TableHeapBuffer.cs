using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019E RID: 414
	internal sealed class TableHeapBuffer : HeapBuffer
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00011F38 File Offset: 0x00010138
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002EAD4 File Offset: 0x0002CCD4
		public TableHeapBuffer(ModuleDefinition module, MetadataBuilder metadata)
			: base(24)
		{
			this.module = module;
			this.metadata = metadata;
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0002EB30 File Offset: 0x0002CD30
		private int GetTableLength(Table table)
		{
			return (int)this.table_infos[(int)table].Length;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public TTable GetTable<TTable>(Table table) where TTable : MetadataTable, new()
		{
			TTable ttable = (TTable)((object)this.tables[(int)table]);
			if (ttable != null)
			{
				return ttable;
			}
			ttable = new TTable();
			this.tables[(int)table] = ttable;
			return ttable;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002EB7E File Offset: 0x0002CD7E
		public void WriteBySize(uint value, int size)
		{
			if (size == 4)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002EB94 File Offset: 0x0002CD94
		public void WriteBySize(uint value, bool large)
		{
			if (large)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002EBA9 File Offset: 0x0002CDA9
		public void WriteString(uint @string)
		{
			this.WriteBySize(this.string_offsets[(int)@string], this.large_string);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0002EBBF File Offset: 0x0002CDBF
		public void WriteBlob(uint blob)
		{
			this.WriteBySize(blob, this.large_blob);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002EBCE File Offset: 0x0002CDCE
		public void WriteGuid(uint guid)
		{
			this.WriteBySize(guid, this.large_guid);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002EBDD File Offset: 0x0002CDDD
		public void WriteRID(uint rid, Table table)
		{
			this.WriteBySize(rid, this.table_infos[(int)table].IsLarge);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
		private int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002EC2C File Offset: 0x0002CE2C
		public void WriteCodedRID(uint rid, CodedIndex coded_index)
		{
			this.WriteBySize(rid, this.GetCodedIndexSize(coded_index));
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002EC3C File Offset: 0x0002CE3C
		public void WriteTableHeap()
		{
			base.WriteUInt32(0U);
			base.WriteByte(this.GetTableHeapVersion());
			base.WriteByte(0);
			base.WriteByte(this.GetHeapSizes());
			base.WriteByte(10);
			base.WriteUInt64(this.GetValid());
			base.WriteUInt64(55193285546867200UL);
			this.WriteRowCount();
			this.WriteTables();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002ECA0 File Offset: 0x0002CEA0
		private void WriteRowCount()
		{
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					base.WriteUInt32((uint)metadataTable.Length);
				}
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002ECE0 File Offset: 0x0002CEE0
		private void WriteTables()
		{
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					metadataTable.Write(this);
				}
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002ED1C File Offset: 0x0002CF1C
		private ulong GetValid()
		{
			ulong num = 0UL;
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					metadataTable.Sort();
					num |= 1UL << i;
				}
			}
			return num;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002ED64 File Offset: 0x0002CF64
		public void ComputeTableInformations()
		{
			if (this.metadata.metadata_builder != null)
			{
				this.ComputeTableInformations(this.metadata.metadata_builder.table_heap);
			}
			this.ComputeTableInformations(this.metadata.table_heap);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002ED9C File Offset: 0x0002CF9C
		private void ComputeTableInformations(TableHeapBuffer table_heap)
		{
			MetadataTable[] array = table_heap.tables;
			for (int i = 0; i < array.Length; i++)
			{
				MetadataTable metadataTable = array[i];
				if (metadataTable != null && metadataTable.Length > 0)
				{
					this.table_infos[i].Length = (uint)metadataTable.Length;
				}
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002EDE8 File Offset: 0x0002CFE8
		private byte GetHeapSizes()
		{
			byte b = 0;
			if (this.metadata.string_heap.IsLarge)
			{
				this.large_string = true;
				b |= 1;
			}
			if (this.metadata.guid_heap.IsLarge)
			{
				this.large_guid = true;
				b |= 2;
			}
			if (this.metadata.blob_heap.IsLarge)
			{
				this.large_blob = true;
				b |= 4;
			}
			return b;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002EE54 File Offset: 0x0002D054
		private byte GetTableHeapVersion()
		{
			TargetRuntime runtime = this.module.Runtime;
			if (runtime <= TargetRuntime.Net_1_1)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002EE74 File Offset: 0x0002D074
		public void FixupData(uint data_rva)
		{
			FieldRVATable table = this.GetTable<FieldRVATable>(Table.FieldRVA);
			if (table.length == 0)
			{
				return;
			}
			int num = (this.GetTable<FieldTable>(Table.Field).IsLarge ? 4 : 2);
			int position = this.position;
			this.position = table.position;
			for (int i = 0; i < table.length; i++)
			{
				uint num2 = base.ReadUInt32();
				this.position -= 4;
				base.WriteUInt32(num2 + data_rva);
				this.position += num;
			}
			this.position = position;
		}

		// Token: 0x040005FD RID: 1533
		private readonly ModuleDefinition module;

		// Token: 0x040005FE RID: 1534
		private readonly MetadataBuilder metadata;

		// Token: 0x040005FF RID: 1535
		internal readonly TableInformation[] table_infos = new TableInformation[58];

		// Token: 0x04000600 RID: 1536
		internal readonly MetadataTable[] tables = new MetadataTable[58];

		// Token: 0x04000601 RID: 1537
		private bool large_string;

		// Token: 0x04000602 RID: 1538
		private bool large_blob;

		// Token: 0x04000603 RID: 1539
		private bool large_guid;

		// Token: 0x04000604 RID: 1540
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x04000605 RID: 1541
		private readonly Func<Table, int> counter;

		// Token: 0x04000606 RID: 1542
		internal uint[] string_offsets;
	}
}
