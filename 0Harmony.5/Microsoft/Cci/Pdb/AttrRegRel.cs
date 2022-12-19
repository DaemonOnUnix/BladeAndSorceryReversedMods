using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003CA RID: 970
	internal struct AttrRegRel
	{
		// Token: 0x04000E30 RID: 3632
		internal uint off;

		// Token: 0x04000E31 RID: 3633
		internal uint typind;

		// Token: 0x04000E32 RID: 3634
		internal ushort reg;

		// Token: 0x04000E33 RID: 3635
		internal uint offCod;

		// Token: 0x04000E34 RID: 3636
		internal ushort segCod;

		// Token: 0x04000E35 RID: 3637
		internal ushort flags;

		// Token: 0x04000E36 RID: 3638
		internal string name;
	}
}
