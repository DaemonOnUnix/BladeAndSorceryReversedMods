using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000331 RID: 817
	internal sealed class PdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x00042D99 File Offset: 0x00040F99
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

		// Token: 0x06001559 RID: 5465 RVA: 0x00042DC8 File Offset: 0x00040FC8
		private static bool HasPortablePdbSymbols(ModuleDefinition module)
		{
			return module.symbol_reader != null && module.symbol_reader is PortablePdbReader;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x00042DE2 File Offset: 0x00040FE2
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
