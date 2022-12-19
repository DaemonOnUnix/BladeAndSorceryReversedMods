using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C9 RID: 713
	internal sealed class PortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001239 RID: 4665 RVA: 0x0003B834 File Offset: 0x00039A34
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenWrite(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolWriter(module, Disposable.Owned<Stream>(fileStream));
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0003B866 File Offset: 0x00039A66
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolWriter(module, Disposable.NotOwned<Stream>(symbolStream));
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0003B884 File Offset: 0x00039A84
		private ISymbolWriter GetSymbolWriter(ModuleDefinition module, Disposable<Stream> stream)
		{
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, this);
			ImageWriter imageWriter = ImageWriter.CreateDebugWriter(module, metadataBuilder, stream);
			return new PortablePdbWriter(metadataBuilder, module, imageWriter);
		}
	}
}
