using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002DA RID: 730
	public enum ImportTargetKind : byte
	{
		// Token: 0x04000966 RID: 2406
		ImportNamespace = 1,
		// Token: 0x04000967 RID: 2407
		ImportNamespaceInAssembly,
		// Token: 0x04000968 RID: 2408
		ImportType,
		// Token: 0x04000969 RID: 2409
		ImportXmlNamespaceWithAlias,
		// Token: 0x0400096A RID: 2410
		ImportAlias,
		// Token: 0x0400096B RID: 2411
		DefineAssemblyAlias,
		// Token: 0x0400096C RID: 2412
		DefineNamespaceAlias,
		// Token: 0x0400096D RID: 2413
		DefineNamespaceInAssemblyAlias,
		// Token: 0x0400096E RID: 2414
		DefineTypeAlias
	}
}
