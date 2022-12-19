using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000127 RID: 295
	public interface IMethodSignature : IMetadataTokenProvider
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000856 RID: 2134
		// (set) Token: 0x06000857 RID: 2135
		bool HasThis { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000858 RID: 2136
		// (set) Token: 0x06000859 RID: 2137
		bool ExplicitThis { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600085A RID: 2138
		// (set) Token: 0x0600085B RID: 2139
		MethodCallingConvention CallingConvention { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600085C RID: 2140
		bool HasParameters { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600085D RID: 2141
		Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600085E RID: 2142
		// (set) Token: 0x0600085F RID: 2143
		TypeReference ReturnType { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000860 RID: 2144
		MethodReturnType MethodReturnType { get; }
	}
}
