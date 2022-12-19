using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000318 RID: 792
	public sealed class MdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x060013F6 RID: 5110 RVA: 0x00040BF7 File Offset: 0x0003EDF7
		public MdbReader(ModuleDefinition module, MonoSymbolFile symFile)
		{
			this.module = module;
			this.symbol_file = symFile;
			this.documents = new Dictionary<string, Document>();
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00040C18 File Offset: 0x0003EE18
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new MdbWriterProvider();
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00040C1F File Offset: 0x0003EE1F
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.symbol_file.Guid == this.module.Mvid;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00040C3C File Offset: 0x0003EE3C
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

		// Token: 0x060013FA RID: 5114 RVA: 0x00040C93 File Offset: 0x0003EE93
		private static int ReadCodeSize(MethodDefinition method)
		{
			return method.Module.Read<MethodDefinition, int>(method, (MethodDefinition m, MetadataReader reader) => reader.ReadCodeSize(m));
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00040CC0 File Offset: 0x0003EEC0
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

		// Token: 0x060013FC RID: 5116 RVA: 0x00040D28 File Offset: 0x0003EF28
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

		// Token: 0x060013FD RID: 5117 RVA: 0x00040D98 File Offset: 0x0003EF98
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

		// Token: 0x060013FE RID: 5118 RVA: 0x00040DE0 File Offset: 0x0003EFE0
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

		// Token: 0x060013FF RID: 5119 RVA: 0x00040EB8 File Offset: 0x0003F0B8
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

		// Token: 0x06001400 RID: 5120 RVA: 0x00040F68 File Offset: 0x0003F168
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

		// Token: 0x06001401 RID: 5121 RVA: 0x00040FC9 File Offset: 0x0003F1C9
		public void Dispose()
		{
			this.symbol_file.Dispose();
		}

		// Token: 0x04000A5B RID: 2651
		private readonly ModuleDefinition module;

		// Token: 0x04000A5C RID: 2652
		private readonly MonoSymbolFile symbol_file;

		// Token: 0x04000A5D RID: 2653
		private readonly Dictionary<string, Document> documents;
	}
}
