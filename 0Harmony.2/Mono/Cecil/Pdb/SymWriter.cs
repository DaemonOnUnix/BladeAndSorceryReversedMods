using System;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000333 RID: 819
	internal class SymWriter
	{
		// Token: 0x06001560 RID: 5472
		[DllImport("ole32.dll")]
		private static extern int CoCreateInstance([In] ref Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] [In] object pUnkOuter, [In] uint dwClsContext, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06001561 RID: 5473 RVA: 0x00042E54 File Offset: 0x00041054
		public SymWriter()
		{
			object obj;
			SymWriter.CoCreateInstance(ref SymWriter.s_CorSymWriter_SxS_ClassID, null, 1U, ref SymWriter.s_symUnmangedWriterIID, out obj);
			this.writer = (ISymUnmanagedWriter2)obj;
			this.documents = new Collection<ISymUnmanagedDocumentWriter>();
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00042E94 File Offset: 0x00041094
		public byte[] GetDebugInfo(out ImageDebugDirectory idd)
		{
			int num;
			this.writer.GetDebugInfo(out idd, 0, out num, null);
			byte[] array = new byte[num];
			this.writer.GetDebugInfo(out idd, num, out num, array);
			return array;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00042ECC File Offset: 0x000410CC
		public void DefineLocalVariable2(string name, VariableAttributes attributes, int sigToken, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			this.writer.DefineLocalVariable2(name, (int)attributes, sigToken, 1, addr1, addr2, addr3, startOffset, endOffset);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00042EF2 File Offset: 0x000410F2
		public void DefineConstant2(string name, object value, int sigToken)
		{
			if (value == null)
			{
				this.writer.DefineConstant2(name, 0, sigToken);
				return;
			}
			this.writer.DefineConstant2(name, value, sigToken);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00042F1C File Offset: 0x0004111C
		public void Close()
		{
			this.writer.Close();
			Marshal.ReleaseComObject(this.writer);
			foreach (ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter in this.documents)
			{
				Marshal.ReleaseComObject(symUnmanagedDocumentWriter);
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00042F84 File Offset: 0x00041184
		public void CloseMethod()
		{
			this.writer.CloseMethod();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00042F91 File Offset: 0x00041191
		public void CloseNamespace()
		{
			this.writer.CloseNamespace();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00042F9E File Offset: 0x0004119E
		public void CloseScope(int endOffset)
		{
			this.writer.CloseScope(endOffset);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00042FAC File Offset: 0x000411AC
		public SymDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter;
			this.writer.DefineDocument(url, ref language, ref languageVendor, ref documentType, out symUnmanagedDocumentWriter);
			this.documents.Add(symUnmanagedDocumentWriter);
			return new SymDocumentWriter(symUnmanagedDocumentWriter);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00042FDF File Offset: 0x000411DF
		public void DefineSequencePoints(SymDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			this.writer.DefineSequencePoints(document.Writer, offsets.Length, offsets, lines, columns, endLines, endColumns);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00042FFD File Offset: 0x000411FD
		public void Initialize(object emitter, string filename, bool fFullBuild)
		{
			this.writer.Initialize(emitter, filename, null, fFullBuild);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004300E File Offset: 0x0004120E
		public void SetUserEntryPoint(int methodToken)
		{
			this.writer.SetUserEntryPoint(methodToken);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0004301C File Offset: 0x0004121C
		public void OpenMethod(int methodToken)
		{
			this.writer.OpenMethod(methodToken);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0004302A File Offset: 0x0004122A
		public void OpenNamespace(string name)
		{
			this.writer.OpenNamespace(name);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00043038 File Offset: 0x00041238
		public int OpenScope(int startOffset)
		{
			int num;
			this.writer.OpenScope(startOffset, out num);
			return num;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00043054 File Offset: 0x00041254
		public void UsingNamespace(string fullName)
		{
			this.writer.UsingNamespace(fullName);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00043064 File Offset: 0x00041264
		public void DefineCustomMetadata(string name, byte[] metadata)
		{
			GCHandle gchandle = GCHandle.Alloc(metadata, GCHandleType.Pinned);
			this.writer.SetSymAttribute(0U, name, (uint)metadata.Length, gchandle.AddrOfPinnedObject());
			gchandle.Free();
		}

		// Token: 0x04000A8C RID: 2700
		private static Guid s_symUnmangedWriterIID = new Guid("0b97726e-9e6d-4f05-9a26-424022093caa");

		// Token: 0x04000A8D RID: 2701
		private static Guid s_CorSymWriter_SxS_ClassID = new Guid("108296c1-281e-11d3-bd22-0000f80849bd");

		// Token: 0x04000A8E RID: 2702
		private readonly ISymUnmanagedWriter2 writer;

		// Token: 0x04000A8F RID: 2703
		private readonly Collection<ISymUnmanagedDocumentWriter> documents;
	}
}
