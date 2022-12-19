using System;

namespace Mono.Cecil
{
	// Token: 0x02000266 RID: 614
	[Flags]
	internal enum MethodDefinitionTreatment
	{
		// Token: 0x040004ED RID: 1261
		None = 0,
		// Token: 0x040004EE RID: 1262
		Abstract = 2,
		// Token: 0x040004EF RID: 1263
		Private = 4,
		// Token: 0x040004F0 RID: 1264
		Public = 8,
		// Token: 0x040004F1 RID: 1265
		Runtime = 16,
		// Token: 0x040004F2 RID: 1266
		InternalCall = 32
	}
}
