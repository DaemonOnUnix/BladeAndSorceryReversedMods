using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000253 RID: 595
	[Flags]
	internal enum CV_prop : ushort
	{
		// Token: 0x04000BBF RID: 3007
		packed = 1,
		// Token: 0x04000BC0 RID: 3008
		ctor = 2,
		// Token: 0x04000BC1 RID: 3009
		ovlops = 4,
		// Token: 0x04000BC2 RID: 3010
		isnested = 8,
		// Token: 0x04000BC3 RID: 3011
		cnested = 16,
		// Token: 0x04000BC4 RID: 3012
		opassign = 32,
		// Token: 0x04000BC5 RID: 3013
		opcast = 64,
		// Token: 0x04000BC6 RID: 3014
		fwdref = 128,
		// Token: 0x04000BC7 RID: 3015
		scoped = 256
	}
}
