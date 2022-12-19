using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003D4 RID: 980
	internal struct FrameProcSym
	{
		// Token: 0x04000E72 RID: 3698
		internal uint cbFrame;

		// Token: 0x04000E73 RID: 3699
		internal uint cbPad;

		// Token: 0x04000E74 RID: 3700
		internal uint offPad;

		// Token: 0x04000E75 RID: 3701
		internal uint cbSaveRegs;

		// Token: 0x04000E76 RID: 3702
		internal uint offExHdlr;

		// Token: 0x04000E77 RID: 3703
		internal ushort secExHdlr;

		// Token: 0x04000E78 RID: 3704
		internal uint flags;
	}
}
