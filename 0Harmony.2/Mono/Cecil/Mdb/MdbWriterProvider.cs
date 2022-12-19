using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200031B RID: 795
	internal sealed class MdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001407 RID: 5127 RVA: 0x00041005 File Offset: 0x0003F205
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbWriter(module, fileName);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004101A File Offset: 0x0003F21A
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
