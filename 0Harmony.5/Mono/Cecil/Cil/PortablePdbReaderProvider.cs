using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C5 RID: 709
	internal sealed class PortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x0003B4D0 File Offset: 0x000396D0
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenRead(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolReader(module, Disposable.Owned<Stream>(fileStream), fileStream.Name);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0003B508 File Offset: 0x00039708
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolReader(module, Disposable.NotOwned<Stream>(symbolStream), symbolStream.GetFileName());
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0003B529 File Offset: 0x00039729
		private ISymbolReader GetSymbolReader(ModuleDefinition module, Disposable<Stream> symbolStream, string fileName)
		{
			return new PortablePdbReader(ImageReader.ReadPortablePdb(symbolStream, fileName), module);
		}
	}
}
