using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D8 RID: 728
	internal struct ProcSymIa64
	{
		// Token: 0x04000E0F RID: 3599
		internal uint parent;

		// Token: 0x04000E10 RID: 3600
		internal uint end;

		// Token: 0x04000E11 RID: 3601
		internal uint next;

		// Token: 0x04000E12 RID: 3602
		internal uint len;

		// Token: 0x04000E13 RID: 3603
		internal uint dbgStart;

		// Token: 0x04000E14 RID: 3604
		internal uint dbgEnd;

		// Token: 0x04000E15 RID: 3605
		internal uint typind;

		// Token: 0x04000E16 RID: 3606
		internal uint off;

		// Token: 0x04000E17 RID: 3607
		internal ushort seg;

		// Token: 0x04000E18 RID: 3608
		internal ushort retReg;

		// Token: 0x04000E19 RID: 3609
		internal byte flags;

		// Token: 0x04000E1A RID: 3610
		internal string name;
	}
}
