using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200032E RID: 814
	internal sealed class NativePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x00042C83 File Offset: 0x00040E83
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbReader(Disposable.Owned<Stream>(File.OpenRead(Mixin.GetPdbFileName(fileName))));
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00042CA6 File Offset: 0x00040EA6
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new NativePdbReader(Disposable.NotOwned<Stream>(symbolStream));
		}
	}
}
