using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D5 RID: 469
	internal sealed class EmbeddedPortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000EDF RID: 3807 RVA: 0x00033DD4 File Offset: 0x00031FD4
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			MemoryStream memoryStream = new MemoryStream();
			PortablePdbWriter portablePdbWriter = (PortablePdbWriter)new PortablePdbWriterProvider().GetSymbolWriter(module, memoryStream);
			return new EmbeddedPortablePdbWriter(memoryStream, portablePdbWriter);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000039F6 File Offset: 0x00001BF6
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
