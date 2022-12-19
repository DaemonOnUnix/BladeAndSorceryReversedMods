using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FC RID: 764
	internal struct DbiHeader
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x0003BAA8 File Offset: 0x00039CA8
		internal DbiHeader(BitAccess bits)
		{
			bits.ReadInt32(out this.sig);
			bits.ReadInt32(out this.ver);
			bits.ReadInt32(out this.age);
			bits.ReadInt16(out this.gssymStream);
			bits.ReadUInt16(out this.vers);
			bits.ReadInt16(out this.pssymStream);
			bits.ReadUInt16(out this.pdbver);
			bits.ReadInt16(out this.symrecStream);
			bits.ReadUInt16(out this.pdbver2);
			bits.ReadInt32(out this.gpmodiSize);
			bits.ReadInt32(out this.secconSize);
			bits.ReadInt32(out this.secmapSize);
			bits.ReadInt32(out this.filinfSize);
			bits.ReadInt32(out this.tsmapSize);
			bits.ReadInt32(out this.mfcIndex);
			bits.ReadInt32(out this.dbghdrSize);
			bits.ReadInt32(out this.ecinfoSize);
			bits.ReadUInt16(out this.flags);
			bits.ReadUInt16(out this.machine);
			bits.ReadInt32(out this.reserved);
		}

		// Token: 0x04000EC0 RID: 3776
		internal int sig;

		// Token: 0x04000EC1 RID: 3777
		internal int ver;

		// Token: 0x04000EC2 RID: 3778
		internal int age;

		// Token: 0x04000EC3 RID: 3779
		internal short gssymStream;

		// Token: 0x04000EC4 RID: 3780
		internal ushort vers;

		// Token: 0x04000EC5 RID: 3781
		internal short pssymStream;

		// Token: 0x04000EC6 RID: 3782
		internal ushort pdbver;

		// Token: 0x04000EC7 RID: 3783
		internal short symrecStream;

		// Token: 0x04000EC8 RID: 3784
		internal ushort pdbver2;

		// Token: 0x04000EC9 RID: 3785
		internal int gpmodiSize;

		// Token: 0x04000ECA RID: 3786
		internal int secconSize;

		// Token: 0x04000ECB RID: 3787
		internal int secmapSize;

		// Token: 0x04000ECC RID: 3788
		internal int filinfSize;

		// Token: 0x04000ECD RID: 3789
		internal int tsmapSize;

		// Token: 0x04000ECE RID: 3790
		internal int mfcIndex;

		// Token: 0x04000ECF RID: 3791
		internal int dbghdrSize;

		// Token: 0x04000ED0 RID: 3792
		internal int ecinfoSize;

		// Token: 0x04000ED1 RID: 3793
		internal ushort flags;

		// Token: 0x04000ED2 RID: 3794
		internal ushort machine;

		// Token: 0x04000ED3 RID: 3795
		internal int reserved;
	}
}
