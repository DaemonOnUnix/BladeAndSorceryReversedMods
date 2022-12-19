using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002B4 RID: 692
	internal struct AttrManyRegSym2
	{
		// Token: 0x04000D48 RID: 3400
		internal uint typind;

		// Token: 0x04000D49 RID: 3401
		internal uint offCod;

		// Token: 0x04000D4A RID: 3402
		internal ushort segCod;

		// Token: 0x04000D4B RID: 3403
		internal ushort flags;

		// Token: 0x04000D4C RID: 3404
		internal ushort count;

		// Token: 0x04000D4D RID: 3405
		internal ushort[] reg;

		// Token: 0x04000D4E RID: 3406
		internal string name;
	}
}
