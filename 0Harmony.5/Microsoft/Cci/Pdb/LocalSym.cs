using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003D7 RID: 983
	internal struct LocalSym
	{
		// Token: 0x04000E82 RID: 3714
		internal uint id;

		// Token: 0x04000E83 RID: 3715
		internal uint typind;

		// Token: 0x04000E84 RID: 3716
		internal ushort flags;

		// Token: 0x04000E85 RID: 3717
		internal uint idParent;

		// Token: 0x04000E86 RID: 3718
		internal uint offParent;

		// Token: 0x04000E87 RID: 3719
		internal uint expr;

		// Token: 0x04000E88 RID: 3720
		internal uint pad0;

		// Token: 0x04000E89 RID: 3721
		internal uint pad1;

		// Token: 0x04000E8A RID: 3722
		internal string name;
	}
}
