using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000266 RID: 614
	internal struct LeafMFunc
	{
		// Token: 0x04000C05 RID: 3077
		internal uint rvtype;

		// Token: 0x04000C06 RID: 3078
		internal uint classtype;

		// Token: 0x04000C07 RID: 3079
		internal uint thistype;

		// Token: 0x04000C08 RID: 3080
		internal byte calltype;

		// Token: 0x04000C09 RID: 3081
		internal byte reserved;

		// Token: 0x04000C0A RID: 3082
		internal ushort parmcount;

		// Token: 0x04000C0B RID: 3083
		internal uint arglist;

		// Token: 0x04000C0C RID: 3084
		internal int thisadjust;
	}
}
