using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F2 RID: 1010
	internal struct DbiHeader
	{
		// Token: 0x0600159A RID: 5530 RVA: 0x000439F0 File Offset: 0x00041BF0
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

		// Token: 0x04000EFF RID: 3839
		internal int sig;

		// Token: 0x04000F00 RID: 3840
		internal int ver;

		// Token: 0x04000F01 RID: 3841
		internal int age;

		// Token: 0x04000F02 RID: 3842
		internal short gssymStream;

		// Token: 0x04000F03 RID: 3843
		internal ushort vers;

		// Token: 0x04000F04 RID: 3844
		internal short pssymStream;

		// Token: 0x04000F05 RID: 3845
		internal ushort pdbver;

		// Token: 0x04000F06 RID: 3846
		internal short symrecStream;

		// Token: 0x04000F07 RID: 3847
		internal ushort pdbver2;

		// Token: 0x04000F08 RID: 3848
		internal int gpmodiSize;

		// Token: 0x04000F09 RID: 3849
		internal int secconSize;

		// Token: 0x04000F0A RID: 3850
		internal int secmapSize;

		// Token: 0x04000F0B RID: 3851
		internal int filinfSize;

		// Token: 0x04000F0C RID: 3852
		internal int tsmapSize;

		// Token: 0x04000F0D RID: 3853
		internal int mfcIndex;

		// Token: 0x04000F0E RID: 3854
		internal int dbghdrSize;

		// Token: 0x04000F0F RID: 3855
		internal int ecinfoSize;

		// Token: 0x04000F10 RID: 3856
		internal ushort flags;

		// Token: 0x04000F11 RID: 3857
		internal ushort machine;

		// Token: 0x04000F12 RID: 3858
		internal int reserved;
	}
}
