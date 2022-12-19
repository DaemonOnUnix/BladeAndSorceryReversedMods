using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003C2 RID: 962
	internal struct ThunkSym32
	{
		// Token: 0x04000E06 RID: 3590
		internal uint parent;

		// Token: 0x04000E07 RID: 3591
		internal uint end;

		// Token: 0x04000E08 RID: 3592
		internal uint next;

		// Token: 0x04000E09 RID: 3593
		internal uint off;

		// Token: 0x04000E0A RID: 3594
		internal ushort seg;

		// Token: 0x04000E0B RID: 3595
		internal ushort len;

		// Token: 0x04000E0C RID: 3596
		internal byte ord;

		// Token: 0x04000E0D RID: 3597
		internal string name;

		// Token: 0x04000E0E RID: 3598
		internal byte[] variant;
	}
}
