using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C9 RID: 713
	internal struct ProcSym32
	{
		// Token: 0x04000DA0 RID: 3488
		internal uint parent;

		// Token: 0x04000DA1 RID: 3489
		internal uint end;

		// Token: 0x04000DA2 RID: 3490
		internal uint next;

		// Token: 0x04000DA3 RID: 3491
		internal uint len;

		// Token: 0x04000DA4 RID: 3492
		internal uint dbgStart;

		// Token: 0x04000DA5 RID: 3493
		internal uint dbgEnd;

		// Token: 0x04000DA6 RID: 3494
		internal uint typind;

		// Token: 0x04000DA7 RID: 3495
		internal uint off;

		// Token: 0x04000DA8 RID: 3496
		internal ushort seg;

		// Token: 0x04000DA9 RID: 3497
		internal byte flags;

		// Token: 0x04000DAA RID: 3498
		internal string name;
	}
}
