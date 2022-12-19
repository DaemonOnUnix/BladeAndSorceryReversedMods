using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003C4 RID: 964
	internal struct TrampolineSym
	{
		// Token: 0x04000E12 RID: 3602
		internal ushort trampType;

		// Token: 0x04000E13 RID: 3603
		internal ushort cbThunk;

		// Token: 0x04000E14 RID: 3604
		internal uint offThunk;

		// Token: 0x04000E15 RID: 3605
		internal uint offTarget;

		// Token: 0x04000E16 RID: 3606
		internal ushort sectThunk;

		// Token: 0x04000E17 RID: 3607
		internal ushort sectTarget;
	}
}
