using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002CB RID: 715
	internal struct ManProcSymMips
	{
		// Token: 0x04000DB7 RID: 3511
		internal uint parent;

		// Token: 0x04000DB8 RID: 3512
		internal uint end;

		// Token: 0x04000DB9 RID: 3513
		internal uint next;

		// Token: 0x04000DBA RID: 3514
		internal uint len;

		// Token: 0x04000DBB RID: 3515
		internal uint dbgStart;

		// Token: 0x04000DBC RID: 3516
		internal uint dbgEnd;

		// Token: 0x04000DBD RID: 3517
		internal uint regSave;

		// Token: 0x04000DBE RID: 3518
		internal uint fpSave;

		// Token: 0x04000DBF RID: 3519
		internal uint intOff;

		// Token: 0x04000DC0 RID: 3520
		internal uint fpOff;

		// Token: 0x04000DC1 RID: 3521
		internal uint token;

		// Token: 0x04000DC2 RID: 3522
		internal uint off;

		// Token: 0x04000DC3 RID: 3523
		internal ushort seg;

		// Token: 0x04000DC4 RID: 3524
		internal byte retReg;

		// Token: 0x04000DC5 RID: 3525
		internal byte frameReg;

		// Token: 0x04000DC6 RID: 3526
		internal string name;
	}
}
