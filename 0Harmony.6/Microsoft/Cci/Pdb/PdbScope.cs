using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000314 RID: 788
	internal class PdbScope
	{
		// Token: 0x06001272 RID: 4722 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
		internal PdbScope(uint address, uint offset, uint length, PdbSlot[] slots, PdbConstant[] constants, string[] usedNamespaces)
		{
			this.constants = constants;
			this.slots = slots;
			this.scopes = new PdbScope[0];
			this.usedNamespaces = usedNamespaces;
			this.address = address;
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0003E024 File Offset: 0x0003C224
		internal PdbScope(uint address, uint length, PdbSlot[] slots, PdbConstant[] constants, string[] usedNamespaces)
			: this(address, 0U, length, slots, constants, usedNamespaces)
		{
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0003E034 File Offset: 0x0003C234
		internal PdbScope(uint funcOffset, BlockSym32 block, BitAccess bits, out uint typind)
		{
			this.address = block.off;
			this.offset = block.off - funcOffset;
			this.length = block.len;
			typind = 0U;
			int num;
			int num2;
			int num3;
			int num4;
			PdbFunction.CountScopesAndSlots(bits, block.end, out num, out num2, out num3, out num4);
			this.constants = new PdbConstant[num];
			this.scopes = new PdbScope[num2];
			this.slots = new PdbSlot[num3];
			this.usedNamespaces = new string[num4];
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			while ((long)bits.Position < (long)((ulong)block.end))
			{
				ushort num9;
				bits.ReadUInt16(out num9);
				int position = bits.Position;
				int num10 = bits.Position + (int)num9;
				bits.Position = position;
				ushort num11;
				bits.ReadUInt16(out num11);
				SYM sym = (SYM)num11;
				if (sym <= SYM.S_BLOCK32)
				{
					if (sym == SYM.S_END)
					{
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_BLOCK32)
					{
						BlockSym32 blockSym = default(BlockSym32);
						bits.ReadUInt32(out blockSym.parent);
						bits.ReadUInt32(out blockSym.end);
						bits.ReadUInt32(out blockSym.len);
						bits.ReadUInt32(out blockSym.off);
						bits.ReadUInt16(out blockSym.seg);
						bits.SkipCString(out blockSym.name);
						bits.Position = num10;
						this.scopes[num6++] = new PdbScope(funcOffset, blockSym, bits, out typind);
						continue;
					}
				}
				else
				{
					if (sym == SYM.S_MANSLOT)
					{
						this.slots[num7++] = new PdbSlot(bits);
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_UNAMESPACE)
					{
						bits.ReadCString(out this.usedNamespaces[num8++]);
						bits.Position = num10;
						continue;
					}
					if (sym == SYM.S_MANCONSTANT)
					{
						this.constants[num5++] = new PdbConstant(bits);
						bits.Position = num10;
						continue;
					}
				}
				bits.Position = num10;
			}
			if ((long)bits.Position != (long)((ulong)block.end))
			{
				throw new Exception("Not at S_END");
			}
			ushort num12;
			bits.ReadUInt16(out num12);
			ushort num13;
			bits.ReadUInt16(out num13);
			if (num13 != 6)
			{
				throw new Exception("Missing S_END");
			}
		}

		// Token: 0x04000F2C RID: 3884
		internal PdbConstant[] constants;

		// Token: 0x04000F2D RID: 3885
		internal PdbSlot[] slots;

		// Token: 0x04000F2E RID: 3886
		internal PdbScope[] scopes;

		// Token: 0x04000F2F RID: 3887
		internal string[] usedNamespaces;

		// Token: 0x04000F30 RID: 3888
		internal uint address;

		// Token: 0x04000F31 RID: 3889
		internal uint offset;

		// Token: 0x04000F32 RID: 3890
		internal uint length;
	}
}
