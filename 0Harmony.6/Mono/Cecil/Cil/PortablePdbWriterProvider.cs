using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D3 RID: 467
	internal sealed class PortablePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06000ED0 RID: 3792 RVA: 0x00033A0C File Offset: 0x00031C0C
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenWrite(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolWriter(module, Disposable.Owned<Stream>(fileStream));
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00033A3E File Offset: 0x00031C3E
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolWriter(module, Disposable.NotOwned<Stream>(symbolStream));
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00033A5C File Offset: 0x00031C5C
		private ISymbolWriter GetSymbolWriter(ModuleDefinition module, Disposable<Stream> stream)
		{
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, this);
			ImageWriter imageWriter = ImageWriter.CreateDebugWriter(module, metadataBuilder, stream);
			return new PortablePdbWriter(metadataBuilder, module, imageWriter);
		}
	}
}
