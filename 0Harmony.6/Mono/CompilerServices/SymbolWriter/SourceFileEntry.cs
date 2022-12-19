﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000216 RID: 534
	public class SourceFileEntry
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x00036B3F File Offset: 0x00034D3F
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00036EE9 File Offset: 0x000350E9
		public SourceFileEntry(MonoSymbolFile file, string file_name)
		{
			this.file = file;
			this.file_name = file_name;
			this.Index = file.AddSource(this);
			this.creating = true;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00036F13 File Offset: 0x00035113
		public SourceFileEntry(MonoSymbolFile file, string sourceFile, byte[] guid, byte[] checksum)
			: this(file, sourceFile, sourceFile, guid, checksum)
		{
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00036F21 File Offset: 0x00035121
		public SourceFileEntry(MonoSymbolFile file, string fileName, string sourceFile, byte[] guid, byte[] checksum)
			: this(file, fileName)
		{
			this.guid = guid;
			this.hash = checksum;
			this.sourceFile = sourceFile;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x00036F42 File Offset: 0x00035142
		public byte[] Checksum
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00036F4C File Offset: 0x0003514C
		internal void WriteData(MyBinaryWriter bw)
		{
			this.DataOffset = (int)bw.BaseStream.Position;
			bw.Write(this.file_name);
			if (this.guid == null)
			{
				this.guid = new byte[16];
			}
			if (this.hash == null)
			{
				try
				{
					using (FileStream fileStream = new FileStream(this.sourceFile, FileMode.Open, FileAccess.Read))
					{
						MD5 md = MD5.Create();
						this.hash = md.ComputeHash(fileStream);
					}
				}
				catch
				{
					this.hash = new byte[16];
				}
			}
			bw.Write(this.guid);
			bw.Write(this.hash);
			bw.Write(this.auto_generated ? 1 : 0);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003701C File Offset: 0x0003521C
		internal void Write(BinaryWriter bw)
		{
			bw.Write(this.Index);
			bw.Write(this.DataOffset);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00037038 File Offset: 0x00035238
		internal SourceFileEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.file = file;
			this.Index = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
			int num = (int)reader.BaseStream.Position;
			reader.BaseStream.Position = (long)this.DataOffset;
			this.sourceFile = (this.file_name = reader.ReadString());
			this.guid = reader.ReadBytes(16);
			this.hash = reader.ReadBytes(16);
			this.auto_generated = reader.ReadByte() == 1;
			reader.BaseStream.Position = (long)num;
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x000370D6 File Offset: 0x000352D6
		// (set) Token: 0x06001019 RID: 4121 RVA: 0x000370DE File Offset: 0x000352DE
		public string FileName
		{
			get
			{
				return this.file_name;
			}
			set
			{
				this.file_name = value;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x000370E7 File Offset: 0x000352E7
		public bool AutoGenerated
		{
			get
			{
				return this.auto_generated;
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x000370EF File Offset: 0x000352EF
		public void SetAutoGenerated()
		{
			if (!this.creating)
			{
				throw new InvalidOperationException();
			}
			this.auto_generated = true;
			this.file.OffsetTable.FileFlags |= OffsetTable.Flags.IsAspxSource;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00037120 File Offset: 0x00035320
		public bool CheckChecksum()
		{
			bool flag;
			try
			{
				using (FileStream fileStream = new FileStream(this.sourceFile, FileMode.Open))
				{
					byte[] array = MD5.Create().ComputeHash(fileStream);
					for (int i = 0; i < 16; i++)
					{
						if (array[i] != this.hash[i])
						{
							return false;
						}
					}
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00037194 File Offset: 0x00035394
		public override string ToString()
		{
			return string.Format("SourceFileEntry ({0}:{1})", this.Index, this.DataOffset);
		}

		// Token: 0x040009C9 RID: 2505
		public readonly int Index;

		// Token: 0x040009CA RID: 2506
		private int DataOffset;

		// Token: 0x040009CB RID: 2507
		private MonoSymbolFile file;

		// Token: 0x040009CC RID: 2508
		private string file_name;

		// Token: 0x040009CD RID: 2509
		private byte[] guid;

		// Token: 0x040009CE RID: 2510
		private byte[] hash;

		// Token: 0x040009CF RID: 2511
		private bool creating;

		// Token: 0x040009D0 RID: 2512
		private bool auto_generated;

		// Token: 0x040009D1 RID: 2513
		private readonly string sourceFile;
	}
}
