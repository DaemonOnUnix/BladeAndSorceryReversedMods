using System;

namespace Mono.Cecil
{
	// Token: 0x020001FB RID: 507
	[Flags]
	public enum EventAttributes : ushort
	{
		// Token: 0x040002EB RID: 747
		None = 0,
		// Token: 0x040002EC RID: 748
		SpecialName = 512,
		// Token: 0x040002ED RID: 749
		RTSpecialName = 1024
	}
}
