using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000225 RID: 549
	internal sealed class MdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x000390B9 File Offset: 0x000372B9
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbWriter(module.Mvid, fileName);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000390D3 File Offset: 0x000372D3
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
