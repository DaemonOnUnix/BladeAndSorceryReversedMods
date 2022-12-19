using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200031C RID: 796
	internal sealed class MdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x0600140A RID: 5130 RVA: 0x00041021 File Offset: 0x0003F221
		public MdbWriter(ModuleDefinition module, string assembly)
		{
			this.module = module;
			this.writer = new MonoSymbolWriter(assembly);
			this.source_files = new Dictionary<string, MdbWriter.SourceFile>();
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00041047 File Offset: 0x0003F247
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new MdbReaderProvider();
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00041050 File Offset: 0x0003F250
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

		// Token: 0x0600140D RID: 5133 RVA: 0x000410C4 File Offset: 0x0003F2C4
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

		// Token: 0x0600140E RID: 5134 RVA: 0x00041130 File Offset: 0x0003F330
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

		// Token: 0x0600140F RID: 5135 RVA: 0x00041206 File Offset: 0x0003F406
		private void WriteRootScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00041224 File Offset: 0x0003F424
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

		// Token: 0x06001411 RID: 5137 RVA: 0x00041298 File Offset: 0x0003F498
		private void WriteScopes(Collection<ScopeDebugInformation> scopes, MethodDebugInformation info)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.WriteScope(scopes[i], info);
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000412C4 File Offset: 0x0003F4C4
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

		// Token: 0x06001413 RID: 5139 RVA: 0x00041340 File Offset: 0x0003F540
		public ImageDebugHeader GetDebugHeader()
		{
			return new ImageDebugHeader();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00041347 File Offset: 0x0003F547
		public void Dispose()
		{
			this.writer.WriteSymbolFile(this.module.Mvid);
		}

		// Token: 0x04000A60 RID: 2656
		private readonly ModuleDefinition module;

		// Token: 0x04000A61 RID: 2657
		private readonly MonoSymbolWriter writer;

		// Token: 0x04000A62 RID: 2658
		private readonly Dictionary<string, MdbWriter.SourceFile> source_files;

		// Token: 0x0200031D RID: 797
		private class SourceFile : ISourceFile
		{
			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x06001415 RID: 5141 RVA: 0x0004135F File Offset: 0x0003F55F
			public SourceFileEntry Entry
			{
				get
				{
					return this.entry;
				}
			}

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x06001416 RID: 5142 RVA: 0x00041367 File Offset: 0x0003F567
			public CompileUnitEntry CompilationUnit
			{
				get
				{
					return this.compilation_unit;
				}
			}

			// Token: 0x06001417 RID: 5143 RVA: 0x0004136F File Offset: 0x0003F56F
			public SourceFile(CompileUnitEntry comp_unit, SourceFileEntry entry)
			{
				this.compilation_unit = comp_unit;
				this.entry = entry;
			}

			// Token: 0x04000A63 RID: 2659
			private readonly CompileUnitEntry compilation_unit;

			// Token: 0x04000A64 RID: 2660
			private readonly SourceFileEntry entry;
		}

		// Token: 0x0200031E RID: 798
		private class SourceMethod : IMethodDef
		{
			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x06001418 RID: 5144 RVA: 0x00041385 File Offset: 0x0003F585
			public string Name
			{
				get
				{
					return this.method.Name;
				}
			}

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x06001419 RID: 5145 RVA: 0x00041394 File Offset: 0x0003F594
			public int Token
			{
				get
				{
					return this.method.MetadataToken.ToInt32();
				}
			}

			// Token: 0x0600141A RID: 5146 RVA: 0x000413B4 File Offset: 0x0003F5B4
			public SourceMethod(MethodDefinition method)
			{
				this.method = method;
			}

			// Token: 0x04000A65 RID: 2661
			private readonly MethodDefinition method;
		}
	}
}
