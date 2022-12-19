using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000311 RID: 785
	internal class MonoSymbolWriter
	{
		// Token: 0x060013A9 RID: 5033 RVA: 0x00040134 File Offset: 0x0003E334
		public MonoSymbolWriter(string filename)
		{
			this.methods = new List<SourceMethodBuilder>();
			this.sources = new List<SourceFileEntry>();
			this.comp_units = new List<CompileUnitEntry>();
			this.file = new MonoSymbolFile();
			this.filename = filename + ".mdb";
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0004018F File Offset: 0x0003E38F
		public MonoSymbolFile SymbolFile
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00018105 File Offset: 0x00016305
		public void CloseNamespace()
		{
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00040197 File Offset: 0x0003E397
		public void DefineLocalVariable(int index, string name)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddLocal(index, name);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x000401AF File Offset: 0x0003E3AF
		public void DefineCapturedLocal(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Local);
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000401C0 File Offset: 0x0003E3C0
		public void DefineCapturedParameter(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Parameter);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000401D1 File Offset: 0x0003E3D1
		public void DefineCapturedThis(int scope_id, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, "this", captured_name, CapturedVariable.CapturedKind.This);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000401E6 File Offset: 0x0003E3E6
		public void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			this.file.DefineCapturedScope(scope_id, id, captured_name);
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x000401F6 File Offset: 0x0003E3F6
		public void DefineScopeVariable(int scope, int index)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddScopeVariable(scope, index);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0004020E File Offset: 0x0003E40E
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.MarkSequencePoint(offset, file, line, column, is_hidden);
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0004022C File Offset: 0x0003E42C
		public SourceMethodBuilder OpenMethod(ICompileUnit file, int ns_id, IMethodDef method)
		{
			SourceMethodBuilder sourceMethodBuilder = new SourceMethodBuilder(file, ns_id, method);
			this.current_method_stack.Push(this.current_method);
			this.current_method = sourceMethodBuilder;
			this.methods.Add(this.current_method);
			return sourceMethodBuilder;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0004026C File Offset: 0x0003E46C
		public void CloseMethod()
		{
			this.current_method = this.current_method_stack.Pop();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00040280 File Offset: 0x0003E480
		public SourceFileEntry DefineDocument(string url)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000402A8 File Offset: 0x0003E4A8
		public SourceFileEntry DefineDocument(string url, byte[] guid, byte[] checksum)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url, guid, checksum);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x000402D4 File Offset: 0x0003E4D4
		public CompileUnitEntry DefineCompilationUnit(SourceFileEntry source)
		{
			CompileUnitEntry compileUnitEntry = new CompileUnitEntry(this.file, source);
			this.comp_units.Add(compileUnitEntry);
			return compileUnitEntry;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000402FB File Offset: 0x0003E4FB
		public int DefineNamespace(string name, CompileUnitEntry unit, string[] using_clauses, int parent)
		{
			if (unit == null || using_clauses == null)
			{
				throw new NullReferenceException();
			}
			return unit.DefineNamespace(name, using_clauses, parent);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00040313 File Offset: 0x0003E513
		public int OpenScope(int start_offset)
		{
			if (this.current_method == null)
			{
				return 0;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.Lexical, start_offset);
			return 0;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0004032D File Offset: 0x0003E52D
		public void CloseScope(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00040344 File Offset: 0x0003E544
		public void OpenCompilerGeneratedBlock(int start_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.CompilerGenerated, start_offset);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0004032D File Offset: 0x0003E52D
		public void CloseCompilerGeneratedBlock(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004035C File Offset: 0x0003E55C
		public void StartIteratorBody(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorBody, start_offset);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004036B File Offset: 0x0003E56B
		public void EndIteratorBody(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00040379 File Offset: 0x0003E579
		public void StartIteratorDispatcher(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorDispatcher, start_offset);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0004036B File Offset: 0x0003E56B
		public void EndIteratorDispatcher(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00040388 File Offset: 0x0003E588
		public void DefineAnonymousScope(int id)
		{
			this.file.DefineAnonymousScope(id);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00040398 File Offset: 0x0003E598
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

		// Token: 0x04000A3D RID: 2621
		private List<SourceMethodBuilder> methods;

		// Token: 0x04000A3E RID: 2622
		private List<SourceFileEntry> sources;

		// Token: 0x04000A3F RID: 2623
		private List<CompileUnitEntry> comp_units;

		// Token: 0x04000A40 RID: 2624
		protected readonly MonoSymbolFile file;

		// Token: 0x04000A41 RID: 2625
		private string filename;

		// Token: 0x04000A42 RID: 2626
		private SourceMethodBuilder current_method;

		// Token: 0x04000A43 RID: 2627
		private Stack<SourceMethodBuilder> current_method_stack = new Stack<SourceMethodBuilder>();
	}
}
