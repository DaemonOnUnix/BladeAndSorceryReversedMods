using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002B3 RID: 691
	internal struct AttrManyRegSym
	{
		// Token: 0x04000D41 RID: 3393
		internal uint typind;

		// Token: 0x04000D42 RID: 3394
		internal uint offCod;

		// Token: 0x04000D43 RID: 3395
		internal ushort segCod;

		// Token: 0x04000D44 RID: 3396
		internal ushort flags;

		// Token: 0x04000D45 RID: 3397
		internal byte count;

		// Token: 0x04000D46 RID: 3398
		internal byte[] reg;

		// Token: 0x04000D47 RID: 3399
		internal string name;
	}
}
