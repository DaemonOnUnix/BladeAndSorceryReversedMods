using System;

namespace Mono.Cecil
{
	// Token: 0x02000215 RID: 533
	internal interface IMarshalInfoProvider : IMetadataTokenProvider
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000B8A RID: 2954
		bool HasMarshalInfo { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000B8B RID: 2955
		// (set) Token: 0x06000B8C RID: 2956
		MarshalInfo MarshalInfo { get; set; }
	}
}
