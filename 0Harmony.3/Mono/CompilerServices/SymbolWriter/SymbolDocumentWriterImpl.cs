using System;
using System.Diagnostics.SymbolStore;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021E RID: 542
	internal class SymbolDocumentWriterImpl : ISymbolDocumentWriter, ISourceFile, ICompileUnit
	{
		// Token: 0x06001079 RID: 4217 RVA: 0x00038C06 File Offset: 0x00036E06
		public SymbolDocumentWriterImpl(CompileUnitEntry comp_unit)
		{
			this.comp_unit = comp_unit;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00012279 File Offset: 0x00010479
		public void SetCheckSum(Guid algorithmId, byte[] checkSum)
		{
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00012279 File Offset: 0x00010479
		public void SetSource(byte[] source)
		{
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x00038C15 File Offset: 0x00036E15
		SourceFileEntry ISourceFile.Entry
		{
			get
			{
				return this.comp_unit.SourceFile;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x00038C22 File Offset: 0x00036E22
		public CompileUnitEntry Entry
		{
			get
			{
				return this.comp_unit;
			}
		}

		// Token: 0x04000A15 RID: 2581
		private CompileUnitEntry comp_unit;
	}
}
