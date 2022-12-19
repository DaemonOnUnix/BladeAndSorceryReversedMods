using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011D RID: 285
	public interface ICustomAttributeProvider : IMetadataTokenProvider
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000839 RID: 2105
		Collection<CustomAttribute> CustomAttributes { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600083A RID: 2106
		bool HasCustomAttributes { get; }
	}
}
