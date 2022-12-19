using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200035C RID: 860
	internal struct LeafMFunc
	{
		// Token: 0x04000C44 RID: 3140
		internal uint rvtype;

		// Token: 0x04000C45 RID: 3141
		internal uint classtype;

		// Token: 0x04000C46 RID: 3142
		internal uint thistype;

		// Token: 0x04000C47 RID: 3143
		internal byte calltype;

		// Token: 0x04000C48 RID: 3144
		internal byte reserved;

		// Token: 0x04000C49 RID: 3145
		internal ushort parmcount;

		// Token: 0x04000C4A RID: 3146
		internal uint arglist;

		// Token: 0x04000C4B RID: 3147
		internal int thisadjust;
	}
}
