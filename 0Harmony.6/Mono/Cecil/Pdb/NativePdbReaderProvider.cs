using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000238 RID: 568
	internal sealed class NativePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x060011DF RID: 4575 RVA: 0x0003AD3B File Offset: 0x00038F3B
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbReader(Disposable.Owned<Stream>(File.OpenRead(Mixin.GetPdbFileName(fileName))));
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0003AD5E File Offset: 0x00038F5E
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new NativePdbReader(Disposable.NotOwned<Stream>(symbolStream));
		}
	}
}
