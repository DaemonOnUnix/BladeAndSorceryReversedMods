using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200030B RID: 779
	internal class PdbFunction
	{
		// Token: 0x06001254 RID: 4692 RVA: 0x0003D4BC File Offset: 0x0003B6BC
		private static string StripNamespace(string module)
		{
			int num = module.LastIndexOf('.');
			if (num > 0)
			{
				return module.Substring(num + 1);
			}
			return module;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0003D4E4 File Offset: 0x0003B6E4
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

		// Token: 0x06001256 RID: 4694 RVA: 0x0003D528 File Offset: 0x0003B728
		private void AdjustVisualBasicScopes(PdbScope[] scopes)
		{
			foreach (PdbScope pdbScope in scopes)
			{
				pdbScope.length += 1U;
				this.AdjustVisualBasicScopes(pdbScope.scopes);
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0003D564 File Offset: 0x0003B764
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

		// Token: 0x06001258 RID: 4696 RVA: 0x0003D734 File Offset: 0x0003B934
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

		// Token: 0x06001259 RID: 4697 RVA: 0x00002AED File Offset: 0x00000CED
		internal PdbFunction()
		{
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0003D838 File Offset: 0x0003BA38
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

		// Token: 0x0600125B RID: 4699 RVA: 0x0003DBF8 File Offset: 0x0003BDF8
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

		// Token: 0x0600125C RID: 4700 RVA: 0x0003DC34 File Offset: 0x0003BE34
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

		// Token: 0x0600125D RID: 4701 RVA: 0x0003DCC5 File Offset: 0x0003BEC5
		private void ReadForwardIterator(BitAccess bits)
		{
			this.iteratorClass = bits.ReadString();
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0003DCD4 File Offset: 0x0003BED4
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

		// Token: 0x0600125F RID: 4703 RVA: 0x0003DD23 File Offset: 0x0003BF23
		private void ReadForwardInfo(BitAccess bits)
		{
			bits.ReadUInt32(out this.tokenOfMethodWhoseUsingInfoAppliesToThisMethod);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0003DD34 File Offset: 0x0003BF34
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

		// Token: 0x04000F03 RID: 3843
		internal static readonly Guid msilMetaData = new Guid(3337240521U, 22963, 18902, 188, 37, 9, 2, 187, 171, 180, 96);

		// Token: 0x04000F04 RID: 3844
		internal static readonly IComparer byAddress = new PdbFunction.PdbFunctionsByAddress();

		// Token: 0x04000F05 RID: 3845
		internal static readonly IComparer byAddressAndToken = new PdbFunction.PdbFunctionsByAddressAndToken();

		// Token: 0x04000F06 RID: 3846
		internal uint token;

		// Token: 0x04000F07 RID: 3847
		internal uint slotToken;

		// Token: 0x04000F08 RID: 3848
		internal uint tokenOfMethodWhoseUsingInfoAppliesToThisMethod;

		// Token: 0x04000F09 RID: 3849
		internal uint segment;

		// Token: 0x04000F0A RID: 3850
		internal uint address;

		// Token: 0x04000F0B RID: 3851
		internal uint length;

		// Token: 0x04000F0C RID: 3852
		internal PdbScope[] scopes;

		// Token: 0x04000F0D RID: 3853
		internal PdbSlot[] slots;

		// Token: 0x04000F0E RID: 3854
		internal PdbConstant[] constants;

		// Token: 0x04000F0F RID: 3855
		internal string[] usedNamespaces;

		// Token: 0x04000F10 RID: 3856
		internal PdbLines[] lines;

		// Token: 0x04000F11 RID: 3857
		internal ushort[] usingCounts;

		// Token: 0x04000F12 RID: 3858
		internal IEnumerable<INamespaceScope> namespaceScopes;

		// Token: 0x04000F13 RID: 3859
		internal string iteratorClass;

		// Token: 0x04000F14 RID: 3860
		internal List<ILocalScope> iteratorScopes;

		// Token: 0x04000F15 RID: 3861
		internal PdbSynchronizationInformation synchronizationInformation;

		// Token: 0x04000F16 RID: 3862
		private bool visualBasicScopesAdjusted;

		// Token: 0x0200030C RID: 780
		internal class PdbFunctionsByAddress : IComparer
		{
			// Token: 0x06001262 RID: 4706 RVA: 0x0003DDCC File Offset: 0x0003BFCC
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

		// Token: 0x0200030D RID: 781
		internal class PdbFunctionsByAddressAndToken : IComparer
		{
			// Token: 0x06001264 RID: 4708 RVA: 0x0003DE28 File Offset: 0x0003C028
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
