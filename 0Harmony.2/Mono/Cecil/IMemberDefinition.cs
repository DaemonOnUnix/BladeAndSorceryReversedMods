using System;

namespace Mono.Cecil
{
	// Token: 0x02000216 RID: 534
	public interface IMemberDefinition : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000B8D RID: 2957
		// (set) Token: 0x06000B8E RID: 2958
		string Name { get; set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000B8F RID: 2959
		string FullName { get; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000B90 RID: 2960
		// (set) Token: 0x06000B91 RID: 2961
		bool IsSpecialName { get; set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000B92 RID: 2962
		// (set) Token: 0x06000B93 RID: 2963
		bool IsRuntimeSpecialName { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000B94 RID: 2964
		// (set) Token: 0x06000B95 RID: 2965
		TypeDefinition DeclaringType { get; set; }
	}
}
