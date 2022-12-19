using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002DE RID: 734
	internal struct FrameProcSym
	{
		// Token: 0x04000E33 RID: 3635
		internal uint cbFrame;

		// Token: 0x04000E34 RID: 3636
		internal uint cbPad;

		// Token: 0x04000E35 RID: 3637
		internal uint offPad;

		// Token: 0x04000E36 RID: 3638
		internal uint cbSaveRegs;

		// Token: 0x04000E37 RID: 3639
		internal uint offExHdlr;

		// Token: 0x04000E38 RID: 3640
		internal ushort secExHdlr;

		// Token: 0x04000E39 RID: 3641
		internal uint flags;
	}
}
