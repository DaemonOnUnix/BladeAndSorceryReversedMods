using System;
using System.IO;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000330 RID: 816
	internal sealed class NativePdbWriterProvider : ISymbolWriterProvider
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x00042D56 File Offset: 0x00040F56
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new NativePdbWriter(module, NativePdbWriterProvider.CreateWriter(module, Mixin.GetPdbFileName(fileName)));
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00042D76 File Offset: 0x00040F76
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

		// Token: 0x06001556 RID: 5462 RVA: 0x0004101A File Offset: 0x0003F21A
		public ISymbolWriter GetSymbolWriter(ModuleDefinition module, Stream symbolStream)
		{
			throw new NotImplementedException();
		}
	}
}
