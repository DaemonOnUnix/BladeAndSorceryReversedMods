using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002E4 RID: 740
	internal struct SectionSym
	{
		// Token: 0x04000E53 RID: 3667
		internal ushort isec;

		// Token: 0x04000E54 RID: 3668
		internal byte align;

		// Token: 0x04000E55 RID: 3669
		internal byte bReserved;

		// Token: 0x04000E56 RID: 3670
		internal uint rva;

		// Token: 0x04000E57 RID: 3671
		internal uint cb;

		// Token: 0x04000E58 RID: 3672
		internal uint characteristics;

		// Token: 0x04000E59 RID: 3673
		internal string name;
	}
}
