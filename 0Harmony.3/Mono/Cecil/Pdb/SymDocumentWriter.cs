using System;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023C RID: 572
	internal class SymDocumentWriter
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x0003AECF File Offset: 0x000390CF
		public ISymUnmanagedDocumentWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0003AED7 File Offset: 0x000390D7
		public SymDocumentWriter(ISymUnmanagedDocumentWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0003AEE6 File Offset: 0x000390E6
		public void SetSource(byte[] source)
		{
			this.writer.SetSource((uint)source.Length, source);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0003AEF7 File Offset: 0x000390F7
		public void SetCheckSum(Guid hashAlgo, byte[] checkSum)
		{
			this.writer.SetCheckSum(hashAlgo, (uint)checkSum.Length, checkSum);
		}

		// Token: 0x04000A4C RID: 2636
		private readonly ISymUnmanagedDocumentWriter writer;
	}
}
