using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000222 RID: 546
	public sealed class MdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x00038CAB File Offset: 0x00036EAB
		public MdbReader(ModuleDefinition module, MonoSymbolFile symFile)
		{
			this.module = module;
			this.symbol_file = symFile;
			this.documents = new Dictionary<string, Document>();
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00038CCC File Offset: 0x00036ECC
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new MdbWriterProvider();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00038CD3 File Offset: 0x00036ED3
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.symbol_file.Guid == this.module.Mvid;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00038CF0 File Offset: 0x00036EF0
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MetadataToken metadataToken = method.MetadataToken;
			MethodEntry methodByToken = this.symbol_file.GetMethodByToken(metadataToken.ToInt32());
			if (methodByToken == null)
			{
				return null;
			}
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			methodDebugInformation.code_size = MdbReader.ReadCodeSize(method);
			ScopeDebugInformation[] array = MdbReader.ReadScopes(methodByToken, methodDebugInformation);
			this.ReadLineNumbers(methodByToken, methodDebugInformation);
			MdbReader.ReadLocalVariables(methodByToken, array);
			return methodDebugInformation;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00038D47 File Offset: 0x00036F47
		private static int ReadCodeSize(MethodDefinition method)
		{
			return method.Module.Read<MethodDefinition, int>(method, (MethodDefinition m, MetadataReader reader) => reader.ReadCodeSize(m));
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00038D74 File Offset: 0x00036F74
		private static void ReadLocalVariables(MethodEntry entry, ScopeDebugInformation[] scopes)
		{
			foreach (LocalVariableEntry localVariableEntry in entry.GetLocals())
			{
				VariableDebugInformation variableDebugInformation = new VariableDebugInformation(localVariableEntry.Index, localVariableEntry.Name);
				int blockIndex = localVariableEntry.BlockIndex;
				if (blockIndex >= 0 && blockIndex < scopes.Length)
				{
					ScopeDebugInformation scopeDebugInformation = scopes[blockIndex];
					if (scopeDebugInformation != null)
					{
						scopeDebugInformation.Variables.Add(variableDebugInformation);
					}
				}
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00038DDC File Offset: 0x00036FDC
		private void ReadLineNumbers(MethodEntry entry, MethodDebugInformation info)
		{
			LineNumberTable lineNumberTable = entry.GetLineNumberTable();
			info.sequence_points = new Collection<SequencePoint>(lineNumberTable.LineNumbers.Length);
			for (int i = 0; i < lineNumberTable.LineNumbers.Length; i++)
			{
				LineNumberEntry lineNumberEntry = lineNumberTable.LineNumbers[i];
				if (i <= 0 || lineNumberTable.LineNumbers[i - 1].Offset != lineNumberEntry.Offset)
				{
					info.sequence_points.Add(this.LineToSequencePoint(lineNumberEntry));
				}
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00038E4C File Offset: 0x0003704C
		private Document GetDocument(SourceFileEntry file)
		{
			string fileName = file.FileName;
			Document document;
			if (this.documents.TryGetValue(fileName, out document))
			{
				return document;
			}
			document = new Document(fileName)
			{
				Hash = file.Checksum
			};
			this.documents.Add(fileName, document);
			return document;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00038E94 File Offset: 0x00037094
		private static ScopeDebugInformation[] ReadScopes(MethodEntry entry, MethodDebugInformation info)
		{
			CodeBlockEntry[] codeBlocks = entry.GetCodeBlocks();
			ScopeDebugInformation[] array = new ScopeDebugInformation[codeBlocks.Length + 1];
			ScopeDebugInformation[] array2 = array;
			int num = 0;
			ScopeDebugInformation scopeDebugInformation = new ScopeDebugInformation();
			scopeDebugInformation.Start = new InstructionOffset(0);
			scopeDebugInformation.End = new InstructionOffset(info.code_size);
			ScopeDebugInformation scopeDebugInformation2 = scopeDebugInformation;
			array2[num] = scopeDebugInformation;
			info.scope = scopeDebugInformation2;
			foreach (CodeBlockEntry codeBlockEntry in codeBlocks)
			{
				if (codeBlockEntry.BlockType == CodeBlockEntry.Type.Lexical || codeBlockEntry.BlockType == CodeBlockEntry.Type.CompilerGenerated)
				{
					ScopeDebugInformation scopeDebugInformation3 = new ScopeDebugInformation();
					scopeDebugInformation3.Start = new InstructionOffset(codeBlockEntry.StartOffset);
					scopeDebugInformation3.End = new InstructionOffset(codeBlockEntry.EndOffset);
					array[codeBlockEntry.Index + 1] = scopeDebugInformation3;
					if (!MdbReader.AddScope(info.scope.Scopes, scopeDebugInformation3))
					{
						info.scope.Scopes.Add(scopeDebugInformation3);
					}
				}
			}
			return array;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00038F6C File Offset: 0x0003716C
		private static bool AddScope(Collection<ScopeDebugInformation> scopes, ScopeDebugInformation scope)
		{
			foreach (ScopeDebugInformation scopeDebugInformation in scopes)
			{
				if (scopeDebugInformation.HasScopes && MdbReader.AddScope(scopeDebugInformation.Scopes, scope))
				{
					return true;
				}
				if (scope.Start.Offset >= scopeDebugInformation.Start.Offset && scope.End.Offset <= scopeDebugInformation.End.Offset)
				{
					scopeDebugInformation.Scopes.Add(scope);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0003901C File Offset: 0x0003721C
		private SequencePoint LineToSequencePoint(LineNumberEntry line)
		{
			SourceFileEntry sourceFile = this.symbol_file.GetSourceFile(line.File);
			return new SequencePoint(line.Offset, this.GetDocument(sourceFile))
			{
				StartLine = line.Row,
				EndLine = line.EndRow,
				StartColumn = line.Column,
				EndColumn = line.EndColumn
			};
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0003907D File Offset: 0x0003727D
		public void Dispose()
		{
			this.symbol_file.Dispose();
		}

		// Token: 0x04000A1C RID: 2588
		private readonly ModuleDefinition module;

		// Token: 0x04000A1D RID: 2589
		private readonly MonoSymbolFile symbol_file;

		// Token: 0x04000A1E RID: 2590
		private readonly Dictionary<string, Document> documents;
	}
}
