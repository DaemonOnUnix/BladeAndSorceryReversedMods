using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003B2 RID: 946
	internal struct CompileSym
	{
		// Token: 0x04000DAC RID: 3500
		internal uint flags;

		// Token: 0x04000DAD RID: 3501
		internal ushort machine;

		// Token: 0x04000DAE RID: 3502
		internal ushort verFEMajor;

		// Token: 0x04000DAF RID: 3503
		internal ushort verFEMinor;

		// Token: 0x04000DB0 RID: 3504
		internal ushort verFEBuild;

		// Token: 0x04000DB1 RID: 3505
		internal ushort verMajor;

		// Token: 0x04000DB2 RID: 3506
		internal ushort verMinor;

		// Token: 0x04000DB3 RID: 3507
		internal ushort verBuild;

		// Token: 0x04000DB4 RID: 3508
		internal string verSt;

		// Token: 0x04000DB5 RID: 3509
		internal string[] verArgs;
	}
}
