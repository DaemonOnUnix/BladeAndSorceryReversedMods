using System;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000209 RID: 521
	public class OffsetTable
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x000361D8 File Offset: 0x000343D8
		internal OffsetTable()
		{
			int platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 128)
			{
				this.FileFlags |= OffsetTable.Flags.WindowsFileNames;
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00036228 File Offset: 0x00034428
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

		// Token: 0x06000FDB RID: 4059 RVA: 0x00036344 File Offset: 0x00034544
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

		// Token: 0x06000FDC RID: 4060 RVA: 0x00036444 File Offset: 0x00034644
		public override string ToString()
		{
			return string.Format("OffsetTable [{0} - {1}:{2} - {3}:{4}:{5} - {6}:{7}:{8} - {9}]", new object[] { this.TotalFileSize, this.DataSectionOffset, this.DataSectionSize, this.SourceCount, this.SourceTableOffset, this.SourceTableSize, this.MethodCount, this.MethodTableOffset, this.MethodTableSize, this.TypeCount });
		}

		// Token: 0x04000984 RID: 2436
		public const int MajorVersion = 50;

		// Token: 0x04000985 RID: 2437
		public const int MinorVersion = 0;

		// Token: 0x04000986 RID: 2438
		public const long Magic = 5037318119232611860L;

		// Token: 0x04000987 RID: 2439
		public int TotalFileSize;

		// Token: 0x04000988 RID: 2440
		public int DataSectionOffset;

		// Token: 0x04000989 RID: 2441
		public int DataSectionSize;

		// Token: 0x0400098A RID: 2442
		public int CompileUnitCount;

		// Token: 0x0400098B RID: 2443
		public int CompileUnitTableOffset;

		// Token: 0x0400098C RID: 2444
		public int CompileUnitTableSize;

		// Token: 0x0400098D RID: 2445
		public int SourceCount;

		// Token: 0x0400098E RID: 2446
		public int SourceTableOffset;

		// Token: 0x0400098F RID: 2447
		public int SourceTableSize;

		// Token: 0x04000990 RID: 2448
		public int MethodCount;

		// Token: 0x04000991 RID: 2449
		public int MethodTableOffset;

		// Token: 0x04000992 RID: 2450
		public int MethodTableSize;

		// Token: 0x04000993 RID: 2451
		public int TypeCount;

		// Token: 0x04000994 RID: 2452
		public int AnonymousScopeCount;

		// Token: 0x04000995 RID: 2453
		public int AnonymousScopeTableOffset;

		// Token: 0x04000996 RID: 2454
		public int AnonymousScopeTableSize;

		// Token: 0x04000997 RID: 2455
		public OffsetTable.Flags FileFlags;

		// Token: 0x04000998 RID: 2456
		public int LineNumberTable_LineBase = -1;

		// Token: 0x04000999 RID: 2457
		public int LineNumberTable_LineRange = 8;

		// Token: 0x0400099A RID: 2458
		public int LineNumberTable_OpcodeBase = 9;

		// Token: 0x0200020A RID: 522
		[Flags]
		public enum Flags
		{
			// Token: 0x0400099C RID: 2460
			IsAspxSource = 1,
			// Token: 0x0400099D RID: 2461
			WindowsFileNames = 2
		}
	}
}
