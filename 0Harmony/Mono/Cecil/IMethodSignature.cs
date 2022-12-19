using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200021A RID: 538
	public interface IMethodSignature : IMetadataTokenProvider
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000B9B RID: 2971
		// (set) Token: 0x06000B9C RID: 2972
		bool HasThis { get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000B9D RID: 2973
		// (set) Token: 0x06000B9E RID: 2974
		bool ExplicitThis { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000B9F RID: 2975
		// (set) Token: 0x06000BA0 RID: 2976
		MethodCallingConvention CallingConvention { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000BA1 RID: 2977
		bool HasParameters { get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000BA2 RID: 2978
		Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000BA3 RID: 2979
		// (set) Token: 0x06000BA4 RID: 2980
		TypeReference ReturnType { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000BA5 RID: 2981
		MethodReturnType MethodReturnType { get; }
	}
}
