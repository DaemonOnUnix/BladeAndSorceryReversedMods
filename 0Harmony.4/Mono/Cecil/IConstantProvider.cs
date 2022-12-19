using System;

namespace Mono.Cecil
{
	// Token: 0x0200020F RID: 527
	internal interface IConstantProvider : IMetadataTokenProvider
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000B7A RID: 2938
		// (set) Token: 0x06000B7B RID: 2939
		bool HasConstant { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000B7C RID: 2940
		// (set) Token: 0x06000B7D RID: 2941
		object Constant { get; set; }
	}
}
