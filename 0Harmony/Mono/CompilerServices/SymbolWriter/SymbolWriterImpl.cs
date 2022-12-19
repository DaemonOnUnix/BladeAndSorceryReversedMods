using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000313 RID: 787
	internal class SymbolWriterImpl : ISymbolWriter
	{
		// Token: 0x060013D3 RID: 5075 RVA: 0x000407E0 File Offset: 0x0003E9E0
		public SymbolWriterImpl(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00040805 File Offset: 0x0003EA05
		public void Close()
		{
			this.msw.WriteSymbolFile(this.guid);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00040818 File Offset: 0x0003EA18
		public void CloseMethod()
		{
			if (this.methodOpened)
			{
				this.methodOpened = false;
				this.nextLocalIndex = 0;
				this.msw.CloseMethod();
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0004083B File Offset: 0x0003EA3B
		public void CloseNamespace()
		{
			this.namespaceStack.Pop();
			this.msw.CloseNamespace();
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00040854 File Offset: 0x0003EA54
		public void CloseScope(int endOffset)
		{
			this.msw.CloseScope(endOffset);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00040864 File Offset: 0x0003EA64
		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			SymbolDocumentWriterImpl symbolDocumentWriterImpl = (SymbolDocumentWriterImpl)this.documents[url];
			if (symbolDocumentWriterImpl == null)
			{
				SourceFileEntry sourceFileEntry = this.msw.DefineDocument(url);
				symbolDocumentWriterImpl = new SymbolDocumentWriterImpl(this.msw.DefineCompilationUnit(sourceFileEntry));
				this.documents[url] = symbolDocumentWriterImpl;
			}
			return symbolDocumentWriterImpl;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00018105 File Offset: 0x00016305
		public void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00018105 File Offset: 0x00016305
		public void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000408B4 File Offset: 0x0003EAB4
		public void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			MonoSymbolWriter monoSymbolWriter = this.msw;
			int num = this.nextLocalIndex;
			this.nextLocalIndex = num + 1;
			monoSymbolWriter.DefineLocalVariable(num, name);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00018105 File Offset: 0x00016305
		public void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000408E0 File Offset: 0x0003EAE0
		public void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			SymbolDocumentWriterImpl symbolDocumentWriterImpl = (SymbolDocumentWriterImpl)document;
			SourceFileEntry sourceFileEntry = ((symbolDocumentWriterImpl != null) ? symbolDocumentWriterImpl.Entry.SourceFile : null);
			for (int i = 0; i < offsets.Length; i++)
			{
				if (i <= 0 || offsets[i] != offsets[i - 1] || lines[i] != lines[i - 1] || columns[i] != columns[i - 1])
				{
					this.msw.MarkSequencePoint(offsets[i], sourceFileEntry, lines[i], columns[i], false);
				}
			}
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0004094F File Offset: 0x0003EB4F
		public void Initialize(IntPtr emitter, string filename, bool fFullBuild)
		{
			this.msw = new MonoSymbolWriter(filename);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0004095D File Offset: 0x0003EB5D
		public void OpenMethod(SymbolToken method)
		{
			this.currentToken = method.GetToken();
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0004096C File Offset: 0x0003EB6C
		public void OpenNamespace(string name)
		{
			NamespaceInfo namespaceInfo = new NamespaceInfo();
			namespaceInfo.NamespaceID = -1;
			namespaceInfo.Name = name;
			this.namespaceStack.Push(namespaceInfo);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00040999 File Offset: 0x0003EB99
		public int OpenScope(int startOffset)
		{
			return this.msw.OpenScope(startOffset);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000409A8 File Offset: 0x0003EBA8
		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
			int currentNamespace = this.GetCurrentNamespace(startDoc);
			SourceMethodImpl sourceMethodImpl = new SourceMethodImpl(this.methodName, this.currentToken, currentNamespace);
			this.msw.OpenMethod(((ICompileUnit)startDoc).Entry, currentNamespace, sourceMethodImpl);
			this.methodOpened = true;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x00018105 File Offset: 0x00016305
		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000409F0 File Offset: 0x0003EBF0
		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
			if (name == "__name")
			{
				this.methodName = Encoding.UTF8.GetString(data);
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00018105 File Offset: 0x00016305
		public void SetUnderlyingWriter(IntPtr underlyingWriter)
		{
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00018105 File Offset: 0x00016305
		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00040A10 File Offset: 0x0003EC10
		public void UsingNamespace(string fullName)
		{
			if (this.namespaceStack.Count == 0)
			{
				this.OpenNamespace("");
			}
			NamespaceInfo namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
			if (namespaceInfo.NamespaceID != -1)
			{
				NamespaceInfo namespaceInfo2 = namespaceInfo;
				this.CloseNamespace();
				this.OpenNamespace(namespaceInfo2.Name);
				namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
				namespaceInfo.UsingClauses = namespaceInfo2.UsingClauses;
			}
			namespaceInfo.UsingClauses.Add(fullName);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00040A90 File Offset: 0x0003EC90
		private int GetCurrentNamespace(ISymbolDocumentWriter doc)
		{
			if (this.namespaceStack.Count == 0)
			{
				this.OpenNamespace("");
			}
			NamespaceInfo namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
			if (namespaceInfo.NamespaceID == -1)
			{
				string[] array = (string[])namespaceInfo.UsingClauses.ToArray(typeof(string));
				int num = 0;
				if (this.namespaceStack.Count > 1)
				{
					this.namespaceStack.Pop();
					num = ((NamespaceInfo)this.namespaceStack.Peek()).NamespaceID;
					this.namespaceStack.Push(namespaceInfo);
				}
				namespaceInfo.NamespaceID = this.msw.DefineNamespace(namespaceInfo.Name, ((ICompileUnit)doc).Entry, array, num);
			}
			return namespaceInfo.NamespaceID;
		}

		// Token: 0x04000A4C RID: 2636
		private MonoSymbolWriter msw;

		// Token: 0x04000A4D RID: 2637
		private int nextLocalIndex;

		// Token: 0x04000A4E RID: 2638
		private int currentToken;

		// Token: 0x04000A4F RID: 2639
		private string methodName;

		// Token: 0x04000A50 RID: 2640
		private Stack namespaceStack = new Stack();

		// Token: 0x04000A51 RID: 2641
		private bool methodOpened;

		// Token: 0x04000A52 RID: 2642
		private Hashtable documents = new Hashtable();

		// Token: 0x04000A53 RID: 2643
		private Guid guid;
	}
}
