using System;

namespace Mono.Cecil
{
	// Token: 0x02000128 RID: 296
	public interface IMetadataImporterProvider
	{
		// Token: 0x06000861 RID: 2145
		IMetadataImporter GetMetadataImporter(ModuleDefinition module);
	}
}
