using System;

namespace Mono.Cecil
{
	// Token: 0x02000170 RID: 368
	[Flags]
	internal enum TypeDefinitionTreatment
	{
		// Token: 0x040004A7 RID: 1191
		None = 0,
		// Token: 0x040004A8 RID: 1192
		KindMask = 15,
		// Token: 0x040004A9 RID: 1193
		NormalType = 1,
		// Token: 0x040004AA RID: 1194
		NormalAttribute = 2,
		// Token: 0x040004AB RID: 1195
		UnmangleWindowsRuntimeName = 3,
		// Token: 0x040004AC RID: 1196
		PrefixWindowsRuntimeName = 4,
		// Token: 0x040004AD RID: 1197
		RedirectToClrType = 5,
		// Token: 0x040004AE RID: 1198
		RedirectToClrAttribute = 6,
		// Token: 0x040004AF RID: 1199
		Abstract = 16,
		// Token: 0x040004B0 RID: 1200
		Internal = 32
	}
}
