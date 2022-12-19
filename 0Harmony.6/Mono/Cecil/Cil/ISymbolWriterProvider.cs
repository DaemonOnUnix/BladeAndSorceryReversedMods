using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FA RID: 506
	public interface ISymbolWriterProvider
	{
		// Token: 0x06000F97 RID: 3991
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName);

		// Token: 0x06000F98 RID: 3992
		ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream);
	}
}
