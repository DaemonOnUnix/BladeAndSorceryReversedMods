using System;

namespace Mono.Cecil
{
	// Token: 0x0200021D RID: 541
	public interface IReflectionImporterProvider
	{
		// Token: 0x06000BAB RID: 2987
		IReflectionImporter GetReflectionImporter(ModuleDefinition module);
	}
}
