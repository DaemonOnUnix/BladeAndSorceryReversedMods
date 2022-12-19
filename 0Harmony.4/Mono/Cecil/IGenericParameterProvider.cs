using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000212 RID: 530
	public interface IGenericParameterProvider : IMetadataTokenProvider
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000B82 RID: 2946
		bool HasGenericParameters { get; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000B83 RID: 2947
		bool IsDefinition { get; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000B84 RID: 2948
		ModuleDefinition Module { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000B85 RID: 2949
		Collection<GenericParameter> GenericParameters { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000B86 RID: 2950
		GenericParameterType GenericParameterType { get; }
	}
}
