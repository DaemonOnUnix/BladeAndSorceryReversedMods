using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000211 RID: 529
	internal interface IGenericInstance : IMetadataTokenProvider
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000B80 RID: 2944
		bool HasGenericArguments { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000B81 RID: 2945
		Collection<TypeReference> GenericArguments { get; }
	}
}
