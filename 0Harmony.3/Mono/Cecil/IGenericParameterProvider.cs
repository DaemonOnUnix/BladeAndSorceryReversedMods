using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011F RID: 287
	public interface IGenericParameterProvider : IMetadataTokenProvider
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600083D RID: 2109
		bool HasGenericParameters { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600083E RID: 2110
		bool IsDefinition { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600083F RID: 2111
		ModuleDefinition Module { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000840 RID: 2112
		Collection<GenericParameter> GenericParameters { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000841 RID: 2113
		GenericParameterType GenericParameterType { get; }
	}
}
