using System;
using System.IO;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002CB RID: 715
	internal sealed class EmbeddedPortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001248 RID: 4680 RVA: 0x0003BC28 File Offset: 0x00039E28
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			MemoryStream memoryStream = new MemoryStream();
			PortablePdbWriter portablePdbWriter = (PortablePdbWriter)new PortablePdbWriterProvider().GetSymbolWriter(module, memoryStream);
			return new EmbeddedPortablePdbWriter(memoryStream, portablePdbWriter);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00003A32 File Offset: 0x00001C32
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotSupportedException();
		}
	}
}
