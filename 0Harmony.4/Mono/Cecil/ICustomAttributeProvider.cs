using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000210 RID: 528
	public interface ICustomAttributeProvider : IMetadataTokenProvider
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000B7E RID: 2942
		Collection<CustomAttribute> CustomAttributes { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000B7F RID: 2943
		bool HasCustomAttributes { get; }
	}
}
