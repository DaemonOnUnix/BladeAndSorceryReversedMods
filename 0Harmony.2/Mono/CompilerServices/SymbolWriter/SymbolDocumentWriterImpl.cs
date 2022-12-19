using System;
using System.Diagnostics.SymbolStore;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000314 RID: 788
	internal class SymbolDocumentWriterImpl : ISymbolDocumentWriter, ISourceFile, ICompileUnit
	{
		// Token: 0x060013E9 RID: 5097 RVA: 0x00040B52 File Offset: 0x0003ED52
		public SymbolDocumentWriterImpl(CompileUnitEntry comp_unit)
		{
			this.comp_unit = comp_unit;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00018105 File Offset: 0x00016305
		public void SetCheckSum(Guid algorithmId, byte[] checkSum)
		{
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00018105 File Offset: 0x00016305
		public void SetSource(byte[] source)
		{
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00040B61 File Offset: 0x0003ED61
		SourceFileEntry ISourceFile.Entry
		{
			get
			{
				return this.comp_unit.SourceFile;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x00040B6E File Offset: 0x0003ED6E
		public CompileUnitEntry Entry
		{
			get
			{
				return this.comp_unit;
			}
		}

		// Token: 0x04000A54 RID: 2644
		private CompileUnitEntry comp_unit;
	}
}
