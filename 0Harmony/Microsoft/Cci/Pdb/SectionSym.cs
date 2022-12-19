using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003DA RID: 986
	internal struct SectionSym
	{
		// Token: 0x04000E92 RID: 3730
		internal ushort isec;

		// Token: 0x04000E93 RID: 3731
		internal byte align;

		// Token: 0x04000E94 RID: 3732
		internal byte bReserved;

		// Token: 0x04000E95 RID: 3733
		internal uint rva;

		// Token: 0x04000E96 RID: 3734
		internal uint cb;

		// Token: 0x04000E97 RID: 3735
		internal uint characteristics;

		// Token: 0x04000E98 RID: 3736
		internal string name;
	}
}
