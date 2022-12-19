using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F4 RID: 1012
	internal struct DbiSecCon
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x00043BCC File Offset: 0x00041DCC
		internal DbiSecCon(BitAccess bits)
		{
			bits.ReadInt16(out this.section);
			bits.ReadInt16(out this.pad1);
			bits.ReadInt32(out this.offset);
			bits.ReadInt32(out this.size);
			bits.ReadUInt32(out this.flags);
			bits.ReadInt16(out this.module);
			bits.ReadInt16(out this.pad2);
			bits.ReadUInt32(out this.dataCrc);
			bits.ReadUInt32(out this.relocCrc);
		}

		// Token: 0x04000F20 RID: 3872
		internal short section;

		// Token: 0x04000F21 RID: 3873
		internal short pad1;

		// Token: 0x04000F22 RID: 3874
		internal int offset;

		// Token: 0x04000F23 RID: 3875
		internal int size;

		// Token: 0x04000F24 RID: 3876
		internal uint flags;

		// Token: 0x04000F25 RID: 3877
		internal short module;

		// Token: 0x04000F26 RID: 3878
		internal short pad2;

		// Token: 0x04000F27 RID: 3879
		internal uint dataCrc;

		// Token: 0x04000F28 RID: 3880
		internal uint relocCrc;
	}
}
