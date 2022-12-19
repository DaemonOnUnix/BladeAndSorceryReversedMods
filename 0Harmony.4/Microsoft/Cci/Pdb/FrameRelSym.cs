using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003B8 RID: 952
	internal struct FrameRelSym
	{
		// Token: 0x04000DBE RID: 3518
		internal int off;

		// Token: 0x04000DBF RID: 3519
		internal uint typind;

		// Token: 0x04000DC0 RID: 3520
		internal uint offCod;

		// Token: 0x04000DC1 RID: 3521
		internal ushort segCod;

		// Token: 0x04000DC2 RID: 3522
		internal ushort flags;

		// Token: 0x04000DC3 RID: 3523
		internal string name;
	}
}
