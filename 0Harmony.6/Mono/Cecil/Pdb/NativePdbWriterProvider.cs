using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023A RID: 570
	internal sealed class NativePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x0003AE0E File Offset: 0x0003900E
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbWriter(module, NativePdbWriterProvider.CreateWriter(module, Mixin.GetPdbFileName(fileName)));
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0003AE2E File Offset: 0x0003902E
		private static SymWriter CreateWriter(ModuleDefinition module, string pdb)
		{
			SymWriter symWriter = new SymWriter();
			if (File.Exists(pdb))
			{
				File.Delete(pdb);
			}
			symWriter.Initialize(new ModuleMetadata(module), pdb, true);
			return symWriter;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x000390D3 File Offset: 0x000372D3
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
