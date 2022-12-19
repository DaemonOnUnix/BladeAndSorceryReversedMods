using System;

namespace Mono.Cecil
{
	// Token: 0x02000109 RID: 265
	[Flags]
	public enum EventAttributes : ushort
	{
		// Token: 0x040002B9 RID: 697
		None = 0,
		// Token: 0x040002BA RID: 698
		SpecialName = 512,
		// Token: 0x040002BB RID: 699
		RTSpecialName = 1024
	}
}
