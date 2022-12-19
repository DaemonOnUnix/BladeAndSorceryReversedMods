using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002D7 RID: 727
	internal struct ProcSymMips
	{
		// Token: 0x04000DFF RID: 3583
		internal uint parent;

		// Token: 0x04000E00 RID: 3584
		internal uint end;

		// Token: 0x04000E01 RID: 3585
		internal uint next;

		// Token: 0x04000E02 RID: 3586
		internal uint len;

		// Token: 0x04000E03 RID: 3587
		internal uint dbgStart;

		// Token: 0x04000E04 RID: 3588
		internal uint dbgEnd;

		// Token: 0x04000E05 RID: 3589
		internal uint regSave;

		// Token: 0x04000E06 RID: 3590
		internal uint fpSave;

		// Token: 0x04000E07 RID: 3591
		internal uint intOff;

		// Token: 0x04000E08 RID: 3592
		internal uint fpOff;

		// Token: 0x04000E09 RID: 3593
		internal uint typind;

		// Token: 0x04000E0A RID: 3594
		internal uint off;

		// Token: 0x04000E0B RID: 3595
		internal ushort seg;

		// Token: 0x04000E0C RID: 3596
		internal byte retReg;

		// Token: 0x04000E0D RID: 3597
		internal byte frameReg;

		// Token: 0x04000E0E RID: 3598
		internal string name;
	}
}
