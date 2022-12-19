using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003C0 RID: 960
	internal struct ManProcSym
	{
		// Token: 0x04000DEA RID: 3562
		internal uint parent;

		// Token: 0x04000DEB RID: 3563
		internal uint end;

		// Token: 0x04000DEC RID: 3564
		internal uint next;

		// Token: 0x04000DED RID: 3565
		internal uint len;

		// Token: 0x04000DEE RID: 3566
		internal uint dbgStart;

		// Token: 0x04000DEF RID: 3567
		internal uint dbgEnd;

		// Token: 0x04000DF0 RID: 3568
		internal uint token;

		// Token: 0x04000DF1 RID: 3569
		internal uint off;

		// Token: 0x04000DF2 RID: 3570
		internal ushort seg;

		// Token: 0x04000DF3 RID: 3571
		internal byte flags;

		// Token: 0x04000DF4 RID: 3572
		internal ushort retReg;

		// Token: 0x04000DF5 RID: 3573
		internal string name;
	}
}
