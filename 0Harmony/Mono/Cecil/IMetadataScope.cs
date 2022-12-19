using System;

namespace Mono.Cecil
{
	// Token: 0x02000218 RID: 536
	public interface IMetadataScope : IMetadataTokenProvider
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000B96 RID: 2966
		MetadataScopeType MetadataScopeType { get; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000B97 RID: 2967
		// (set) Token: 0x06000B98 RID: 2968
		string Name { get; set; }
	}
}
