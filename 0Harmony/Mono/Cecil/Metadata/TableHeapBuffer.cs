using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000293 RID: 659
	internal sealed class TableHeapBuffer : HeapBuffer
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x000364FC File Offset: 0x000346FC
		public TableHeapBuffer(ModuleDefinition module, MetadataBuilder metadata)
			: base(24)
		{
			this.module = module;
			this.metadata = metadata;
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00036558 File Offset: 0x00034758
		private int GetTableLength(Table table)
		{
			return (int)this.table_infos[(int)table].Length;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0003656C File Offset: 0x0003476C
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

		// Token: 0x060010DD RID: 4317 RVA: 0x000365A6 File Offset: 0x000347A6
		public void WriteBySize(uint value, int size)
		{
			if (size == 4)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x000365BC File Offset: 0x000347BC
		public void WriteBySize(uint value, bool large)
		{
			if (large)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x000365D1 File Offset: 0x000347D1
		public void WriteString(uint @string)
		{
			this.WriteBySize(this.string_offsets[(int)@string], this.large_string);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x000365E7 File Offset: 0x000347E7
		public void WriteBlob(uint blob)
		{
			this.WriteBySize(blob, this.large_blob);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x000365F6 File Offset: 0x000347F6
		public void WriteGuid(uint guid)
		{
			this.WriteBySize(guid, this.large_guid);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00036605 File Offset: 0x00034805
		public void WriteRID(uint rid, Table table)
		{
			this.WriteBySize(rid, this.table_infos[(int)table].IsLarge);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00036620 File Offset: 0x00034820
		private int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00036654 File Offset: 0x00034854
		public void WriteCodedRID(uint rid, CodedIndex coded_index)
		{
			this.WriteBySize(rid, this.GetCodedIndexSize(coded_index));
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00036664 File Offset: 0x00034864
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

		// Token: 0x060010E6 RID: 4326 RVA: 0x000366C8 File Offset: 0x000348C8
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

		// Token: 0x060010E7 RID: 4327 RVA: 0x00036708 File Offset: 0x00034908
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

		// Token: 0x060010E8 RID: 4328 RVA: 0x00036744 File Offset: 0x00034944
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

		// Token: 0x060010E9 RID: 4329 RVA: 0x0003678C File Offset: 0x0003498C
		public void ComputeTableInformations()
		{
			if (this.metadata.metadata_builder != null)
			{
				this.ComputeTableInformations(this.metadata.metadata_builder.table_heap);
			}
			this.ComputeTableInformations(this.metadata.table_heap);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x000367C4 File Offset: 0x000349C4
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

		// Token: 0x060010EB RID: 4331 RVA: 0x00036810 File Offset: 0x00034A10
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

		// Token: 0x060010EC RID: 4332 RVA: 0x0003687C File Offset: 0x00034A7C
		private byte GetTableHeapVersion()
		{
			TargetRuntime runtime = this.module.Runtime;
			if (runtime <= TargetRuntime.Net_1_1)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0003689C File Offset: 0x00034A9C
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

		// Token: 0x04000635 RID: 1589
		private readonly ModuleDefinition module;

		// Token: 0x04000636 RID: 1590
		private readonly MetadataBuilder metadata;

		// Token: 0x04000637 RID: 1591
		internal readonly TableInformation[] table_infos = new TableInformation[58];

		// Token: 0x04000638 RID: 1592
		internal readonly MetadataTable[] tables = new MetadataTable[58];

		// Token: 0x04000639 RID: 1593
		private bool large_string;

		// Token: 0x0400063A RID: 1594
		private bool large_blob;

		// Token: 0x0400063B RID: 1595
		private bool large_guid;

		// Token: 0x0400063C RID: 1596
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x0400063D RID: 1597
		private readonly Func<Table, int> counter;

		// Token: 0x0400063E RID: 1598
		internal uint[] string_offsets;
	}
}
