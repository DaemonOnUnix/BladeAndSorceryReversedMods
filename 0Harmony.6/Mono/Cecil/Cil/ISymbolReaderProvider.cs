using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F3 RID: 499
	public interface ISymbolReaderProvider
	{
		// Token: 0x06000F85 RID: 3973
		ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName);

		// Token: 0x06000F86 RID: 3974
		ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream);
	}
}
