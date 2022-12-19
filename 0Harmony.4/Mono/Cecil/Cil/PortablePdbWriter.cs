using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002CA RID: 714
	internal sealed class PortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x0003B8AA File Offset: 0x00039AAA
		private bool IsEmbedded
		{
			get
			{
				return this.writer == null;
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0003B8B8 File Offset: 0x00039AB8
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

		// Token: 0x0600123F RID: 4671 RVA: 0x0003B906 File Offset: 0x00039B06
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module, ImageWriter writer)
			: this(pdb_metadata, module)
		{
			this.writer = writer;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0003B917 File Offset: 0x00039B17
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new PortablePdbReaderProvider();
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0003B920 File Offset: 0x00039B20
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
			string text = this.writer.BaseStream.GetFileName();
			if (string.IsNullOrEmpty(text))
			{
				text = this.module.Assembly.Name.Name + ".pdb";
			}
			byteBuffer.WriteBytes(Encoding.UTF8.GetBytes(text));
			byteBuffer.WriteByte(0);
			byte[] array = new byte[byteBuffer.length];
			Buffer.BlockCopy(byteBuffer.buffer, 0, array, 0, byteBuffer.length);
			imageDebugDirectory.SizeOfData = array.Length;
			return new ImageDebugHeader(new ImageDebugHeaderEntry(imageDebugDirectory, array));
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0003BA31 File Offset: 0x00039C31
		public void Write(MethodDebugInformation info)
		{
			this.CheckMethodDebugInformationTable();
			this.pdb_metadata.AddMethodDebugInformation(info);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0003BA48 File Offset: 0x00039C48
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

		// Token: 0x06001244 RID: 4676 RVA: 0x0003BA98 File Offset: 0x00039C98
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.WritePdbFile();
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0003BAAC File Offset: 0x00039CAC
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

		// Token: 0x06001246 RID: 4678 RVA: 0x0003BB04 File Offset: 0x00039D04
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

		// Token: 0x06001247 RID: 4679 RVA: 0x0003BBD8 File Offset: 0x00039DD8
		private void WriteTableHeap()
		{
			this.pdb_metadata.table_heap.string_offsets = this.pdb_metadata.string_heap.WriteStrings();
			this.pdb_metadata.table_heap.ComputeTableInformations();
			this.pdb_metadata.table_heap.WriteTableHeap();
		}

		// Token: 0x0400092D RID: 2349
		private readonly MetadataBuilder pdb_metadata;

		// Token: 0x0400092E RID: 2350
		private readonly ModuleDefinition module;

		// Token: 0x0400092F RID: 2351
		private readonly ImageWriter writer;

		// Token: 0x04000930 RID: 2352
		private MetadataBuilder module_metadata;
	}
}
