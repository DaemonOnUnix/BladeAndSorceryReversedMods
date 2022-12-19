using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200040B RID: 1035
	internal class PdbSlot
	{
		// Token: 0x060015E4 RID: 5604 RVA: 0x000461BC File Offset: 0x000443BC
		internal PdbSlot(uint slot, uint typeToken, string name, ushort flags)
		{
			this.slot = slot;
			this.typeToken = typeToken;
			this.name = name;
			this.flags = flags;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x000461E4 File Offset: 0x000443E4
		internal PdbSlot(BitAccess bits)
		{
			AttrSlotSym attrSlotSym;
			bits.ReadUInt32(out attrSlotSym.index);
			bits.ReadUInt32(out attrSlotSym.typind);
			bits.ReadUInt32(out attrSlotSym.offCod);
			bits.ReadUInt16(out attrSlotSym.segCod);
			bits.ReadUInt16(out attrSlotSym.flags);
			bits.ReadCString(out attrSlotSym.name);
			this.slot = attrSlotSym.index;
			this.typeToken = attrSlotSym.typind;
			this.name = attrSlotSym.name;
			this.flags = attrSlotSym.flags;
		}

		// Token: 0x04000F71 RID: 3953
		internal uint slot;

		// Token: 0x04000F72 RID: 3954
		internal uint typeToken;

		// Token: 0x04000F73 RID: 3955
		internal string name;

		// Token: 0x04000F74 RID: 3956
		internal ushort flags;
	}
}
