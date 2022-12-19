using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003BF RID: 959
	internal struct ProcSym32
	{
		// Token: 0x04000DDF RID: 3551
		internal uint parent;

		// Token: 0x04000DE0 RID: 3552
		internal uint end;

		// Token: 0x04000DE1 RID: 3553
		internal uint next;

		// Token: 0x04000DE2 RID: 3554
		internal uint len;

		// Token: 0x04000DE3 RID: 3555
		internal uint dbgStart;

		// Token: 0x04000DE4 RID: 3556
		internal uint dbgEnd;

		// Token: 0x04000DE5 RID: 3557
		internal uint typind;

		// Token: 0x04000DE6 RID: 3558
		internal uint off;

		// Token: 0x04000DE7 RID: 3559
		internal ushort seg;

		// Token: 0x04000DE8 RID: 3560
		internal byte flags;

		// Token: 0x04000DE9 RID: 3561
		internal string name;
	}
}
