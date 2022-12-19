using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FB RID: 763
	internal struct DbiDbgHdr
	{
		// Token: 0x0600122A RID: 4650 RVA: 0x0003BA14 File Offset: 0x00039C14
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

		// Token: 0x04000EB5 RID: 3765
		internal ushort snFPO;

		// Token: 0x04000EB6 RID: 3766
		internal ushort snException;

		// Token: 0x04000EB7 RID: 3767
		internal ushort snFixup;

		// Token: 0x04000EB8 RID: 3768
		internal ushort snOmapToSrc;

		// Token: 0x04000EB9 RID: 3769
		internal ushort snOmapFromSrc;

		// Token: 0x04000EBA RID: 3770
		internal ushort snSectionHdr;

		// Token: 0x04000EBB RID: 3771
		internal ushort snTokenRidMap;

		// Token: 0x04000EBC RID: 3772
		internal ushort snXdata;

		// Token: 0x04000EBD RID: 3773
		internal ushort snPdata;

		// Token: 0x04000EBE RID: 3774
		internal ushort snNewFPO;

		// Token: 0x04000EBF RID: 3775
		internal ushort snSectionHdrOrig;
	}
}
