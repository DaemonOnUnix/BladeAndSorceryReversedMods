using System;

namespace Mono.Cecil
{
	// Token: 0x02000256 RID: 598
	[Flags]
	public enum PropertyAttributes : ushort
	{
		// Token: 0x040004A5 RID: 1189
		None = 0,
		// Token: 0x040004A6 RID: 1190
		SpecialName = 512,
		// Token: 0x040004A7 RID: 1191
		RTSpecialName = 1024,
		// Token: 0x040004A8 RID: 1192
		HasDefault = 4096,
		// Token: 0x040004A9 RID: 1193
		Unused = 59903
	}
}
