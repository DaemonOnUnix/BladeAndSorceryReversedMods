using System;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000332 RID: 818
	internal class SymDocumentWriter
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x00042E17 File Offset: 0x00041017
		public ISymUnmanagedDocumentWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00042E1F File Offset: 0x0004101F
		public SymDocumentWriter(ISymUnmanagedDocumentWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00042E2E File Offset: 0x0004102E
		public void SetSource(byte[] source)
		{
			this.writer.SetSource((uint)source.Length, source);
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00042E3F File Offset: 0x0004103F
		public void SetCheckSum(Guid hashAlgo, byte[] checkSum)
		{
			this.writer.SetCheckSum(hashAlgo, (uint)checkSum.Length, checkSum);
		}

		// Token: 0x04000A8B RID: 2699
		private readonly ISymUnmanagedDocumentWriter writer;
	}
}
