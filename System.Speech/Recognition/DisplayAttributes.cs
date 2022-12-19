using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000135 RID: 309
	[Flags]
	public enum DisplayAttributes
	{
		// Token: 0x040005DA RID: 1498
		None = 0,
		// Token: 0x040005DB RID: 1499
		ZeroTrailingSpaces = 2,
		// Token: 0x040005DC RID: 1500
		OneTrailingSpace = 4,
		// Token: 0x040005DD RID: 1501
		TwoTrailingSpaces = 8,
		// Token: 0x040005DE RID: 1502
		ConsumeLeadingSpaces = 16
	}
}
