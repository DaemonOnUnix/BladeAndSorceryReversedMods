using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CE RID: 718
	internal struct TrampolineSym
	{
		// Token: 0x04000DD3 RID: 3539
		internal ushort trampType;

		// Token: 0x04000DD4 RID: 3540
		internal ushort cbThunk;

		// Token: 0x04000DD5 RID: 3541
		internal uint offThunk;

		// Token: 0x04000DD6 RID: 3542
		internal uint offTarget;

		// Token: 0x04000DD7 RID: 3543
		internal ushort sectThunk;

		// Token: 0x04000DD8 RID: 3544
		internal ushort sectTarget;
	}
}
