using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003CD RID: 973
	internal struct ProcSymMips
	{
		// Token: 0x04000E3E RID: 3646
		internal uint parent;

		// Token: 0x04000E3F RID: 3647
		internal uint end;

		// Token: 0x04000E40 RID: 3648
		internal uint next;

		// Token: 0x04000E41 RID: 3649
		internal uint len;

		// Token: 0x04000E42 RID: 3650
		internal uint dbgStart;

		// Token: 0x04000E43 RID: 3651
		internal uint dbgEnd;

		// Token: 0x04000E44 RID: 3652
		internal uint regSave;

		// Token: 0x04000E45 RID: 3653
		internal uint fpSave;

		// Token: 0x04000E46 RID: 3654
		internal uint intOff;

		// Token: 0x04000E47 RID: 3655
		internal uint fpOff;

		// Token: 0x04000E48 RID: 3656
		internal uint typind;

		// Token: 0x04000E49 RID: 3657
		internal uint off;

		// Token: 0x04000E4A RID: 3658
		internal ushort seg;

		// Token: 0x04000E4B RID: 3659
		internal byte retReg;

		// Token: 0x04000E4C RID: 3660
		internal byte frameReg;

		// Token: 0x04000E4D RID: 3661
		internal string name;
	}
}
