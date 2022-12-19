using System;

namespace Mono.Cecil
{
	// Token: 0x02000122 RID: 290
	internal interface IMarshalInfoProvider : IMetadataTokenProvider
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000845 RID: 2117
		bool HasMarshalInfo { get; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000846 RID: 2118
		// (set) Token: 0x06000847 RID: 2119
		MarshalInfo MarshalInfo { get; set; }
	}
}
