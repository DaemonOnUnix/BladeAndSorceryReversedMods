using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000226 RID: 550
	internal sealed class MdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x0600109A RID: 4250 RVA: 0x000390DA File Offset: 0x000372DA
		public MdbWriter(Guid mvid, string assembly)
		{
			this.mvid = mvid;
			this.writer = new MonoSymbolWriter(assembly);
			this.source_files = new Dictionary<string, MdbWriter.SourceFile>();
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00039100 File Offset: 0x00037300
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new MdbReaderProvider();
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00039108 File Offset: 0x00037308
		private MdbWriter.SourceFile GetSourceFile(Document document)
		{
			string url = document.Url;
			MdbWriter.SourceFile sourceFile;
			if (this.source_files.TryGetValue(url, out sourceFile))
			{
				return sourceFile;
			}
			SourceFileEntry sourceFileEntry = this.writer.DefineDocument(url, null, (document.Hash != null && document.Hash.Length == 16) ? document.Hash : null);
			sourceFile = new MdbWriter.SourceFile(this.writer.DefineCompilationUnit(sourceFileEntry), sourceFileEntry);
			this.source_files.Add(url, sourceFile);
			return sourceFile;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0003917C File Offset: 0x0003737C
		private void Populate(Collection<SequencePoint> sequencePoints, int[] offsets, int[] startRows, int[] endRows, int[] startCols, int[] endCols, out MdbWriter.SourceFile file)
		{
			MdbWriter.SourceFile sourceFile = null;
			for (int i = 0; i < sequencePoints.Count; i++)
			{
				SequencePoint sequencePoint = sequencePoints[i];
				offsets[i] = sequencePoint.Offset;
				if (sourceFile == null)
				{
					sourceFile = this.GetSourceFile(sequencePoint.Document);
				}
				startRows[i] = sequencePoint.StartLine;
				endRows[i] = sequencePoint.EndLine;
				startCols[i] = sequencePoint.StartColumn;
				endCols[i] = sequencePoint.EndColumn;
			}
			file = sourceFile;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x000391E8 File Offset: 0x000373E8
		public void Write(MethodDebugInformation info)
		{
			MdbWriter.SourceMethod sourceMethod = new MdbWriter.SourceMethod(info.method);
			Collection<SequencePoint> sequencePoints = info.SequencePoints;
			int count = sequencePoints.Count;
			if (count == 0)
			{
				return;
			}
			int[] array = new int[count];
			int[] array2 = new int[count];
			int[] array3 = new int[count];
			int[] array4 = new int[count];
			int[] array5 = new int[count];
			MdbWriter.SourceFile sourceFile;
			this.Populate(sequencePoints, array, array2, array3, array4, array5, out sourceFile);
			SourceMethodBuilder sourceMethodBuilder = this.writer.OpenMethod(sourceFile.CompilationUnit, 0, sourceMethod);
			for (int i = 0; i < count; i++)
			{
				sourceMethodBuilder.MarkSequencePoint(array[i], sourceFile.CompilationUnit.SourceFile, array2[i], array4[i], array3[i], array5[i], false);
			}
			if (info.scope != null)
			{
				this.WriteRootScope(info.scope, info);
			}
			this.writer.CloseMethod();
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x000392BE File Offset: 0x000374BE
		private void WriteRootScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000392DC File Offset: 0x000374DC
		private void WriteScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.writer.OpenScope(scope.Start.Offset);
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
			this.writer.CloseScope(scope.End.IsEndOfMethod ? info.code_size : scope.End.Offset);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00039350 File Offset: 0x00037550
		private void WriteScopes(Collection<ScopeDebugInformation> scopes, MethodDebugInformation info)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.WriteScope(scopes[i], info);
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0003937C File Offset: 0x0003757C
		private void WriteScopeVariables(ScopeDebugInformation scope)
		{
			if (!scope.HasVariables)
			{
				return;
			}
			foreach (VariableDebugInformation variableDebugInformation in scope.variables)
			{
				if (!string.IsNullOrEmpty(variableDebugInformation.Name))
				{
					this.writer.DefineLocalVariable(variableDebugInformation.Index, variableDebugInformation.Name);
				}
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x000393F8 File Offset: 0x000375F8
		public ImageDebugHeader GetDebugHeader()
		{
			return new ImageDebugHeader();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x000393FF File Offset: 0x000375FF
		public void Dispose()
		{
			this.writer.WriteSymbolFile(this.mvid);
		}

		// Token: 0x04000A21 RID: 2593
		private readonly Guid mvid;

		// Token: 0x04000A22 RID: 2594
		private readonly MonoSymbolWriter writer;

		// Token: 0x04000A23 RID: 2595
		private readonly Dictionary<string, MdbWriter.SourceFile> source_files;

		// Token: 0x02000227 RID: 551
		private class SourceFile : ISourceFile
		{
			// Token: 0x17000378 RID: 888
			// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00039412 File Offset: 0x00037612
			public SourceFileEntry Entry
			{
				get
				{
					return this.entry;
				}
			}

			// Token: 0x17000379 RID: 889
			// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0003941A File Offset: 0x0003761A
			public CompileUnitEntry CompilationUnit
			{
				get
				{
					return this.compilation_unit;
				}
			}

			// Token: 0x060010A7 RID: 4263 RVA: 0x00039422 File Offset: 0x00037622
			public SourceFile(CompileUnitEntry comp_unit, SourceFileEntry entry)
			{
				this.compilation_unit = comp_unit;
				this.entry = entry;
			}

			// Token: 0x04000A24 RID: 2596
			private readonly CompileUnitEntry compilation_unit;

			// Token: 0x04000A25 RID: 2597
			private readonly SourceFileEntry entry;
		}

		// Token: 0x02000228 RID: 552
		private class SourceMethod : IMethodDef
		{
			// Token: 0x1700037A RID: 890
			// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00039438 File Offset: 0x00037638
			public string Name
			{
				get
				{
					return this.method.Name;
				}
			}

			// Token: 0x1700037B RID: 891
			// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00039448 File Offset: 0x00037648
			public int Token
			{
				get
				{
					return this.method.MetadataToken.ToInt32();
				}
			}

			// Token: 0x060010AA RID: 4266 RVA: 0x00039468 File Offset: 0x00037668
			public SourceMethod(MethodDefinition method)
			{
				this.method = method;
			}

			// Token: 0x04000A26 RID: 2598
			private readonly MethodDefinition method;
		}
	}
}
