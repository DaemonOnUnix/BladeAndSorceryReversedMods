using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000215 RID: 533
	public class CompileUnitEntry : ICompileUnit
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x00036B3F File Offset: 0x00034D3F
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00011FA0 File Offset: 0x000101A0
		CompileUnitEntry ICompileUnit.Entry
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00036B42 File Offset: 0x00034D42
		public CompileUnitEntry(MonoSymbolFile file, SourceFileEntry source)
		{
			this.file = file;
			this.source = source;
			this.Index = file.AddCompileUnit(this);
			this.creating = true;
			this.namespaces = new List<NamespaceEntry>();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00036B77 File Offset: 0x00034D77
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

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x00036BA6 File Offset: 0x00034DA6
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

		// Token: 0x06001008 RID: 4104 RVA: 0x00036BC4 File Offset: 0x00034DC4
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

		// Token: 0x06001009 RID: 4105 RVA: 0x00036C04 File Offset: 0x00034E04
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

		// Token: 0x0600100A RID: 4106 RVA: 0x00036D00 File Offset: 0x00034F00
		internal void Write(BinaryWriter bw)
		{
			bw.Write(this.Index);
			bw.Write(this.DataOffset);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00036D1A File Offset: 0x00034F1A
		internal CompileUnitEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.file = file;
			this.Index = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00036D41 File Offset: 0x00034F41
		public void ReadAll()
		{
			this.ReadData();
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00036D4C File Offset: 0x00034F4C
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

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x00036E74 File Offset: 0x00035074
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

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x00036EA8 File Offset: 0x000350A8
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

		// Token: 0x040009C2 RID: 2498
		public readonly int Index;

		// Token: 0x040009C3 RID: 2499
		private int DataOffset;

		// Token: 0x040009C4 RID: 2500
		private MonoSymbolFile file;

		// Token: 0x040009C5 RID: 2501
		private SourceFileEntry source;

		// Token: 0x040009C6 RID: 2502
		private List<SourceFileEntry> include_files;

		// Token: 0x040009C7 RID: 2503
		private List<NamespaceEntry> namespaces;

		// Token: 0x040009C8 RID: 2504
		private bool creating;
	}
}
