using System;

namespace MonoMod.Utils
{
	// Token: 0x0200044B RID: 1099
	[Flags]
	internal enum Platform
	{
		// Token: 0x0400101C RID: 4124
		OS = 1,
		// Token: 0x0400101D RID: 4125
		Bits64 = 2,
		// Token: 0x0400101E RID: 4126
		NT = 4,
		// Token: 0x0400101F RID: 4127
		Unix = 8,
		// Token: 0x04001020 RID: 4128
		ARM = 65536,
		// Token: 0x04001021 RID: 4129
		Wine = 131072,
		// Token: 0x04001022 RID: 4130
		Unknown = 17,
		// Token: 0x04001023 RID: 4131
		Windows = 37,
		// Token: 0x04001024 RID: 4132
		MacOS = 73,
		// Token: 0x04001025 RID: 4133
		Linux = 137,
		// Token: 0x04001026 RID: 4134
		Android = 393,
		// Token: 0x04001027 RID: 4135
		iOS = 585
	}
}
