using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F6 RID: 758
	[Flags]
	internal enum FRAMEDATA_FLAGS : uint
	{
		// Token: 0x04000E9D RID: 3741
		fHasSEH = 1U,
		// Token: 0x04000E9E RID: 3742
		fHasEH = 2U,
		// Token: 0x04000E9F RID: 3743
		fIsFunctionStart = 4U
	}
}
