using System;

namespace Mono.Cecil
{
	// Token: 0x02000264 RID: 612
	[Flags]
	internal enum TypeDefinitionTreatment
	{
		// Token: 0x040004DC RID: 1244
		None = 0,
		// Token: 0x040004DD RID: 1245
		KindMask = 15,
		// Token: 0x040004DE RID: 1246
		NormalType = 1,
		// Token: 0x040004DF RID: 1247
		NormalAttribute = 2,
		// Token: 0x040004E0 RID: 1248
		UnmangleWindowsRuntimeName = 3,
		// Token: 0x040004E1 RID: 1249
		PrefixWindowsRuntimeName = 4,
		// Token: 0x040004E2 RID: 1250
		RedirectToClrType = 5,
		// Token: 0x040004E3 RID: 1251
		RedirectToClrAttribute = 6,
		// Token: 0x040004E4 RID: 1252
		RedirectImplementedMethods = 7,
		// Token: 0x040004E5 RID: 1253
		Abstract = 16,
		// Token: 0x040004E6 RID: 1254
		Internal = 32
	}
}
