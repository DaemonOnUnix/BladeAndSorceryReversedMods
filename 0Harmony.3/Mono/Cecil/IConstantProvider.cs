using System;

namespace Mono.Cecil
{
	// Token: 0x0200011C RID: 284
	internal interface IConstantProvider : IMetadataTokenProvider
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000835 RID: 2101
		// (set) Token: 0x06000836 RID: 2102
		bool HasConstant { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000837 RID: 2103
		// (set) Token: 0x06000838 RID: 2104
		object Constant { get; set; }
	}
}
