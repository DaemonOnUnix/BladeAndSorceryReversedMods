using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000054 RID: 84
	[Flags]
	public enum DisplayAttributes
	{
		// Token: 0x04000324 RID: 804
		None = 0,
		// Token: 0x04000325 RID: 805
		ZeroTrailingSpaces = 2,
		// Token: 0x04000326 RID: 806
		OneTrailingSpace = 4,
		// Token: 0x04000327 RID: 807
		TwoTrailingSpaces = 8,
		// Token: 0x04000328 RID: 808
		ConsumeLeadingSpaces = 16
	}
}
