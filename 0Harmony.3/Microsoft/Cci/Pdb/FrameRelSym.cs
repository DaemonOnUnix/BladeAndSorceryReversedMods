using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002C2 RID: 706
	internal struct FrameRelSym
	{
		// Token: 0x04000D7F RID: 3455
		internal int off;

		// Token: 0x04000D80 RID: 3456
		internal uint typind;

		// Token: 0x04000D81 RID: 3457
		internal uint offCod;

		// Token: 0x04000D82 RID: 3458
		internal ushort segCod;

		// Token: 0x04000D83 RID: 3459
		internal ushort flags;

		// Token: 0x04000D84 RID: 3460
		internal string name;
	}
}
