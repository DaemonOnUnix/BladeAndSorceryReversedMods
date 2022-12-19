using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x0200028B RID: 651
	internal sealed class Image : IDisposable
	{
		// Token: 0x0600106F RID: 4207 RVA: 0x00033B51 File Offset: 0x00031D51
		public Image()
		{
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00033B78 File Offset: 0x00031D78
		public bool HasTable(Table table)
		{
			return this.GetTableLength(table) > 0;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00033B84 File Offset: 0x00031D84
		public int GetTableLength(Table table)
		{
			return (int)this.TableHeap[table].Length;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00033B97 File Offset: 0x00031D97
		public int GetTableIndexSize(Table table)
		{
			if (this.GetTableLength(table) >= 65536)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00033BAC File Offset: 0x00031DAC
		public int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00033BE0 File Offset: 0x00031DE0
		public uint ResolveVirtualAddress(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00033C06 File Offset: 0x00031E06
		public uint ResolveVirtualAddressInSection(uint rva, Section section)
		{
			return rva + section.PointerToRawData - section.VirtualAddress;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00033C18 File Offset: 0x00031E18
		public Section GetSection(string name)
		{
			foreach (Section section in this.Sections)
			{
				if (section.Name == name)
				{
					return section;
				}
			}
			return null;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00033C50 File Offset: 0x00031E50
		public Section GetSectionAtVirtualAddress(uint rva)
		{
			foreach (Section section in this.Sections)
			{
				if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.SizeOfRawData)
				{
					return section;
				}
			}
			return null;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00033C94 File Offset: 0x00031E94
		private BinaryStreamReader GetReaderAt(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				return null;
			}
			BinaryStreamReader binaryStreamReader = new BinaryStreamReader(this.Stream.value);
			binaryStreamReader.MoveTo(this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress));
			return binaryStreamReader;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00033CCC File Offset: 0x00031ECC
		public TRet GetReaderAt<TItem, TRet>(uint rva, TItem item, Func<TItem, BinaryStreamReader, TRet> read) where TRet : class
		{
			long position = this.Stream.value.Position;
			TRet tret;
			try
			{
				BinaryStreamReader readerAt = this.GetReaderAt(rva);
				if (readerAt == null)
				{
					tret = default(TRet);
					tret = tret;
				}
				else
				{
					tret = read(item, readerAt);
				}
			}
			finally
			{
				this.Stream.value.Position = position;
			}
			return tret;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00033D30 File Offset: 0x00031F30
		public bool HasDebugTables()
		{
			return this.HasTable(Table.Document) || this.HasTable(Table.MethodDebugInformation) || this.HasTable(Table.LocalScope) || this.HasTable(Table.LocalVariable) || this.HasTable(Table.LocalConstant) || this.HasTable(Table.StateMachineMethod) || this.HasTable(Table.CustomDebugInformation);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00033D83 File Offset: 0x00031F83
		public void Dispose()
		{
			this.Stream.Dispose();
		}

		// Token: 0x040005E8 RID: 1512
		public Disposable<Stream> Stream;

		// Token: 0x040005E9 RID: 1513
		public string FileName;

		// Token: 0x040005EA RID: 1514
		public ModuleKind Kind;

		// Token: 0x040005EB RID: 1515
		public uint Characteristics;

		// Token: 0x040005EC RID: 1516
		public string RuntimeVersion;

		// Token: 0x040005ED RID: 1517
		public TargetArchitecture Architecture;

		// Token: 0x040005EE RID: 1518
		public ModuleCharacteristics DllCharacteristics;

		// Token: 0x040005EF RID: 1519
		public ushort LinkerVersion;

		// Token: 0x040005F0 RID: 1520
		public ushort SubSystemMajor;

		// Token: 0x040005F1 RID: 1521
		public ushort SubSystemMinor;

		// Token: 0x040005F2 RID: 1522
		public ImageDebugHeader DebugHeader;

		// Token: 0x040005F3 RID: 1523
		public Section[] Sections;

		// Token: 0x040005F4 RID: 1524
		public Section MetadataSection;

		// Token: 0x040005F5 RID: 1525
		public uint EntryPointToken;

		// Token: 0x040005F6 RID: 1526
		public uint Timestamp;

		// Token: 0x040005F7 RID: 1527
		public ModuleAttributes Attributes;

		// Token: 0x040005F8 RID: 1528
		public DataDirectory Win32Resources;

		// Token: 0x040005F9 RID: 1529
		public DataDirectory Debug;

		// Token: 0x040005FA RID: 1530
		public DataDirectory Resources;

		// Token: 0x040005FB RID: 1531
		public DataDirectory StrongName;

		// Token: 0x040005FC RID: 1532
		public StringHeap StringHeap;

		// Token: 0x040005FD RID: 1533
		public BlobHeap BlobHeap;

		// Token: 0x040005FE RID: 1534
		public UserStringHeap UserStringHeap;

		// Token: 0x040005FF RID: 1535
		public GuidHeap GuidHeap;

		// Token: 0x04000600 RID: 1536
		public TableHeap TableHeap;

		// Token: 0x04000601 RID: 1537
		public PdbHeap PdbHeap;

		// Token: 0x04000602 RID: 1538
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x04000603 RID: 1539
		private readonly Func<Table, int> counter;
	}
}
