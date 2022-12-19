using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x020002FE RID: 766
	public class MonoSymbolFile : IDisposable
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x0003D328 File Offset: 0x0003B528
		public MonoSymbolFile()
		{
			this.ot = new OffsetTable();
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0003D364 File Offset: 0x0003B564
		public int AddSource(SourceFileEntry source)
		{
			this.sources.Add(source);
			return this.sources.Count;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0003D37D File Offset: 0x0003B57D
		public int AddCompileUnit(CompileUnitEntry entry)
		{
			this.comp_units.Add(entry);
			return this.comp_units.Count;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0003D396 File Offset: 0x0003B596
		public void AddMethod(MethodEntry entry)
		{
			this.methods.Add(entry);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0003D3A4 File Offset: 0x0003B5A4
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

		// Token: 0x06001327 RID: 4903 RVA: 0x0003D3DD File Offset: 0x0003B5DD
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

		// Token: 0x06001328 RID: 4904 RVA: 0x0003D412 File Offset: 0x0003B612
		internal void DefineCapturedVariable(int scope_id, string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedVariable(name, captured_name, kind);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0003D437 File Offset: 0x0003B637
		internal void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedScope(id, captured_name);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0003D45C File Offset: 0x0003B65C
		internal int GetNextTypeIndex()
		{
			int num = this.last_type_index + 1;
			this.last_type_index = num;
			return num;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0003D47C File Offset: 0x0003B67C
		internal int GetNextMethodIndex()
		{
			int num = this.last_method_index + 1;
			this.last_method_index = num;
			return num;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0003D49C File Offset: 0x0003B69C
		internal int GetNextNamespaceIndex()
		{
			int num = this.last_namespace_index + 1;
			this.last_namespace_index = num;
			return num;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0003D4BC File Offset: 0x0003B6BC
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

		// Token: 0x0600132E RID: 4910 RVA: 0x0003D8CC File Offset: 0x0003BACC
		public void CreateSymbolFile(Guid guid, FileStream fs)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.Write(new MyBinaryWriter(fs), guid);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0003D8EC File Offset: 0x0003BAEC
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

		// Token: 0x06001330 RID: 4912 RVA: 0x0003DA44 File Offset: 0x0003BC44
		public static MonoSymbolFile ReadSymbolFile(Assembly assembly)
		{
			string text = assembly.Location + ".mdb";
			Guid moduleVersionId = assembly.GetModules()[0].ModuleVersionId;
			return MonoSymbolFile.ReadSymbolFile(text, moduleVersionId);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0003DA75 File Offset: 0x0003BC75
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename)
		{
			return MonoSymbolFile.ReadSymbolFile(new FileStream(mdbFilename, FileMode.Open, FileAccess.Read));
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0003DA84 File Offset: 0x0003BC84
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename, Guid assemblyGuid)
		{
			MonoSymbolFile monoSymbolFile = MonoSymbolFile.ReadSymbolFile(mdbFilename);
			if (assemblyGuid != monoSymbolFile.guid)
			{
				throw new MonoSymbolFileException("Symbol file `{0}' does not match assembly", new object[] { mdbFilename });
			}
			return monoSymbolFile;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0003DABC File Offset: 0x0003BCBC
		public static MonoSymbolFile ReadSymbolFile(Stream stream)
		{
			return new MonoSymbolFile(stream);
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0003DAC4 File Offset: 0x0003BCC4
		public int CompileUnitCount
		{
			get
			{
				return this.ot.CompileUnitCount;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0003DAD1 File Offset: 0x0003BCD1
		public int SourceCount
		{
			get
			{
				return this.ot.SourceCount;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x0003DADE File Offset: 0x0003BCDE
		public int MethodCount
		{
			get
			{
				return this.ot.MethodCount;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0003DAEB File Offset: 0x0003BCEB
		public int TypeCount
		{
			get
			{
				return this.ot.TypeCount;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0003DAF8 File Offset: 0x0003BCF8
		public int AnonymousScopeCount
		{
			get
			{
				return this.ot.AnonymousScopeCount;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001339 RID: 4921 RVA: 0x0003DB05 File Offset: 0x0003BD05
		public int NamespaceCount
		{
			get
			{
				return this.last_namespace_index;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x0003DB0D File Offset: 0x0003BD0D
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x0003DB15 File Offset: 0x0003BD15
		public OffsetTable OffsetTable
		{
			get
			{
				return this.ot;
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0003DB20 File Offset: 0x0003BD20
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

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x0003DBF8 File Offset: 0x0003BDF8
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

		// Token: 0x0600133E RID: 4926 RVA: 0x0003DC40 File Offset: 0x0003BE40
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

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x0003DD18 File Offset: 0x0003BF18
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

		// Token: 0x06001340 RID: 4928 RVA: 0x0003DD60 File Offset: 0x0003BF60
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

		// Token: 0x06001341 RID: 4929 RVA: 0x0003DE3C File Offset: 0x0003C03C
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

		// Token: 0x06001342 RID: 4930 RVA: 0x0003DE98 File Offset: 0x0003C098
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

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001343 RID: 4931 RVA: 0x0003DF0C File Offset: 0x0003C10C
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

		// Token: 0x06001344 RID: 4932 RVA: 0x0003DF74 File Offset: 0x0003C174
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

		// Token: 0x06001345 RID: 4933 RVA: 0x0003E018 File Offset: 0x0003C218
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

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0003E0E4 File Offset: 0x0003C2E4
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

		// Token: 0x06001347 RID: 4935 RVA: 0x0003E0FA File Offset: 0x0003C2FA
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0003E103 File Offset: 0x0003C303
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
		}

		// Token: 0x040009AC RID: 2476
		private List<MethodEntry> methods = new List<MethodEntry>();

		// Token: 0x040009AD RID: 2477
		private List<SourceFileEntry> sources = new List<SourceFileEntry>();

		// Token: 0x040009AE RID: 2478
		private List<CompileUnitEntry> comp_units = new List<CompileUnitEntry>();

		// Token: 0x040009AF RID: 2479
		private Dictionary<int, AnonymousScopeEntry> anonymous_scopes;

		// Token: 0x040009B0 RID: 2480
		private OffsetTable ot;

		// Token: 0x040009B1 RID: 2481
		private int last_type_index;

		// Token: 0x040009B2 RID: 2482
		private int last_method_index;

		// Token: 0x040009B3 RID: 2483
		private int last_namespace_index;

		// Token: 0x040009B4 RID: 2484
		public readonly int MajorVersion = 50;

		// Token: 0x040009B5 RID: 2485
		public readonly int MinorVersion;

		// Token: 0x040009B6 RID: 2486
		public int NumLineNumbers;

		// Token: 0x040009B7 RID: 2487
		private MyBinaryReader reader;

		// Token: 0x040009B8 RID: 2488
		private Dictionary<int, SourceFileEntry> source_file_hash;

		// Token: 0x040009B9 RID: 2489
		private Dictionary<int, CompileUnitEntry> compile_unit_hash;

		// Token: 0x040009BA RID: 2490
		private List<MethodEntry> method_list;

		// Token: 0x040009BB RID: 2491
		private Dictionary<int, MethodEntry> method_token_hash;

		// Token: 0x040009BC RID: 2492
		private Dictionary<string, int> source_name_hash;

		// Token: 0x040009BD RID: 2493
		private Guid guid;

		// Token: 0x040009BE RID: 2494
		internal int LineNumberCount;

		// Token: 0x040009BF RID: 2495
		internal int LocalCount;

		// Token: 0x040009C0 RID: 2496
		internal int StringSize;

		// Token: 0x040009C1 RID: 2497
		internal int LineNumberSize;

		// Token: 0x040009C2 RID: 2498
		internal int ExtendedLineNumberSize;
	}
}
