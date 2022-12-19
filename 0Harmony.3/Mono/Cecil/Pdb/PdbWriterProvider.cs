using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023B RID: 571
	internal sealed class PdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x060011E9 RID: 4585 RVA: 0x0003AE51 File Offset: 0x00039051
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			if (PdbWriterProvider.HasPortablePdbSymbols(module))
			{
				return new PortablePdbWriterProvider().GetSymbolWriter(module, fileName);
			}
			return new NativePdbWriterProvider().GetSymbolWriter(module, fileName);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0003AE80 File Offset: 0x00039080
		private static bool HasPortablePdbSymbols(ModuleDefinition module)
		{
			return module.symbol_reader != null && module.symbol_reader is PortablePdbReader;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0003AE9A File Offset: 0x0003909A
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			Mixin.CheckReadSeek(symbolStream);
			if (PdbWriterProvider.HasPortablePdbSymbols(module))
			{
				return new PortablePdbWriterProvider().GetSymbolWriter(module, symbolStream);
			}
			return new NativePdbWriterProvider().GetSymbolWriter(module, symbolStream);
		}
	}
}
