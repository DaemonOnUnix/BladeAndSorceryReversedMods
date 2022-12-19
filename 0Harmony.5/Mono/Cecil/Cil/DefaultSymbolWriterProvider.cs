using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002F1 RID: 753
	internal class DefaultSymbolWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001309 RID: 4873 RVA: 0x0003D1EC File Offset: 0x0003B3EC
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

		// Token: 0x0600130A RID: 4874 RVA: 0x00003A32 File Offset: 0x00001C32
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
