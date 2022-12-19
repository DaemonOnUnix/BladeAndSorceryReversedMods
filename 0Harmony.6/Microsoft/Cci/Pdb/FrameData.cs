using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002F7 RID: 759
	internal struct FrameData
	{
		// Token: 0x04000EA0 RID: 3744
		internal uint ulRvaStart;

		// Token: 0x04000EA1 RID: 3745
		internal uint cbBlock;

		// Token: 0x04000EA2 RID: 3746
		internal uint cbLocals;

		// Token: 0x04000EA3 RID: 3747
		internal uint cbParams;

		// Token: 0x04000EA4 RID: 3748
		internal uint cbStkMax;

		// Token: 0x04000EA5 RID: 3749
		internal uint frameFunc;

		// Token: 0x04000EA6 RID: 3750
		internal ushort cbProlog;

		// Token: 0x04000EA7 RID: 3751
		internal ushort cbSavedRegs;

		// Token: 0x04000EA8 RID: 3752
		internal uint flags;
	}
}
