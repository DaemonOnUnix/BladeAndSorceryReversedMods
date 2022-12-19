using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003ED RID: 1005
	internal struct FrameData
	{
		// Token: 0x04000EDF RID: 3807
		internal uint ulRvaStart;

		// Token: 0x04000EE0 RID: 3808
		internal uint cbBlock;

		// Token: 0x04000EE1 RID: 3809
		internal uint cbLocals;

		// Token: 0x04000EE2 RID: 3810
		internal uint cbParams;

		// Token: 0x04000EE3 RID: 3811
		internal uint cbStkMax;

		// Token: 0x04000EE4 RID: 3812
		internal uint frameFunc;

		// Token: 0x04000EE5 RID: 3813
		internal ushort cbProlog;

		// Token: 0x04000EE6 RID: 3814
		internal ushort cbSavedRegs;

		// Token: 0x04000EE7 RID: 3815
		internal uint flags;
	}
}
