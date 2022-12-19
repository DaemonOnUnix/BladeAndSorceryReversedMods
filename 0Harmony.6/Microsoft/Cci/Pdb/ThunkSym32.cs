using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CC RID: 716
	internal struct ThunkSym32
	{
		// Token: 0x04000DC7 RID: 3527
		internal uint parent;

		// Token: 0x04000DC8 RID: 3528
		internal uint end;

		// Token: 0x04000DC9 RID: 3529
		internal uint next;

		// Token: 0x04000DCA RID: 3530
		internal uint off;

		// Token: 0x04000DCB RID: 3531
		internal ushort seg;

		// Token: 0x04000DCC RID: 3532
		internal ushort len;

		// Token: 0x04000DCD RID: 3533
		internal byte ord;

		// Token: 0x04000DCE RID: 3534
		internal string name;

		// Token: 0x04000DCF RID: 3535
		internal byte[] variant;
	}
}
