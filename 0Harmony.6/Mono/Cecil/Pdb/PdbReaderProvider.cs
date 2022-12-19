using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000239 RID: 569
	internal sealed class PdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0003AD78 File Offset: 0x00038F78
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

		// Token: 0x060011E3 RID: 4579 RVA: 0x0003ADD9 File Offset: 0x00038FD9
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
