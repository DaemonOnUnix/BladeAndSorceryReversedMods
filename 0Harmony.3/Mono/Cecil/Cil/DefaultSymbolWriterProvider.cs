using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FB RID: 507
	internal class DefaultSymbolWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000F99 RID: 3993 RVA: 0x000352A0 File Offset: 0x000334A0
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			ISymbolReader symbolReader = module.SymbolReader;
			if (symbolReader == null)
			{
				throw new InvalidOperationException();
			}
			if (module.Image != null && module.Image.HasDebugTables())
			{
				return null;
			}
			return symbolReader.GetWriterProvider().GetSymbolWriter(module, fileName);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000039F6 File Offset: 0x00001BF6
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
