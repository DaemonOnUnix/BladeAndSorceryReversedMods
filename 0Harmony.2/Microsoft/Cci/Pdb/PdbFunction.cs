using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000401 RID: 1025
	internal class PdbFunction
	{
		// Token: 0x060015C3 RID: 5571 RVA: 0x00045404 File Offset: 0x00043604
		private static string StripNamespace(string module)
		{
			int num = module.LastIndexOf('.');
			if (num > 0)
			{
				return module.Substring(num + 1);
			}
			return module;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0004542C File Offset: 0x0004362C
		internal void AdjustVisualBasicScopes()
		{
			if (!this.visualBasicScopesAdjusted)
			{
				this.visualBasicScopesAdjusted = true;
				foreach (PdbScope pdbScope in this.scopes)
				{
					this.AdjustVisualBasicScopes(pdbScope.scopes);
				}
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00045470 File Offset: 0x00043670
		private void AdjustVisualBasicScopes(PdbScope[] scopes)
		{
			foreach (PdbScope pdbScope in scopes)
			{
				pdbScope.length += 1U;
				this.AdjustVisualBasicScopes(pdbScope.scopes);
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000454AC File Offset: 0x000436AC
		internal static PdbFunction[] LoadManagedFunctions(BitAccess bits, uint limit, bool readStrings)
		{
			int position = bits.Position;
			int num = 0;
			while ((long)bits.Position < (long)((ulong)limit))
			{
				ushort num2;
				bits.ReadUInt16(out num2);
				int position2 = bits.Position;
				int num3 = bits.Position + (int)num2;
				bits.Position = position2;
				ushort num4;
				bits.ReadUInt16(out num4);
				SYM sym = (SYM)num4;
				if (sym != SYM.S_END)
				{
					if (sym - SYM.S_GMANPROC <= 1)
					{
						ManProcSym manProcSym;
						bits.ReadUInt32(out manProcSym.parent);
						bits.ReadUInt32(out manProcSym.end);
						bits.Position = (int)manProcSym.end;
						num++;
					}
					else
					{
						bits.Position = num3;
					}
				}
				else
				{
					bits.Position = num3;
				}
			}
			if (num == 0)
			{
				return null;
			}
			bits.Position = position;
			PdbFunction[] array = new PdbFunction[num];
			int num5 = 0;
			while ((long)bits.Position < (long)((ulong)limit))
			{
				ushort num6;
				bits.ReadUInt16(out num6);
				int position3 = bits.Position;
				int num7 = bits.Position + (int)num6;
				ushort num8;
				bits.ReadUInt16(out num8);
				SYM sym = (SYM)num8;
				if (sym - SYM.S_GMANPROC <= 1)
				{
					ManProcSym manProcSym2;
					bits.ReadUInt32(out manProcSym2.parent);
					bits.ReadUInt32(out manProcSym2.end);
					bits.ReadUInt32(out manProcSym2.next);
					bits.ReadUInt32(out manProcSym2.len);
					bits.ReadUInt32(out manProcSym2.dbgStart);
					bits.ReadUInt32(out manProcSym2.dbgEnd);
					bits.ReadUInt32(out manProcSym2.token);
					bits.ReadUInt32(out manProcSym2.off);
					bits.ReadUInt16(out manProcSym2.seg);
					bits.ReadUInt8(out manProcSym2.flags);
					bits.ReadUInt16(out manProcSym2.retReg);
					if (readStrings)
					{
						bits.ReadCString(out manProcSym2.name);
					}
					else
					{
						bits.SkipCString(out manProcSym2.name);
					}
					bits.Position = num7;
					array[num5++] = new PdbFunction(manProcSym2, bits);
				}
				else
				{
					bits.Position = num7;
				}
			}
			return array;
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0004567C File Offset: 0x0004387C
		internal static void CountScopesAndSlots(BitAccess bits, uint limit, out int constants, out int scopes, out int slots, out int usedNamespaces)
		{
			int position = bits.Position;
			constants = 0;
			slots = 0;
			scopes = 0;
			usedNamespaces = 0;
			while ((long)bits.Position < (long)((ulong)limit))
			{
				ushort num;
				bits.ReadUInt16(out num);
				int position2 = bits.Position;
				int num2 = bits.Position + (int)num;
				bits.Position = position2;
				ushort num3;
				bits.ReadUInt16(out num3);
				SYM sym = (SYM)num3;
				if (sym <= SYM.S_MANSLOT)
				{
					if (sym == SYM.S_BLOCK32)
					{
						BlockSym32 blockSym;
						bits.ReadUInt32(out blockSym.parent);
						bits.ReadUInt32(out blockSym.end);
						scopes++;
						bits.Position = (int)blockSym.end;
						continue;
					}
					if (sym == SYM.S_MANSLOT)
					{
						slots++;
						bits.Position = num2;
						continue;
					}
				}
				else
				{
					if (sym == SYM.S_UNAMESPACE)
					{
						usedNamespaces++;
						bits.Position = num2;
						continue;
					}
					if (sym == SYM.S_MANCONSTANT)
					{
						constants++;
						bits.Position = num2;
						continue;
					}
				}
				bits.Position = num2;
			}
			bits.Position = position;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00002AED File Offset: 0x00000CED
		internal PdbFunction()
		{
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00045780 File Offset: 0x00043980
		internal PdbFunction(ManProcSym proc, BitAccess bits)
		{
			this.token = proc.token;
			this.segment = (uint)proc.seg;
			this.address = proc.off;
			this.length = proc.len;
			if (proc.seg != 1)
			{
				throw new PdbDebugException("Segment is {0}, not 1.", new object[] { proc.seg });
			}
			if (proc.parent != 0U || proc.next != 0U)
			{
				throw new PdbDebugException("Warning parent={0}, next={1}", new object[] { proc.parent, proc.next });
			}
			int num;
			int num2;
			int num3;
			int num4;
			PdbFunction.CountScopesAndSlots(bits, proc.end, out num, out num2, out num3, out num4);
			int num5 = ((num > 0 || num3 > 0 || num4 > 0) ? 1 : 0);
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			this.scopes = new PdbScope[num2 + num5];
			this.slots = new PdbSlot[num3];
			this.constants = new PdbConstant[num];
			this.usedNamespaces = new string[num4];
			if (num5 > 0)
			{
				this.scopes[0] = new PdbScope(this.address, proc.len, this.slots, this.constants, this.usedNamespaces);
			}
			while ((long)bits.Position < (long)((ulong)proc.end))
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
					if (sym != SYM.S_OEM)
					{
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
							this.scopes[num5++] = new PdbScope(this.address, blockSym, bits, out this.slotToken);
							bits.Position = (int)blockSym.end;
							continue;
						}
					}
					else
					{
						OemSymbol oemSymbol;
						bits.ReadGuid(out oemSymbol.idOem);
						bits.ReadUInt32(out oemSymbol.typind);
						if (oemSymbol.idOem == PdbFunction.msilMetaData)
						{
							string text = bits.ReadString();
							if (text == "MD2")
							{
								this.ReadMD2CustomMetadata(bits);
							}
							else if (text == "asyncMethodInfo")
							{
								this.synchronizationInformation = new PdbSynchronizationInformation(bits);
							}
							bits.Position = num10;
							continue;
						}
						throw new PdbDebugException("OEM section: guid={0} ti={1}", new object[] { oemSymbol.idOem, oemSymbol.typind });
					}
				}
				else
				{
					if (sym == SYM.S_MANSLOT)
					{
						this.slots[num6++] = new PdbSlot(bits);
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
						this.constants[num7++] = new PdbConstant(bits);
						bits.Position = num10;
						continue;
					}
				}
				bits.Position = num10;
			}
			if ((long)bits.Position != (long)((ulong)proc.end))
			{
				throw new PdbDebugException("Not at S_END", new object[0]);
			}
			ushort num12;
			bits.ReadUInt16(out num12);
			ushort num13;
			bits.ReadUInt16(out num13);
			if (num13 != 6)
			{
				throw new PdbDebugException("Missing S_END", new object[0]);
			}
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00045B40 File Offset: 0x00043D40
		internal void ReadMD2CustomMetadata(BitAccess bits)
		{
			byte b;
			bits.ReadUInt8(out b);
			if (b == 4)
			{
				byte b2;
				bits.ReadUInt8(out b2);
				bits.Align(4);
				for (;;)
				{
					byte b3 = b2;
					b2 = b3 - 1;
					if (b3 <= 0)
					{
						break;
					}
					this.ReadCustomMetadata(bits);
				}
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00045B7C File Offset: 0x00043D7C
		private void ReadCustomMetadata(BitAccess bits)
		{
			int position = bits.Position;
			byte b;
			bits.ReadUInt8(out b);
			byte b2;
			bits.ReadUInt8(out b2);
			bits.Position += 2;
			uint num;
			bits.ReadUInt32(out num);
			if (b == 4)
			{
				switch (b2)
				{
				case 0:
					this.ReadUsingInfo(bits);
					break;
				case 1:
					this.ReadForwardInfo(bits);
					break;
				case 3:
					this.ReadIteratorLocals(bits);
					break;
				case 4:
					this.ReadForwardIterator(bits);
					break;
				}
			}
			bits.Position = position + (int)num;
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00045C0D File Offset: 0x00043E0D
		private void ReadForwardIterator(BitAccess bits)
		{
			this.iteratorClass = bits.ReadString();
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00045C1C File Offset: 0x00043E1C
		private void ReadIteratorLocals(BitAccess bits)
		{
			uint num;
			bits.ReadUInt32(out num);
			this.iteratorScopes = new List<ILocalScope>((int)num);
			while (num-- > 0U)
			{
				uint num2;
				bits.ReadUInt32(out num2);
				uint num3;
				bits.ReadUInt32(out num3);
				this.iteratorScopes.Add(new PdbIteratorScope(num2, num3 - num2));
			}
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00045C6B File Offset: 0x00043E6B
		private void ReadForwardInfo(BitAccess bits)
		{
			bits.ReadUInt32(out this.tokenOfMethodWhoseUsingInfoAppliesToThisMethod);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00045C7C File Offset: 0x00043E7C
		private void ReadUsingInfo(BitAccess bits)
		{
			ushort num;
			bits.ReadUInt16(out num);
			this.usingCounts = new ushort[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				bits.ReadUInt16(out this.usingCounts[(int)num2]);
			}
		}

		// Token: 0x04000F41 RID: 3905
		internal static readonly Guid msilMetaData = new Guid(3337240521U, 22963, 18902, 188, 37, 9, 2, 187, 171, 180, 96);

		// Token: 0x04000F42 RID: 3906
		internal static readonly IComparer byAddress = new PdbFunction.PdbFunctionsByAddress();

		// Token: 0x04000F43 RID: 3907
		internal static readonly IComparer byAddressAndToken = new PdbFunction.PdbFunctionsByAddressAndToken();

		// Token: 0x04000F44 RID: 3908
		internal uint token;

		// Token: 0x04000F45 RID: 3909
		internal uint slotToken;

		// Token: 0x04000F46 RID: 3910
		internal uint tokenOfMethodWhoseUsingInfoAppliesToThisMethod;

		// Token: 0x04000F47 RID: 3911
		internal uint segment;

		// Token: 0x04000F48 RID: 3912
		internal uint address;

		// Token: 0x04000F49 RID: 3913
		internal uint length;

		// Token: 0x04000F4A RID: 3914
		internal PdbScope[] scopes;

		// Token: 0x04000F4B RID: 3915
		internal PdbSlot[] slots;

		// Token: 0x04000F4C RID: 3916
		internal PdbConstant[] constants;

		// Token: 0x04000F4D RID: 3917
		internal string[] usedNamespaces;

		// Token: 0x04000F4E RID: 3918
		internal PdbLines[] lines;

		// Token: 0x04000F4F RID: 3919
		internal ushort[] usingCounts;

		// Token: 0x04000F50 RID: 3920
		internal IEnumerable<INamespaceScope> namespaceScopes;

		// Token: 0x04000F51 RID: 3921
		internal string iteratorClass;

		// Token: 0x04000F52 RID: 3922
		internal List<ILocalScope> iteratorScopes;

		// Token: 0x04000F53 RID: 3923
		internal PdbSynchronizationInformation synchronizationInformation;

		// Token: 0x04000F54 RID: 3924
		private bool visualBasicScopesAdjusted;

		// Token: 0x02000402 RID: 1026
		internal class PdbFunctionsByAddress : IComparer
		{
			// Token: 0x060015D1 RID: 5585 RVA: 0x00045D14 File Offset: 0x00043F14
			public int Compare(object x, object y)
			{
				PdbFunction pdbFunction = (PdbFunction)x;
				PdbFunction pdbFunction2 = (PdbFunction)y;
				if (pdbFunction.segment < pdbFunction2.segment)
				{
					return -1;
				}
				if (pdbFunction.segment > pdbFunction2.segment)
				{
					return 1;
				}
				if (pdbFunction.address < pdbFunction2.address)
				{
					return -1;
				}
				if (pdbFunction.address > pdbFunction2.address)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x02000403 RID: 1027
		internal class PdbFunctionsByAddressAndToken : IComparer
		{
			// Token: 0x060015D3 RID: 5587 RVA: 0x00045D70 File Offset: 0x00043F70
			public int Compare(object x, object y)
			{
				PdbFunction pdbFunction = (PdbFunction)x;
				PdbFunction pdbFunction2 = (PdbFunction)y;
				if (pdbFunction.segment < pdbFunction2.segment)
				{
					return -1;
				}
				if (pdbFunction.segment > pdbFunction2.segment)
				{
					return 1;
				}
				if (pdbFunction.address < pdbFunction2.address)
				{
					return -1;
				}
				if (pdbFunction.address > pdbFunction2.address)
				{
					return 1;
				}
				if (pdbFunction.token < pdbFunction2.token)
				{
					return -1;
				}
				if (pdbFunction.token > pdbFunction2.token)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
