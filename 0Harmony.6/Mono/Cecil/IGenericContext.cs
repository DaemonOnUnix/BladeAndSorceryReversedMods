using System;

namespace Mono.Cecil
{
	// Token: 0x02000121 RID: 289
	internal interface IGenericContext
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000842 RID: 2114
		bool IsDefinition { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000843 RID: 2115
		IGenericParameterProvider Type { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000844 RID: 2116
		IGenericParameterProvider Method { get; }
	}
}
