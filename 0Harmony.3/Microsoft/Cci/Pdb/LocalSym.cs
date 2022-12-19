using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002E1 RID: 737
	internal struct LocalSym
	{
		// Token: 0x04000E43 RID: 3651
		internal uint id;

		// Token: 0x04000E44 RID: 3652
		internal uint typind;

		// Token: 0x04000E45 RID: 3653
		internal ushort flags;

		// Token: 0x04000E46 RID: 3654
		internal uint idParent;

		// Token: 0x04000E47 RID: 3655
		internal uint offParent;

		// Token: 0x04000E48 RID: 3656
		internal uint expr;

		// Token: 0x04000E49 RID: 3657
		internal uint pad0;

		// Token: 0x04000E4A RID: 3658
		internal uint pad1;

		// Token: 0x04000E4B RID: 3659
		internal string name;
	}
}
