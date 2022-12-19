using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E4 RID: 484
	public enum ImportTargetKind : byte
	{
		// Token: 0x0400092A RID: 2346
		ImportNamespace = 1,
		// Token: 0x0400092B RID: 2347
		ImportNamespaceInAssembly,
		// Token: 0x0400092C RID: 2348
		ImportType,
		// Token: 0x0400092D RID: 2349
		ImportXmlNamespaceWithAlias,
		// Token: 0x0400092E RID: 2350
		ImportAlias,
		// Token: 0x0400092F RID: 2351
		DefineAssemblyAlias,
		// Token: 0x04000930 RID: 2352
		DefineNamespaceAlias,
		// Token: 0x04000931 RID: 2353
		DefineNamespaceInAssemblyAlias,
		// Token: 0x04000932 RID: 2354
		DefineTypeAlias
	}
}
