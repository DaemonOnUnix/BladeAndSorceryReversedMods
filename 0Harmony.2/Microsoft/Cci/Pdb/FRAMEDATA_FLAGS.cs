using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003EC RID: 1004
	[Flags]
	internal enum FRAMEDATA_FLAGS : uint
	{
		// Token: 0x04000EDC RID: 3804
		fHasSEH = 1U,
		// Token: 0x04000EDD RID: 3805
		fHasEH = 2U,
		// Token: 0x04000EDE RID: 3806
		fIsFunctionStart = 4U
	}
}
