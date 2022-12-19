using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200030B RID: 779
	public class CompileUnitEntry : ICompileUnit
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0003EA8B File Offset: 0x0003CC8B
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00017E2C File Offset: 0x0001602C
		CompileUnitEntry ICompileUnit.Entry
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0003EA8E File Offset: 0x0003CC8E
		public CompileUnitEntry(MonoSymbolFile file, SourceFileEntry source)
		{
			this.file = file;
			this.source = source;
			this.Index = file.AddCompileUnit(this);
			this.creating = true;
			this.namespaces = new List<NamespaceEntry>();
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0003EAC3 File Offset: 0x0003CCC3
		public void AddFile(SourceFileEntry file)
		{
			if (!this.creating)
			{
				throw new InvalidOperationException();
			}
			if (this.include_files == null)
			{
				this.include_files = new List<SourceFileEntry>();
			}
			this.include_files.Add(file);
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x0003EAF2 File Offset: 0x0003CCF2
		public SourceFileEntry SourceFile
		{
			get
			{
				if (this.creating)
				{
					return this.source;
				}
				this.ReadData();
				return this.source;
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0003EB10 File Offset: 0x0003CD10
		public int DefineNamespace(string name, string[] using_clauses, int parent)
		{
			if (!this.creating)
			{
				throw new InvalidOperationException();
			}
			int nextNamespaceIndex = this.file.GetNextNamespaceIndex();
			NamespaceEntry namespaceEntry = new NamespaceEntry(name, nextNamespaceIndex, using_clauses, parent);
			this.namespaces.Add(namespaceEntry);
			return nextNamespaceIndex;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0003EB50 File Offset: 0x0003CD50
		internal void WriteData(MyBinaryWriter bw)
		{
			this.DataOffset = (int)bw.BaseStream.Position;
			bw.WriteLeb128(this.source.Index);
			int num = ((this.include_files != null) ? this.include_files.Count : 0);
			bw.WriteLeb128(num);
			if (this.include_files != null)
			{
				foreach (SourceFileEntry sourceFileEntry in this.include_files)
				{
					bw.WriteLeb128(sourceFileEntry.Index);
				}
			}
			bw.WriteLeb128(this.namespaces.Count);
			foreach (NamespaceEntry namespaceEntry in this.namespaces)
			{
				namespaceEntry.Write(this.file, bw);
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0003EC4C File Offset: 0x0003CE4C
		internal void Write(BinaryWriter bw)
		{
			bw.Write(this.Index);
			bw.Write(this.DataOffset);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0003EC66 File Offset: 0x0003CE66
		internal CompileUnitEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.file = file;
			this.Index = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0003EC8D File Offset: 0x0003CE8D
		public void ReadAll()
		{
			this.ReadData();
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0003EC98 File Offset: 0x0003CE98
		private void ReadData()
		{
			if (this.creating)
			{
				throw new InvalidOperationException();
			}
			MonoSymbolFile monoSymbolFile = this.file;
			lock (monoSymbolFile)
			{
				if (this.namespaces == null)
				{
					MyBinaryReader binaryReader = this.file.BinaryReader;
					int num = (int)binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.DataOffset;
					int num2 = binaryReader.ReadLeb128();
					this.source = this.file.GetSourceFile(num2);
					int num3 = binaryReader.ReadLeb128();
					if (num3 > 0)
					{
						this.include_files = new List<SourceFileEntry>();
						for (int i = 0; i < num3; i++)
						{
							this.include_files.Add(this.file.GetSourceFile(binaryReader.ReadLeb128()));
						}
					}
					int num4 = binaryReader.ReadLeb128();
					this.namespaces = new List<NamespaceEntry>();
					for (int j = 0; j < num4; j++)
					{
						this.namespaces.Add(new NamespaceEntry(this.file, binaryReader));
					}
					binaryReader.BaseStream.Position = (long)num;
				}
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x0003EDC0 File Offset: 0x0003CFC0
		public NamespaceEntry[] Namespaces
		{
			get
			{
				this.ReadData();
				NamespaceEntry[] array = new NamespaceEntry[this.namespaces.Count];
				this.namespaces.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0003EDF4 File Offset: 0x0003CFF4
		public SourceFileEntry[] IncludeFiles
		{
			get
			{
				this.ReadData();
				if (this.include_files == null)
				{
					return new SourceFileEntry[0];
				}
				SourceFileEntry[] array = new SourceFileEntry[this.include_files.Count];
				this.include_files.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000A01 RID: 2561
		public readonly int Index;

		// Token: 0x04000A02 RID: 2562
		private int DataOffset;

		// Token: 0x04000A03 RID: 2563
		private MonoSymbolFile file;

		// Token: 0x04000A04 RID: 2564
		private SourceFileEntry source;

		// Token: 0x04000A05 RID: 2565
		private List<SourceFileEntry> include_files;

		// Token: 0x04000A06 RID: 2566
		private List<NamespaceEntry> namespaces;

		// Token: 0x04000A07 RID: 2567
		private bool creating;
	}
}
