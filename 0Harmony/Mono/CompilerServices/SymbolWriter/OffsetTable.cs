using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x020002FF RID: 767
	public class OffsetTable
	{
		// Token: 0x06001349 RID: 4937 RVA: 0x0003E124 File Offset: 0x0003C324
		internal OffsetTable()
		{
			int platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 128)
			{
				this.FileFlags |= OffsetTable.Flags.WindowsFileNames;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0003E174 File Offset: 0x0003C374
		internal OffsetTable(BinaryReader reader, int major_version, int minor_version)
		{
			this.TotalFileSize = reader.ReadInt32();
			this.DataSectionOffset = reader.ReadInt32();
			this.DataSectionSize = reader.ReadInt32();
			this.CompileUnitCount = reader.ReadInt32();
			this.CompileUnitTableOffset = reader.ReadInt32();
			this.CompileUnitTableSize = reader.ReadInt32();
			this.SourceCount = reader.ReadInt32();
			this.SourceTableOffset = reader.ReadInt32();
			this.SourceTableSize = reader.ReadInt32();
			this.MethodCount = reader.ReadInt32();
			this.MethodTableOffset = reader.ReadInt32();
			this.MethodTableSize = reader.ReadInt32();
			this.TypeCount = reader.ReadInt32();
			this.AnonymousScopeCount = reader.ReadInt32();
			this.AnonymousScopeTableOffset = reader.ReadInt32();
			this.AnonymousScopeTableSize = reader.ReadInt32();
			this.LineNumberTable_LineBase = reader.ReadInt32();
			this.LineNumberTable_LineRange = reader.ReadInt32();
			this.LineNumberTable_OpcodeBase = reader.ReadInt32();
			this.FileFlags = (OffsetTable.Flags)reader.ReadInt32();
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0003E290 File Offset: 0x0003C490
		internal void Write(BinaryWriter bw, int major_version, int minor_version)
		{
			bw.Write(this.TotalFileSize);
			bw.Write(this.DataSectionOffset);
			bw.Write(this.DataSectionSize);
			bw.Write(this.CompileUnitCount);
			bw.Write(this.CompileUnitTableOffset);
			bw.Write(this.CompileUnitTableSize);
			bw.Write(this.SourceCount);
			bw.Write(this.SourceTableOffset);
			bw.Write(this.SourceTableSize);
			bw.Write(this.MethodCount);
			bw.Write(this.MethodTableOffset);
			bw.Write(this.MethodTableSize);
			bw.Write(this.TypeCount);
			bw.Write(this.AnonymousScopeCount);
			bw.Write(this.AnonymousScopeTableOffset);
			bw.Write(this.AnonymousScopeTableSize);
			bw.Write(this.LineNumberTable_LineBase);
			bw.Write(this.LineNumberTable_LineRange);
			bw.Write(this.LineNumberTable_OpcodeBase);
			bw.Write((int)this.FileFlags);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0003E390 File Offset: 0x0003C590
		public override string ToString()
		{
			return string.Format("OffsetTable [{0} - {1}:{2} - {3}:{4}:{5} - {6}:{7}:{8} - {9}]", new object[] { this.TotalFileSize, this.DataSectionOffset, this.DataSectionSize, this.SourceCount, this.SourceTableOffset, this.SourceTableSize, this.MethodCount, this.MethodTableOffset, this.MethodTableSize, this.TypeCount });
		}

		// Token: 0x040009C3 RID: 2499
		public const int MajorVersion = 50;

		// Token: 0x040009C4 RID: 2500
		public const int MinorVersion = 0;

		// Token: 0x040009C5 RID: 2501
		public const long Magic = 5037318119232611860L;

		// Token: 0x040009C6 RID: 2502
		public int TotalFileSize;

		// Token: 0x040009C7 RID: 2503
		public int DataSectionOffset;

		// Token: 0x040009C8 RID: 2504
		public int DataSectionSize;

		// Token: 0x040009C9 RID: 2505
		public int CompileUnitCount;

		// Token: 0x040009CA RID: 2506
		public int CompileUnitTableOffset;

		// Token: 0x040009CB RID: 2507
		public int CompileUnitTableSize;

		// Token: 0x040009CC RID: 2508
		public int SourceCount;

		// Token: 0x040009CD RID: 2509
		public int SourceTableOffset;

		// Token: 0x040009CE RID: 2510
		public int SourceTableSize;

		// Token: 0x040009CF RID: 2511
		public int MethodCount;

		// Token: 0x040009D0 RID: 2512
		public int MethodTableOffset;

		// Token: 0x040009D1 RID: 2513
		public int MethodTableSize;

		// Token: 0x040009D2 RID: 2514
		public int TypeCount;

		// Token: 0x040009D3 RID: 2515
		public int AnonymousScopeCount;

		// Token: 0x040009D4 RID: 2516
		public int AnonymousScopeTableOffset;

		// Token: 0x040009D5 RID: 2517
		public int AnonymousScopeTableSize;

		// Token: 0x040009D6 RID: 2518
		public OffsetTable.Flags FileFlags;

		// Token: 0x040009D7 RID: 2519
		public int LineNumberTable_LineBase = -1;

		// Token: 0x040009D8 RID: 2520
		public int LineNumberTable_LineRange = 8;

		// Token: 0x040009D9 RID: 2521
		public int LineNumberTable_OpcodeBase = 9;

		// Token: 0x02000300 RID: 768
		[Flags]
		public enum Flags
		{
			// Token: 0x040009DB RID: 2523
			IsAspxSource = 1,
			// Token: 0x040009DC RID: 2524
			WindowsFileNames = 2
		}
	}
}
