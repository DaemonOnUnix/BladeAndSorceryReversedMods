using System;

namespace MonoMod.Utils
{
	// Token: 0x02000350 RID: 848
	[Flags]
	internal enum Platform
	{
		// Token: 0x04000FD6 RID: 4054
		OS = 1,
		// Token: 0x04000FD7 RID: 4055
		Bits64 = 2,
		// Token: 0x04000FD8 RID: 4056
		NT = 4,
		// Token: 0x04000FD9 RID: 4057
		Unix = 8,
		// Token: 0x04000FDA RID: 4058
		ARM = 65536,
		// Token: 0x04000FDB RID: 4059
		Unknown = 17,
		// Token: 0x04000FDC RID: 4060
		Windows = 37,
		// Token: 0x04000FDD RID: 4061
		MacOS = 73,
		// Token: 0x04000FDE RID: 4062
		Linux = 137,
		// Token: 0x04000FDF RID: 4063
		Android = 393,
		// Token: 0x04000FE0 RID: 4064
		iOS = 585
	}
}
