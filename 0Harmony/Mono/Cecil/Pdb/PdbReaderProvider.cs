using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200032F RID: 815
	internal sealed class PdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001551 RID: 5457 RVA: 0x00042CC0 File Offset: 0x00040EC0
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			if (module.HasDebugHeader && module.GetDebugHeader().GetEmbeddedPortablePdbEntry() != null)
			{
				return new EmbeddedPortablePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			Mixin.CheckFileName(fileName);
			if (!Mixin.IsPortablePdb(Mixin.GetPdbFileName(fileName)))
			{
				return new NativePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			return new PortablePdbReaderProvider().GetSymbolReader(module, fileName);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00042D21 File Offset: 0x00040F21
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			Mixin.CheckReadSeek(symbolStream);
			if (!Mixin.IsPortablePdb(symbolStream))
			{
				return new NativePdbReaderProvider().GetSymbolReader(module, symbolStream);
			}
			return new PortablePdbReaderProvider().GetSymbolReader(module, symbolStream);
		}
	}
}
