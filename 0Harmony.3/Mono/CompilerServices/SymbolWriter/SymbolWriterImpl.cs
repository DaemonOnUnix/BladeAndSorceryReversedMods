using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021D RID: 541
	internal class SymbolWriterImpl : ISymbolWriter
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x00038894 File Offset: 0x00036A94
		public SymbolWriterImpl(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000388B9 File Offset: 0x00036AB9
		public void Close()
		{
			this.msw.WriteSymbolFile(this.guid);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000388CC File Offset: 0x00036ACC
		public void CloseMethod()
		{
			if (this.methodOpened)
			{
				this.methodOpened = false;
				this.nextLocalIndex = 0;
				this.msw.CloseMethod();
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000388EF File Offset: 0x00036AEF
		public void CloseNamespace()
		{
			this.namespaceStack.Pop();
			this.msw.CloseNamespace();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00038908 File Offset: 0x00036B08
		public void CloseScope(int endOffset)
		{
			this.msw.CloseScope(endOffset);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00038918 File Offset: 0x00036B18
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

		// Token: 0x06001069 RID: 4201 RVA: 0x00012279 File Offset: 0x00010479
		public void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00012279 File Offset: 0x00010479
		public void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00038968 File Offset: 0x00036B68
		public void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			MonoSymbolWriter monoSymbolWriter = this.msw;
			int num = this.nextLocalIndex;
			this.nextLocalIndex = num + 1;
			monoSymbolWriter.DefineLocalVariable(num, name);
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00012279 File Offset: 0x00010479
		public void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00038994 File Offset: 0x00036B94
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

		// Token: 0x0600106E RID: 4206 RVA: 0x00038A03 File Offset: 0x00036C03
		public void Initialize(IntPtr emitter, string filename, bool fFullBuild)
		{
			this.msw = new MonoSymbolWriter(filename);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00038A11 File Offset: 0x00036C11
		public void OpenMethod(SymbolToken method)
		{
			this.currentToken = method.GetToken();
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00038A20 File Offset: 0x00036C20
		public void OpenNamespace(string name)
		{
			NamespaceInfo namespaceInfo = new NamespaceInfo();
			namespaceInfo.NamespaceID = -1;
			namespaceInfo.Name = name;
			this.namespaceStack.Push(namespaceInfo);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00038A4D File Offset: 0x00036C4D
		public int OpenScope(int startOffset)
		{
			return this.msw.OpenScope(startOffset);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00038A5C File Offset: 0x00036C5C
		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
			int currentNamespace = this.GetCurrentNamespace(startDoc);
			SourceMethodImpl sourceMethodImpl = new SourceMethodImpl(this.methodName, this.currentToken, currentNamespace);
			this.msw.OpenMethod(((ICompileUnit)startDoc).Entry, currentNamespace, sourceMethodImpl);
			this.methodOpened = true;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00012279 File Offset: 0x00010479
		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00038AA4 File Offset: 0x00036CA4
		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
			if (name == "__name")
			{
				this.methodName = Encoding.UTF8.GetString(data);
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00012279 File Offset: 0x00010479
		public void SetUnderlyingWriter(IntPtr underlyingWriter)
		{
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00012279 File Offset: 0x00010479
		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00038AC4 File Offset: 0x00036CC4
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

		// Token: 0x06001078 RID: 4216 RVA: 0x00038B44 File Offset: 0x00036D44
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

		// Token: 0x04000A0D RID: 2573
		private MonoSymbolWriter msw;

		// Token: 0x04000A0E RID: 2574
		private int nextLocalIndex;

		// Token: 0x04000A0F RID: 2575
		private int currentToken;

		// Token: 0x04000A10 RID: 2576
		private string methodName;

		// Token: 0x04000A11 RID: 2577
		private Stack namespaceStack = new Stack();

		// Token: 0x04000A12 RID: 2578
		private bool methodOpened;

		// Token: 0x04000A13 RID: 2579
		private Hashtable documents = new Hashtable();

		// Token: 0x04000A14 RID: 2580
		private Guid guid;
	}
}
