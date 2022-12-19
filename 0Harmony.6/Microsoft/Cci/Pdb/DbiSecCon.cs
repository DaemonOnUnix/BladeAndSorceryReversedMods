using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FE RID: 766
	internal struct DbiSecCon
	{
		// Token: 0x0600122D RID: 4653 RVA: 0x0003BC84 File Offset: 0x00039E84
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

		// Token: 0x04000EE1 RID: 3809
		internal short section;

		// Token: 0x04000EE2 RID: 3810
		internal short pad1;

		// Token: 0x04000EE3 RID: 3811
		internal int offset;

		// Token: 0x04000EE4 RID: 3812
		internal int size;

		// Token: 0x04000EE5 RID: 3813
		internal uint flags;

		// Token: 0x04000EE6 RID: 3814
		internal short module;

		// Token: 0x04000EE7 RID: 3815
		internal short pad2;

		// Token: 0x04000EE8 RID: 3816
		internal uint dataCrc;

		// Token: 0x04000EE9 RID: 3817
		internal uint relocCrc;
	}
}
