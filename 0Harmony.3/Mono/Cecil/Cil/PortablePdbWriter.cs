using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D4 RID: 468
	internal sealed class PortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00033A82 File Offset: 0x00031C82
		private bool IsEmbedded
		{
			get
			{
				return this.writer == null;
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00033A90 File Offset: 0x00031C90
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module)
		{
			this.pdb_metadata = pdb_metadata;
			this.module = module;
			this.module_metadata = module.metadata_builder;
			if (this.module_metadata != pdb_metadata)
			{
				this.pdb_metadata.metadata_builder = this.module_metadata;
			}
			pdb_metadata.AddCustomDebugInformations(module);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00033ADE File Offset: 0x00031CDE
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module, ImageWriter writer)
			: this(pdb_metadata, module)
		{
			this.writer = writer;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00033AEF File Offset: 0x00031CEF
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new PortablePdbReaderProvider();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00033AF8 File Offset: 0x00031CF8
		public ImageDebugHeader GetDebugHeader()
		{
			if (this.IsEmbedded)
			{
				return new ImageDebugHeader();
			}
			ImageDebugDirectory imageDebugDirectory = new ImageDebugDirectory
			{
				MajorVersion = 256,
				MinorVersion = 20557,
				Type = ImageDebugType.CodeView,
				TimeDateStamp = (int)this.module.timestamp
			};
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteUInt32(1396986706U);
			byteBuffer.WriteBytes(this.module.Mvid.ToByteArray());
			byteBuffer.WriteUInt32(1U);
			byteBuffer.WriteBytes(Encoding.UTF8.GetBytes(this.writer.BaseStream.GetFileName()));
			byteBuffer.WriteByte(0);
			byte[] array = new byte[byteBuffer.length];
			Buffer.BlockCopy(byteBuffer.buffer, 0, array, 0, byteBuffer.length);
			imageDebugDirectory.SizeOfData = array.Length;
			return new ImageDebugHeader(new ImageDebugHeaderEntry(imageDebugDirectory, array));
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00033BDE File Offset: 0x00031DDE
		public void Write(MethodDebugInformation info)
		{
			this.CheckMethodDebugInformationTable();
			this.pdb_metadata.AddMethodDebugInformation(info);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00033BF4 File Offset: 0x00031DF4
		private void CheckMethodDebugInformationTable()
		{
			MethodDebugInformationTable table = this.pdb_metadata.table_heap.GetTable<MethodDebugInformationTable>(Table.MethodDebugInformation);
			if (table.length > 0)
			{
				return;
			}
			table.rows = new Row<uint, uint>[this.module_metadata.method_rid - 1U];
			table.length = table.rows.Length;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00033C44 File Offset: 0x00031E44
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.WritePdbFile();
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00033C58 File Offset: 0x00031E58
		private void WritePdbFile()
		{
			this.WritePdbHeap();
			this.WriteTableHeap();
			this.writer.BuildMetadataTextMap();
			this.writer.WriteMetadataHeader();
			this.writer.WriteMetadata();
			this.writer.Flush();
			this.writer.stream.Dispose();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00033CB0 File Offset: 0x00031EB0
		private void WritePdbHeap()
		{
			PdbHeapBuffer pdb_heap = this.pdb_metadata.pdb_heap;
			pdb_heap.WriteBytes(this.module.Mvid.ToByteArray());
			pdb_heap.WriteUInt32(this.module_metadata.timestamp);
			pdb_heap.WriteUInt32(this.module_metadata.entry_point.ToUInt32());
			MetadataTable[] tables = this.module_metadata.table_heap.tables;
			ulong num = 0UL;
			for (int i = 0; i < tables.Length; i++)
			{
				if (tables[i] != null && tables[i].Length != 0)
				{
					num |= 1UL << i;
				}
			}
			pdb_heap.WriteUInt64(num);
			for (int j = 0; j < tables.Length; j++)
			{
				if (tables[j] != null && tables[j].Length != 0)
				{
					pdb_heap.WriteUInt32((uint)tables[j].Length);
				}
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00033D84 File Offset: 0x00031F84
		private void WriteTableHeap()
		{
			this.pdb_metadata.table_heap.string_offsets = this.pdb_metadata.string_heap.WriteStrings();
			this.pdb_metadata.table_heap.ComputeTableInformations();
			this.pdb_metadata.table_heap.WriteTableHeap();
		}

		// Token: 0x040008F1 RID: 2289
		private readonly MetadataBuilder pdb_metadata;

		// Token: 0x040008F2 RID: 2290
		private readonly ModuleDefinition module;

		// Token: 0x040008F3 RID: 2291
		private readonly ImageWriter writer;

		// Token: 0x040008F4 RID: 2292
		private MetadataBuilder module_metadata;
	}
}
