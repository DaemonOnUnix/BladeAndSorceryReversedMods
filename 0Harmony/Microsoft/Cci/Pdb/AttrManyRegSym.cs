using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003A9 RID: 937
	internal struct AttrManyRegSym
	{
		// Token: 0x04000D80 RID: 3456
		internal uint typind;

		// Token: 0x04000D81 RID: 3457
		internal uint offCod;

		// Token: 0x04000D82 RID: 3458
		internal ushort segCod;

		// Token: 0x04000D83 RID: 3459
		internal ushort flags;

		// Token: 0x04000D84 RID: 3460
		internal byte count;

		// Token: 0x04000D85 RID: 3461
		internal byte[] reg;

		// Token: 0x04000D86 RID: 3462
		internal string name;
	}
}
