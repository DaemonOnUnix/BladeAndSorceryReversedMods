using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CF RID: 463
	internal sealed class PortablePdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x000336DC File Offset: 0x000318DC
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			FileStream fileStream = File.OpenRead(Mixin.GetPdbFileName(fileName));
			return this.GetSymbolReader(module, Disposable.Owned<Stream>(fileStream), fileStream.Name);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00033714 File Offset: 0x00031914
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return this.GetSymbolReader(module, Disposable.NotOwned<Stream>(symbolStream), symbolStream.GetFileName());
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00033735 File Offset: 0x00031935
		private ISymbolReader GetSymbolReader(ModuleDefinition module, Disposable<Stream> symbolStream, string fileName)
		{
			return new PortablePdbReader(ImageReader.ReadPortablePdb(symbolStream, fileName), module);
		}
	}
}
