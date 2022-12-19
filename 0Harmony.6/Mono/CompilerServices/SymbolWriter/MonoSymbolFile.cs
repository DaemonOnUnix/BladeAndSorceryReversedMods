using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000208 RID: 520
	public class MonoSymbolFile : IDisposable
	{
		// Token: 0x06000FB2 RID: 4018 RVA: 0x000353DC File Offset: 0x000335DC
		public MonoSymbolFile()
		{
			this.ot = new OffsetTable();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00035418 File Offset: 0x00033618
		public int AddSource(SourceFileEntry source)
		{
			this.sources.Add(source);
			return this.sources.Count;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00035431 File Offset: 0x00033631
		public int AddCompileUnit(CompileUnitEntry entry)
		{
			this.comp_units.Add(entry);
			return this.comp_units.Count;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003544A File Offset: 0x0003364A
		public void AddMethod(MethodEntry entry)
		{
			this.methods.Add(entry);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00035458 File Offset: 0x00033658
		public MethodEntry DefineMethod(CompileUnitEntry comp_unit, int token, ScopeVariable[] scope_vars, LocalVariableEntry[] locals, LineNumberEntry[] lines, CodeBlockEntry[] code_blocks, string real_name, MethodEntry.Flags flags, int namespace_id)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry = new MethodEntry(this, comp_unit, token, scope_vars, locals, lines, code_blocks, real_name, flags, namespace_id);
			this.AddMethod(methodEntry);
			return methodEntry;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00035491 File Offset: 0x00033691
		internal void DefineAnonymousScope(int id)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			if (this.anonymous_scopes == null)
			{
				this.anonymous_scopes = new Dictionary<int, AnonymousScopeEntry>();
			}
			this.anonymous_scopes.Add(id, new AnonymousScopeEntry(id));
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x000354C6 File Offset: 0x000336C6
		internal void DefineCapturedVariable(int scope_id, string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedVariable(name, captured_name, kind);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x000354EB File Offset: 0x000336EB
		internal void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedScope(id, captured_name);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00035510 File Offset: 0x00033710
		internal int GetNextTypeIndex()
		{
			int num = this.last_type_index + 1;
			this.last_type_index = num;
			return num;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00035530 File Offset: 0x00033730
		internal int GetNextMethodIndex()
		{
			int num = this.last_method_index + 1;
			this.last_method_index = num;
			return num;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00035550 File Offset: 0x00033750
		internal int GetNextNamespaceIndex()
		{
			int num = this.last_namespace_index + 1;
			this.last_namespace_index = num;
			return num;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00035570 File Offset: 0x00033770
		private void Write(MyBinaryWriter bw, Guid guid)
		{
			bw.Write(5037318119232611860L);
			bw.Write(this.MajorVersion);
			bw.Write(this.MinorVersion);
			bw.Write(guid.ToByteArray());
			long position = bw.BaseStream.Position;
			this.ot.Write(bw, this.MajorVersion, this.MinorVersion);
			this.methods.Sort();
			for (int i = 0; i < this.methods.Count; i++)
			{
				this.methods[i].Index = i + 1;
			}
			this.ot.DataSectionOffset = (int)bw.BaseStream.Position;
			foreach (SourceFileEntry sourceFileEntry in this.sources)
			{
				sourceFileEntry.WriteData(bw);
			}
			foreach (CompileUnitEntry compileUnitEntry in this.comp_units)
			{
				compileUnitEntry.WriteData(bw);
			}
			foreach (MethodEntry methodEntry in this.methods)
			{
				methodEntry.WriteData(this, bw);
			}
			this.ot.DataSectionSize = (int)bw.BaseStream.Position - this.ot.DataSectionOffset;
			this.ot.MethodTableOffset = (int)bw.BaseStream.Position;
			for (int j = 0; j < this.methods.Count; j++)
			{
				this.methods[j].Write(bw);
			}
			this.ot.MethodTableSize = (int)bw.BaseStream.Position - this.ot.MethodTableOffset;
			this.ot.SourceTableOffset = (int)bw.BaseStream.Position;
			for (int k = 0; k < this.sources.Count; k++)
			{
				this.sources[k].Write(bw);
			}
			this.ot.SourceTableSize = (int)bw.BaseStream.Position - this.ot.SourceTableOffset;
			this.ot.CompileUnitTableOffset = (int)bw.BaseStream.Position;
			for (int l = 0; l < this.comp_units.Count; l++)
			{
				this.comp_units[l].Write(bw);
			}
			this.ot.CompileUnitTableSize = (int)bw.BaseStream.Position - this.ot.CompileUnitTableOffset;
			this.ot.AnonymousScopeCount = ((this.anonymous_scopes != null) ? this.anonymous_scopes.Count : 0);
			this.ot.AnonymousScopeTableOffset = (int)bw.BaseStream.Position;
			if (this.anonymous_scopes != null)
			{
				foreach (AnonymousScopeEntry anonymousScopeEntry in this.anonymous_scopes.Values)
				{
					anonymousScopeEntry.Write(bw);
				}
			}
			this.ot.AnonymousScopeTableSize = (int)bw.BaseStream.Position - this.ot.AnonymousScopeTableOffset;
			this.ot.TypeCount = this.last_type_index;
			this.ot.MethodCount = this.methods.Count;
			this.ot.SourceCount = this.sources.Count;
			this.ot.CompileUnitCount = this.comp_units.Count;
			this.ot.TotalFileSize = (int)bw.BaseStream.Position;
			bw.Seek((int)position, SeekOrigin.Begin);
			this.ot.Write(bw, this.MajorVersion, this.MinorVersion);
			bw.Seek(0, SeekOrigin.End);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00035980 File Offset: 0x00033B80
		public void CreateSymbolFile(Guid guid, FileStream fs)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.Write(new MyBinaryWriter(fs), guid);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000359A0 File Offset: 0x00033BA0
		private MonoSymbolFile(Stream stream)
		{
			this.reader = new MyBinaryReader(stream);
			try
			{
				long num = this.reader.ReadInt64();
				int num2 = this.reader.ReadInt32();
				int num3 = this.reader.ReadInt32();
				if (num != 5037318119232611860L)
				{
					throw new MonoSymbolFileException("Symbol file is not a valid", new object[0]);
				}
				if (num2 != 50)
				{
					throw new MonoSymbolFileException("Symbol file has version {0} but expected {1}", new object[] { num2, 50 });
				}
				if (num3 != 0)
				{
					throw new MonoSymbolFileException("Symbol file has version {0}.{1} but expected {2}.{3}", new object[] { num2, num3, 50, 0 });
				}
				this.MajorVersion = num2;
				this.MinorVersion = num3;
				this.guid = new Guid(this.reader.ReadBytes(16));
				this.ot = new OffsetTable(this.reader, num2, num3);
			}
			catch (Exception ex)
			{
				throw new MonoSymbolFileException("Cannot read symbol file", ex);
			}
			this.source_file_hash = new Dictionary<int, SourceFileEntry>();
			this.compile_unit_hash = new Dictionary<int, CompileUnitEntry>();
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00035AF8 File Offset: 0x00033CF8
		public static MonoSymbolFile ReadSymbolFile(Assembly assembly)
		{
			string text = assembly.Location + ".mdb";
			Guid moduleVersionId = assembly.GetModules()[0].ModuleVersionId;
			return MonoSymbolFile.ReadSymbolFile(text, moduleVersionId);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00035B29 File Offset: 0x00033D29
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename)
		{
			return MonoSymbolFile.ReadSymbolFile(new FileStream(mdbFilename, FileMode.Open, FileAccess.Read));
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00035B38 File Offset: 0x00033D38
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename, Guid assemblyGuid)
		{
			MonoSymbolFile monoSymbolFile = MonoSymbolFile.ReadSymbolFile(mdbFilename);
			if (assemblyGuid != monoSymbolFile.guid)
			{
				throw new MonoSymbolFileException("Symbol file `{0}' does not match assembly", new object[] { mdbFilename });
			}
			return monoSymbolFile;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00035B70 File Offset: 0x00033D70
		public static MonoSymbolFile ReadSymbolFile(Stream stream)
		{
			return new MonoSymbolFile(stream);
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00035B78 File Offset: 0x00033D78
		public int CompileUnitCount
		{
			get
			{
				return this.ot.CompileUnitCount;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x00035B85 File Offset: 0x00033D85
		public int SourceCount
		{
			get
			{
				return this.ot.SourceCount;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x00035B92 File Offset: 0x00033D92
		public int MethodCount
		{
			get
			{
				return this.ot.MethodCount;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00035B9F File Offset: 0x00033D9F
		public int TypeCount
		{
			get
			{
				return this.ot.TypeCount;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00035BAC File Offset: 0x00033DAC
		public int AnonymousScopeCount
		{
			get
			{
				return this.ot.AnonymousScopeCount;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00035BB9 File Offset: 0x00033DB9
		public int NamespaceCount
		{
			get
			{
				return this.last_namespace_index;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00035BC1 File Offset: 0x00033DC1
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00035BC9 File Offset: 0x00033DC9
		public OffsetTable OffsetTable
		{
			get
			{
				return this.ot;
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00035BD4 File Offset: 0x00033DD4
		public SourceFileEntry GetSourceFile(int index)
		{
			if (index < 1 || index > this.ot.SourceCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			SourceFileEntry sourceFileEntry2;
			lock (this)
			{
				SourceFileEntry sourceFileEntry;
				if (this.source_file_hash.TryGetValue(index, out sourceFileEntry))
				{
					sourceFileEntry2 = sourceFileEntry;
				}
				else
				{
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)(this.ot.SourceTableOffset + SourceFileEntry.Size * (index - 1));
					sourceFileEntry = new SourceFileEntry(this, this.reader);
					this.source_file_hash.Add(index, sourceFileEntry);
					this.reader.BaseStream.Position = position;
					sourceFileEntry2 = sourceFileEntry;
				}
			}
			return sourceFileEntry2;
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00035CAC File Offset: 0x00033EAC
		public SourceFileEntry[] Sources
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				SourceFileEntry[] array = new SourceFileEntry[this.SourceCount];
				for (int i = 0; i < this.SourceCount; i++)
				{
					array[i] = this.GetSourceFile(i + 1);
				}
				return array;
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00035CF4 File Offset: 0x00033EF4
		public CompileUnitEntry GetCompileUnit(int index)
		{
			if (index < 1 || index > this.ot.CompileUnitCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			CompileUnitEntry compileUnitEntry2;
			lock (this)
			{
				CompileUnitEntry compileUnitEntry;
				if (this.compile_unit_hash.TryGetValue(index, out compileUnitEntry))
				{
					compileUnitEntry2 = compileUnitEntry;
				}
				else
				{
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)(this.ot.CompileUnitTableOffset + CompileUnitEntry.Size * (index - 1));
					compileUnitEntry = new CompileUnitEntry(this, this.reader);
					this.compile_unit_hash.Add(index, compileUnitEntry);
					this.reader.BaseStream.Position = position;
					compileUnitEntry2 = compileUnitEntry;
				}
			}
			return compileUnitEntry2;
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00035DCC File Offset: 0x00033FCC
		public CompileUnitEntry[] CompileUnits
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				CompileUnitEntry[] array = new CompileUnitEntry[this.CompileUnitCount];
				for (int i = 0; i < this.CompileUnitCount; i++)
				{
					array[i] = this.GetCompileUnit(i + 1);
				}
				return array;
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00035E14 File Offset: 0x00034014
		private void read_methods()
		{
			lock (this)
			{
				if (this.method_token_hash == null)
				{
					this.method_token_hash = new Dictionary<int, MethodEntry>();
					this.method_list = new List<MethodEntry>();
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)this.ot.MethodTableOffset;
					for (int i = 0; i < this.MethodCount; i++)
					{
						MethodEntry methodEntry = new MethodEntry(this, this.reader, i + 1);
						this.method_token_hash.Add(methodEntry.Token, methodEntry);
						this.method_list.Add(methodEntry);
					}
					this.reader.BaseStream.Position = position;
				}
			}
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00035EF0 File Offset: 0x000340F0
		public MethodEntry GetMethodByToken(int token)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry2;
			lock (this)
			{
				this.read_methods();
				MethodEntry methodEntry;
				this.method_token_hash.TryGetValue(token, out methodEntry);
				methodEntry2 = methodEntry;
			}
			return methodEntry2;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00035F4C File Offset: 0x0003414C
		public MethodEntry GetMethod(int index)
		{
			if (index < 1 || index > this.ot.MethodCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry;
			lock (this)
			{
				this.read_methods();
				methodEntry = this.method_list[index - 1];
			}
			return methodEntry;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00035FC0 File Offset: 0x000341C0
		public MethodEntry[] Methods
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				MethodEntry[] array2;
				lock (this)
				{
					this.read_methods();
					MethodEntry[] array = new MethodEntry[this.MethodCount];
					this.method_list.CopyTo(array, 0);
					array2 = array;
				}
				return array2;
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00036028 File Offset: 0x00034228
		public int FindSource(string file_name)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			int num2;
			lock (this)
			{
				if (this.source_name_hash == null)
				{
					this.source_name_hash = new Dictionary<string, int>();
					for (int i = 0; i < this.ot.SourceCount; i++)
					{
						SourceFileEntry sourceFile = this.GetSourceFile(i + 1);
						this.source_name_hash.Add(sourceFile.FileName, i);
					}
				}
				int num;
				if (!this.source_name_hash.TryGetValue(file_name, out num))
				{
					num2 = -1;
				}
				else
				{
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000360CC File Offset: 0x000342CC
		public AnonymousScopeEntry GetAnonymousScope(int id)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			AnonymousScopeEntry anonymousScopeEntry2;
			lock (this)
			{
				if (this.anonymous_scopes != null)
				{
					AnonymousScopeEntry anonymousScopeEntry;
					this.anonymous_scopes.TryGetValue(id, out anonymousScopeEntry);
					anonymousScopeEntry2 = anonymousScopeEntry;
				}
				else
				{
					this.anonymous_scopes = new Dictionary<int, AnonymousScopeEntry>();
					this.reader.BaseStream.Position = (long)this.ot.AnonymousScopeTableOffset;
					for (int i = 0; i < this.ot.AnonymousScopeCount; i++)
					{
						AnonymousScopeEntry anonymousScopeEntry = new AnonymousScopeEntry(this.reader);
						this.anonymous_scopes.Add(anonymousScopeEntry.ID, anonymousScopeEntry);
					}
					anonymousScopeEntry2 = this.anonymous_scopes[id];
				}
			}
			return anonymousScopeEntry2;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00036198 File Offset: 0x00034398
		internal MyBinaryReader BinaryReader
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				return this.reader;
			}
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000361AE File Offset: 0x000343AE
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000361B7 File Offset: 0x000343B7
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
		}

		// Token: 0x0400096D RID: 2413
		private List<MethodEntry> methods = new List<MethodEntry>();

		// Token: 0x0400096E RID: 2414
		private List<SourceFileEntry> sources = new List<SourceFileEntry>();

		// Token: 0x0400096F RID: 2415
		private List<CompileUnitEntry> comp_units = new List<CompileUnitEntry>();

		// Token: 0x04000970 RID: 2416
		private Dictionary<int, AnonymousScopeEntry> anonymous_scopes;

		// Token: 0x04000971 RID: 2417
		private OffsetTable ot;

		// Token: 0x04000972 RID: 2418
		private int last_type_index;

		// Token: 0x04000973 RID: 2419
		private int last_method_index;

		// Token: 0x04000974 RID: 2420
		private int last_namespace_index;

		// Token: 0x04000975 RID: 2421
		public readonly int MajorVersion = 50;

		// Token: 0x04000976 RID: 2422
		public readonly int MinorVersion;

		// Token: 0x04000977 RID: 2423
		public int NumLineNumbers;

		// Token: 0x04000978 RID: 2424
		private MyBinaryReader reader;

		// Token: 0x04000979 RID: 2425
		private Dictionary<int, SourceFileEntry> source_file_hash;

		// Token: 0x0400097A RID: 2426
		private Dictionary<int, CompileUnitEntry> compile_unit_hash;

		// Token: 0x0400097B RID: 2427
		private List<MethodEntry> method_list;

		// Token: 0x0400097C RID: 2428
		private Dictionary<int, MethodEntry> method_token_hash;

		// Token: 0x0400097D RID: 2429
		private Dictionary<string, int> source_name_hash;

		// Token: 0x0400097E RID: 2430
		private Guid guid;

		// Token: 0x0400097F RID: 2431
		internal int LineNumberCount;

		// Token: 0x04000980 RID: 2432
		internal int LocalCount;

		// Token: 0x04000981 RID: 2433
		internal int StringSize;

		// Token: 0x04000982 RID: 2434
		internal int LineNumberSize;

		// Token: 0x04000983 RID: 2435
		internal int ExtendedLineNumberSize;
	}
}
