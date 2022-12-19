using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CA RID: 714
	internal struct ManProcSym
	{
		// Token: 0x04000DAB RID: 3499
		internal uint parent;

		// Token: 0x04000DAC RID: 3500
		internal uint end;

		// Token: 0x04000DAD RID: 3501
		internal uint next;

		// Token: 0x04000DAE RID: 3502
		internal uint len;

		// Token: 0x04000DAF RID: 3503
		internal uint dbgStart;

		// Token: 0x04000DB0 RID: 3504
		internal uint dbgEnd;

		// Token: 0x04000DB1 RID: 3505
		internal uint token;

		// Token: 0x04000DB2 RID: 3506
		internal uint off;

		// Token: 0x04000DB3 RID: 3507
		internal ushort seg;

		// Token: 0x04000DB4 RID: 3508
		internal byte flags;

		// Token: 0x04000DB5 RID: 3509
		internal ushort retReg;

		// Token: 0x04000DB6 RID: 3510
		internal string name;
	}
}
