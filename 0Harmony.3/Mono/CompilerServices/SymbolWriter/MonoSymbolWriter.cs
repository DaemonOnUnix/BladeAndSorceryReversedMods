using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021B RID: 539
	internal class MonoSymbolWriter
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x000381E8 File Offset: 0x000363E8
		public MonoSymbolWriter(string filename)
		{
			this.methods = new List<SourceMethodBuilder>();
			this.sources = new List<SourceFileEntry>();
			this.comp_units = new List<CompileUnitEntry>();
			this.file = new MonoSymbolFile();
			this.filename = filename + ".mdb";
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00038243 File Offset: 0x00036443
		public MonoSymbolFile SymbolFile
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00012279 File Offset: 0x00010479
		public void CloseNamespace()
		{
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0003824B File Offset: 0x0003644B
		public void DefineLocalVariable(int index, string name)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddLocal(index, name);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00038263 File Offset: 0x00036463
		public void DefineCapturedLocal(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Local);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00038274 File Offset: 0x00036474
		public void DefineCapturedParameter(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Parameter);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00038285 File Offset: 0x00036485
		public void DefineCapturedThis(int scope_id, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, "this", captured_name, CapturedVariable.CapturedKind.This);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0003829A File Offset: 0x0003649A
		public void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			this.file.DefineCapturedScope(scope_id, id, captured_name);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000382AA File Offset: 0x000364AA
		public void DefineScopeVariable(int scope, int index)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddScopeVariable(scope, index);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000382C2 File Offset: 0x000364C2
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.MarkSequencePoint(offset, file, line, column, is_hidden);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000382E0 File Offset: 0x000364E0
		public SourceMethodBuilder OpenMethod(ICompileUnit file, int ns_id, IMethodDef method)
		{
			SourceMethodBuilder sourceMethodBuilder = new SourceMethodBuilder(file, ns_id, method);
			this.current_method_stack.Push(this.current_method);
			this.current_method = sourceMethodBuilder;
			this.methods.Add(this.current_method);
			return sourceMethodBuilder;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00038320 File Offset: 0x00036520
		public void CloseMethod()
		{
			this.current_method = this.current_method_stack.Pop();
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00038334 File Offset: 0x00036534
		public SourceFileEntry DefineDocument(string url)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003835C File Offset: 0x0003655C
		public SourceFileEntry DefineDocument(string url, byte[] guid, byte[] checksum)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url, guid, checksum);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00038388 File Offset: 0x00036588
		public CompileUnitEntry DefineCompilationUnit(SourceFileEntry source)
		{
			CompileUnitEntry compileUnitEntry = new CompileUnitEntry(this.file, source);
			this.comp_units.Add(compileUnitEntry);
			return compileUnitEntry;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000383AF File Offset: 0x000365AF
		public int DefineNamespace(string name, CompileUnitEntry unit, string[] using_clauses, int parent)
		{
			if (unit == null || using_clauses == null)
			{
				throw new NullReferenceException();
			}
			return unit.DefineNamespace(name, using_clauses, parent);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000383C7 File Offset: 0x000365C7
		public int OpenScope(int start_offset)
		{
			if (this.current_method == null)
			{
				return 0;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.Lexical, start_offset);
			return 0;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x000383E1 File Offset: 0x000365E1
		public void CloseScope(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000383F8 File Offset: 0x000365F8
		public void OpenCompilerGeneratedBlock(int start_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.CompilerGenerated, start_offset);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000383E1 File Offset: 0x000365E1
		public void CloseCompilerGeneratedBlock(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00038410 File Offset: 0x00036610
		public void StartIteratorBody(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorBody, start_offset);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003841F File Offset: 0x0003661F
		public void EndIteratorBody(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0003842D File Offset: 0x0003662D
		public void StartIteratorDispatcher(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorDispatcher, start_offset);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0003841F File Offset: 0x0003661F
		public void EndIteratorDispatcher(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003843C File Offset: 0x0003663C
		public void DefineAnonymousScope(int id)
		{
			this.file.DefineAnonymousScope(id);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0003844C File Offset: 0x0003664C
		public void WriteSymbolFile(Guid guid)
		{
			foreach (SourceMethodBuilder sourceMethodBuilder in this.methods)
			{
				sourceMethodBuilder.DefineMethod(this.file);
			}
			try
			{
				File.Delete(this.filename);
			}
			catch
			{
			}
			using (FileStream fileStream = new FileStream(this.filename, FileMode.Create, FileAccess.Write))
			{
				this.file.CreateSymbolFile(guid, fileStream);
			}
		}

		// Token: 0x040009FE RID: 2558
		private List<SourceMethodBuilder> methods;

		// Token: 0x040009FF RID: 2559
		private List<SourceFileEntry> sources;

		// Token: 0x04000A00 RID: 2560
		private List<CompileUnitEntry> comp_units;

		// Token: 0x04000A01 RID: 2561
		protected readonly MonoSymbolFile file;

		// Token: 0x04000A02 RID: 2562
		private string filename;

		// Token: 0x04000A03 RID: 2563
		private SourceMethodBuilder current_method;

		// Token: 0x04000A04 RID: 2564
		private Stack<SourceMethodBuilder> current_method_stack = new Stack<SourceMethodBuilder>();
	}
}
