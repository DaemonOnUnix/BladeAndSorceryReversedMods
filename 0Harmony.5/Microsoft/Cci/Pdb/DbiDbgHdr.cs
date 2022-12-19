using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F1 RID: 1009
	internal struct DbiDbgHdr
	{
		// Token: 0x06001599 RID: 5529 RVA: 0x0004395C File Offset: 0x00041B5C
		internal DbiDbgHdr(BitAccess bits)
		{
			bits.ReadUInt16(out this.snFPO);
			bits.ReadUInt16(out this.snException);
			bits.ReadUInt16(out this.snFixup);
			bits.ReadUInt16(out this.snOmapToSrc);
			bits.ReadUInt16(out this.snOmapFromSrc);
			bits.ReadUInt16(out this.snSectionHdr);
			bits.ReadUInt16(out this.snTokenRidMap);
			bits.ReadUInt16(out this.snXdata);
			bits.ReadUInt16(out this.snPdata);
			bits.ReadUInt16(out this.snNewFPO);
			bits.ReadUInt16(out this.snSectionHdrOrig);
		}

		// Token: 0x04000EF4 RID: 3828
		internal ushort snFPO;

		// Token: 0x04000EF5 RID: 3829
		internal ushort snException;

		// Token: 0x04000EF6 RID: 3830
		internal ushort snFixup;

		// Token: 0x04000EF7 RID: 3831
		internal ushort snOmapToSrc;

		// Token: 0x04000EF8 RID: 3832
		internal ushort snOmapFromSrc;

		// Token: 0x04000EF9 RID: 3833
		internal ushort snSectionHdr;

		// Token: 0x04000EFA RID: 3834
		internal ushort snTokenRidMap;

		// Token: 0x04000EFB RID: 3835
		internal ushort snXdata;

		// Token: 0x04000EFC RID: 3836
		internal ushort snPdata;

		// Token: 0x04000EFD RID: 3837
		internal ushort snNewFPO;

		// Token: 0x04000EFE RID: 3838
		internal ushort snSectionHdrOrig;
	}
}
