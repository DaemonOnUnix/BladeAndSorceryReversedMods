using System;

namespace Mono.Cecil
{
	// Token: 0x0200021B RID: 539
	public interface IMetadataImporterProvider
	{
		// Token: 0x06000BA6 RID: 2982
		IMetadataImporter GetMetadataImporter(ModuleDefinition module);
	}
}
