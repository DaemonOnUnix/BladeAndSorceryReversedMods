using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E9 RID: 745
	public interface ISymbolReaderProvider
	{
		// Token: 0x060012F5 RID: 4853
		ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName);

		// Token: 0x060012F6 RID: 4854
		ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream);
	}
}
