using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011E RID: 286
	internal interface IGenericInstance : IMetadataTokenProvider
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600083B RID: 2107
		bool HasGenericArguments { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600083C RID: 2108
		Collection<TypeReference> GenericArguments { get; }
	}
}
