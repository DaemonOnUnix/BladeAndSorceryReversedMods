using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000315 RID: 789
	internal class PdbSlot
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x0003E274 File Offset: 0x0003C474
		internal PdbSlot(uint slot, uint typeToken, string name, ushort flags)
		{
			this.slot = slot;
			this.typeToken = typeToken;
			this.name = name;
			this.flags = flags;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0003E29C File Offset: 0x0003C49C
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

		// Token: 0x04000F33 RID: 3891
		internal uint slot;

		// Token: 0x04000F34 RID: 3892
		internal uint typeToken;

		// Token: 0x04000F35 RID: 3893
		internal string name;

		// Token: 0x04000F36 RID: 3894
		internal ushort flags;
	}
}
