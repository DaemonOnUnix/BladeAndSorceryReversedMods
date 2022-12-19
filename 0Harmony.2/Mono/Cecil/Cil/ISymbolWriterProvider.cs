using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002F0 RID: 752
	public interface ISymbolWriterProvider
	{
		// Token: 0x06001307 RID: 4871
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName);

		// Token: 0x06001308 RID: 4872
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream);
	}
}
