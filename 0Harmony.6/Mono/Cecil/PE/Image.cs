using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x02000196 RID: 406
	internal sealed class Image : IDisposable
	{
		// Token: 0x06000D0C RID: 3340 RVA: 0x0002C171 File Offset: 0x0002A371
		public Image()
		{
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0002C198 File Offset: 0x0002A398
		public bool HasTable(Table table)
		{
			return this.GetTableLength(table) > 0;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0002C1A4 File Offset: 0x0002A3A4
		public int GetTableLength(Table table)
		{
			return (int)this.TableHeap[table].Length;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0002C1B7 File Offset: 0x0002A3B7
		public int GetTableIndexSize(Table table)
		{
			if (this.GetTableLength(table) >= 65536)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0002C1CC File Offset: 0x0002A3CC
		public int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0002C200 File Offset: 0x0002A400
		public uint ResolveVirtualAddress(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002C226 File Offset: 0x0002A426
		public uint ResolveVirtualAddressInSection(uint rva, Section section)
		{
			return rva + section.PointerToRawData - section.VirtualAddress;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002C238 File Offset: 0x0002A438
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

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002C270 File Offset: 0x0002A470
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

		// Token: 0x06000D15 RID: 3349 RVA: 0x0002C2B4 File Offset: 0x0002A4B4
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

		// Token: 0x06000D16 RID: 3350 RVA: 0x0002C2EC File Offset: 0x0002A4EC
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

		// Token: 0x06000D17 RID: 3351 RVA: 0x0002C350 File Offset: 0x0002A550
		public bool HasDebugTables()
		{
			return this.HasTable(Table.Document) || this.HasTable(Table.MethodDebugInformation) || this.HasTable(Table.LocalScope) || this.HasTable(Table.LocalVariable) || this.HasTable(Table.LocalConstant) || this.HasTable(Table.StateMachineMethod) || this.HasTable(Table.CustomDebugInformation);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0002C3A3 File Offset: 0x0002A5A3
		public void Dispose()
		{
			this.Stream.Dispose();
		}

		// Token: 0x040005B1 RID: 1457
		public Disposable<Stream> Stream;

		// Token: 0x040005B2 RID: 1458
		public string FileName;

		// Token: 0x040005B3 RID: 1459
		public ModuleKind Kind;

		// Token: 0x040005B4 RID: 1460
		public string RuntimeVersion;

		// Token: 0x040005B5 RID: 1461
		public TargetArchitecture Architecture;

		// Token: 0x040005B6 RID: 1462
		public ModuleCharacteristics Characteristics;

		// Token: 0x040005B7 RID: 1463
		public ushort LinkerVersion;

		// Token: 0x040005B8 RID: 1464
		public ushort SubSystemMajor;

		// Token: 0x040005B9 RID: 1465
		public ushort SubSystemMinor;

		// Token: 0x040005BA RID: 1466
		public ImageDebugHeader DebugHeader;

		// Token: 0x040005BB RID: 1467
		public Section[] Sections;

		// Token: 0x040005BC RID: 1468
		public Section MetadataSection;

		// Token: 0x040005BD RID: 1469
		public uint EntryPointToken;

		// Token: 0x040005BE RID: 1470
		public uint Timestamp;

		// Token: 0x040005BF RID: 1471
		public ModuleAttributes Attributes;

		// Token: 0x040005C0 RID: 1472
		public DataDirectory Win32Resources;

		// Token: 0x040005C1 RID: 1473
		public DataDirectory Debug;

		// Token: 0x040005C2 RID: 1474
		public DataDirectory Resources;

		// Token: 0x040005C3 RID: 1475
		public DataDirectory StrongName;

		// Token: 0x040005C4 RID: 1476
		public StringHeap StringHeap;

		// Token: 0x040005C5 RID: 1477
		public BlobHeap BlobHeap;

		// Token: 0x040005C6 RID: 1478
		public UserStringHeap UserStringHeap;

		// Token: 0x040005C7 RID: 1479
		public GuidHeap GuidHeap;

		// Token: 0x040005C8 RID: 1480
		public TableHeap TableHeap;

		// Token: 0x040005C9 RID: 1481
		public PdbHeap PdbHeap;

		// Token: 0x040005CA RID: 1482
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x040005CB RID: 1483
		private readonly Func<Table, int> counter;
	}
}
