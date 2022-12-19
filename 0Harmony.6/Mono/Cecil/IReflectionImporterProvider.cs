using System;

namespace Mono.Cecil
{
	// Token: 0x0200012A RID: 298
	public interface IReflectionImporterProvider
	{
		// Token: 0x06000866 RID: 2150
		IReflectionImporter GetReflectionImporter(ModuleDefinition module);
	}
}
