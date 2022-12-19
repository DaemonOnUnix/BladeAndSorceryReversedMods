using System;

namespace Mono.Cecil
{
	// Token: 0x02000214 RID: 532
	internal interface IGenericContext
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000B87 RID: 2951
		bool IsDefinition { get; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000B88 RID: 2952
		IGenericParameterProvider Type { get; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000B89 RID: 2953
		IGenericParameterProvider Method { get; }
	}
}
