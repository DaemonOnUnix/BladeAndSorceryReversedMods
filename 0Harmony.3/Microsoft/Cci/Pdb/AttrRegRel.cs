using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D4 RID: 724
	internal struct AttrRegRel
	{
		// Token: 0x04000DF1 RID: 3569
		internal uint off;

		// Token: 0x04000DF2 RID: 3570
		internal uint typind;

		// Token: 0x04000DF3 RID: 3571
		internal ushort reg;

		// Token: 0x04000DF4 RID: 3572
		internal uint offCod;

		// Token: 0x04000DF5 RID: 3573
		internal ushort segCod;

		// Token: 0x04000DF6 RID: 3574
		internal ushort flags;

		// Token: 0x04000DF7 RID: 3575
		internal string name;
	}
}
