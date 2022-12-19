using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000317 RID: 791
	internal sealed class MdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x060013F3 RID: 5107 RVA: 0x00040BBE File Offset: 0x0003EDBE
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(Mixin.GetMdbFileName(fileName)));
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00040BDD File Offset: 0x0003EDDD
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(symbolStream));
		}
	}
}
