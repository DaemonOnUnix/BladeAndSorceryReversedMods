using System;

namespace Mono.Cecil
{
	// Token: 0x02000172 RID: 370
	[Flags]
	internal enum MethodDefinitionTreatment
	{
		// Token: 0x040004B7 RID: 1207
		None = 0,
		// Token: 0x040004B8 RID: 1208
		Dispose = 1,
		// Token: 0x040004B9 RID: 1209
		Abstract = 2,
		// Token: 0x040004BA RID: 1210
		Private = 4,
		// Token: 0x040004BB RID: 1211
		Public = 8,
		// Token: 0x040004BC RID: 1212
		Runtime = 16,
		// Token: 0x040004BD RID: 1213
		InternalCall = 32
	}
}
