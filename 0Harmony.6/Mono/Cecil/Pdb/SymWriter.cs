using System;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023D RID: 573
	internal class SymWriter
	{
		// Token: 0x060011F1 RID: 4593
		[DllImport("ole32.dll")]
		private static extern int CoCreateInstance([In] ref Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] [In] object pUnkOuter, [In] uint dwClsContext, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x060011F2 RID: 4594 RVA: 0x0003AF0C File Offset: 0x0003910C
		public SymWriter()
		{
			object obj;
			SymWriter.CoCreateInstance(ref SymWriter.s_CorSymWriter_SxS_ClassID, null, 1U, ref SymWriter.s_symUnmangedWriterIID, out obj);
			this.writer = (ISymUnmanagedWriter2)obj;
			this.documents = new Collection<ISymUnmanagedDocumentWriter>();
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0003AF4C File Offset: 0x0003914C
		public byte[] GetDebugInfo(out ImageDebugDirectory idd)
		{
			int num;
			this.writer.GetDebugInfo(out idd, 0, out num, null);
			byte[] array = new byte[num];
			this.writer.GetDebugInfo(out idd, num, out num, array);
			return array;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003AF84 File Offset: 0x00039184
		public void DefineLocalVariable2(string name, VariableAttributes attributes, int sigToken, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			this.writer.DefineLocalVariable2(name, (int)attributes, sigToken, 1, addr1, addr2, addr3, startOffset, endOffset);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0003AFAA File Offset: 0x000391AA
		public void DefineConstant2(string name, object value, int sigToken)
		{
			if (value == null)
			{
				this.writer.DefineConstant2(name, 0, sigToken);
				return;
			}
			this.writer.DefineConstant2(name, value, sigToken);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0003AFD4 File Offset: 0x000391D4
		public void Close()
		{
			this.writer.Close();
			Marshal.ReleaseComObject(this.writer);
			foreach (ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter in this.documents)
			{
				Marshal.ReleaseComObject(symUnmanagedDocumentWriter);
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003B03C File Offset: 0x0003923C
		public void CloseMethod()
		{
			this.writer.CloseMethod();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0003B049 File Offset: 0x00039249
		public void CloseNamespace()
		{
			this.writer.CloseNamespace();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003B056 File Offset: 0x00039256
		public void CloseScope(int endOffset)
		{
			this.writer.CloseScope(endOffset);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0003B064 File Offset: 0x00039264
		public SymDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter;
			this.writer.DefineDocument(url, ref language, ref languageVendor, ref documentType, out symUnmanagedDocumentWriter);
			this.documents.Add(symUnmanagedDocumentWriter);
			return new SymDocumentWriter(symUnmanagedDocumentWriter);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0003B097 File Offset: 0x00039297
		public void DefineSequencePoints(SymDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			this.writer.DefineSequencePoints(document.Writer, offsets.Length, offsets, lines, columns, endLines, endColumns);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0003B0B5 File Offset: 0x000392B5
		public void Initialize(object emitter, string filename, bool fFullBuild)
		{
			this.writer.Initialize(emitter, filename, null, fFullBuild);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0003B0C6 File Offset: 0x000392C6
		public void SetUserEntryPoint(int methodToken)
		{
			this.writer.SetUserEntryPoint(methodToken);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0003B0D4 File Offset: 0x000392D4
		public void OpenMethod(int methodToken)
		{
			this.writer.OpenMethod(methodToken);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0003B0E2 File Offset: 0x000392E2
		public void OpenNamespace(string name)
		{
			this.writer.OpenNamespace(name);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0003B0F0 File Offset: 0x000392F0
		public int OpenScope(int startOffset)
		{
			int num;
			this.writer.OpenScope(startOffset, out num);
			return num;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0003B10C File Offset: 0x0003930C
		public void UsingNamespace(string fullName)
		{
			this.writer.UsingNamespace(fullName);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0003B11C File Offset: 0x0003931C
		public void DefineCustomMetadata(string name, byte[] metadata)
		{
			GCHandle gchandle = GCHandle.Alloc(metadata, GCHandleType.Pinned);
			this.writer.SetSymAttribute(0U, name, (uint)metadata.Length, gchandle.AddrOfPinnedObject());
			gchandle.Free();
		}

		// Token: 0x04000A4D RID: 2637
		private static Guid s_symUnmangedWriterIID = new Guid("0b97726e-9e6d-4f05-9a26-424022093caa");

		// Token: 0x04000A4E RID: 2638
		private static Guid s_CorSymWriter_SxS_ClassID = new Guid("108296c1-281e-11d3-bd22-0000f80849bd");

		// Token: 0x04000A4F RID: 2639
		private readonly ISymUnmanagedWriter2 writer;

		// Token: 0x04000A50 RID: 2640
		private readonly Collection<ISymUnmanagedDocumentWriter> documents;
	}
}
