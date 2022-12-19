using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000221 RID: 545
	internal sealed class MdbReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x00038C72 File Offset: 0x00036E72
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			Mixin.CheckModule(module);
			Mixin.CheckFileName(fileName);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(Mixin.GetMdbFileName(fileName)));
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00038C91 File Offset: 0x00036E91
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			Mixin.CheckModule(module);
			Mixin.CheckStream(symbolStream);
			return new MdbReader(module, MonoSymbolFile.ReadSymbolFile(symbolStream));
		}
	}
}
