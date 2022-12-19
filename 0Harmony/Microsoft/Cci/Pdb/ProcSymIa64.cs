using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003CE RID: 974
	internal struct ProcSymIa64
	{
		// Token: 0x04000E4E RID: 3662
		internal uint parent;

		// Token: 0x04000E4F RID: 3663
		internal uint end;

		// Token: 0x04000E50 RID: 3664
		internal uint next;

		// Token: 0x04000E51 RID: 3665
		internal uint len;

		// Token: 0x04000E52 RID: 3666
		internal uint dbgStart;

		// Token: 0x04000E53 RID: 3667
		internal uint dbgEnd;

		// Token: 0x04000E54 RID: 3668
		internal uint typind;

		// Token: 0x04000E55 RID: 3669
		internal uint off;

		// Token: 0x04000E56 RID: 3670
		internal ushort seg;

		// Token: 0x04000E57 RID: 3671
		internal ushort retReg;

		// Token: 0x04000E58 RID: 3672
		internal byte flags;

		// Token: 0x04000E59 RID: 3673
		internal string name;
	}
}
