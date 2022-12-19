using System;

namespace Mono.Cecil
{
	// Token: 0x02000125 RID: 293
	public interface IMetadataScope : IMetadataTokenProvider
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000851 RID: 2129
		MetadataScopeType MetadataScopeType { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000852 RID: 2130
		// (set) Token: 0x06000853 RID: 2131
		string Name { get; set; }
	}
}
