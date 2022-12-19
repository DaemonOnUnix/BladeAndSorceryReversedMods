using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200030D RID: 781
	public class LineNumberTable
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x0003F102 File Offset: 0x0003D302
		public LineNumberEntry[] LineNumbers
		{
			get
			{
				return this._line_numbers;
			}
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0003F10C File Offset: 0x0003D30C
		protected LineNumberTable(MonoSymbolFile file)
		{
			this.LineBase = file.OffsetTable.LineNumberTable_LineBase;
			this.LineRange = file.OffsetTable.LineNumberTable_LineRange;
			this.OpcodeBase = (byte)file.OffsetTable.LineNumberTable_OpcodeBase;
			this.MaxAddressIncrement = (int)(byte.MaxValue - this.OpcodeBase) / this.LineRange;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0003F16C File Offset: 0x0003D36C
		internal LineNumberTable(MonoSymbolFile file, LineNumberEntry[] lines)
			: this(file)
		{
			this._line_numbers = lines;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0003F17C File Offset: 0x0003D37C
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw, bool hasColumnsInfo, bool hasEndInfo)
		{
			int num = (int)bw.BaseStream.Position;
			bool flag = false;
			int num2 = 1;
			int num3 = 0;
			int num4 = 1;
			for (int i = 0; i < this.LineNumbers.Length; i++)
			{
				int num5 = this.LineNumbers[i].Row - num2;
				int num6 = this.LineNumbers[i].Offset - num3;
				if (this.LineNumbers[i].File != num4)
				{
					bw.Write(4);
					bw.WriteLeb128(this.LineNumbers[i].File);
					num4 = this.LineNumbers[i].File;
				}
				if (this.LineNumbers[i].IsHidden != flag)
				{
					bw.Write(0);
					bw.Write(1);
					bw.Write(64);
					flag = this.LineNumbers[i].IsHidden;
				}
				if (num6 >= this.MaxAddressIncrement)
				{
					if (num6 < 2 * this.MaxAddressIncrement)
					{
						bw.Write(8);
						num6 -= this.MaxAddressIncrement;
					}
					else
					{
						bw.Write(2);
						bw.WriteLeb128(num6);
						num6 = 0;
					}
				}
				if (num5 < this.LineBase || num5 >= this.LineBase + this.LineRange)
				{
					bw.Write(3);
					bw.WriteLeb128(num5);
					if (num6 != 0)
					{
						bw.Write(2);
						bw.WriteLeb128(num6);
					}
					bw.Write(1);
				}
				else
				{
					byte b = (byte)(num5 - this.LineBase + this.LineRange * num6 + (int)this.OpcodeBase);
					bw.Write(b);
				}
				num2 = this.LineNumbers[i].Row;
				num3 = this.LineNumbers[i].Offset;
			}
			bw.Write(0);
			bw.Write(1);
			bw.Write(1);
			if (hasColumnsInfo)
			{
				for (int j = 0; j < this.LineNumbers.Length; j++)
				{
					LineNumberEntry lineNumberEntry = this.LineNumbers[j];
					if (lineNumberEntry.Row >= 0)
					{
						bw.WriteLeb128(lineNumberEntry.Column);
					}
				}
			}
			if (hasEndInfo)
			{
				for (int k = 0; k < this.LineNumbers.Length; k++)
				{
					LineNumberEntry lineNumberEntry2 = this.LineNumbers[k];
					if (lineNumberEntry2.EndRow == -1 || lineNumberEntry2.EndColumn == -1 || lineNumberEntry2.Row > lineNumberEntry2.EndRow)
					{
						bw.WriteLeb128(16777215);
					}
					else
					{
						bw.WriteLeb128(lineNumberEntry2.EndRow - lineNumberEntry2.Row);
						bw.WriteLeb128(lineNumberEntry2.EndColumn);
					}
				}
			}
			file.ExtendedLineNumberSize += (int)bw.BaseStream.Position - num;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0003F3FD File Offset: 0x0003D5FD
		internal static LineNumberTable Read(MonoSymbolFile file, MyBinaryReader br, bool readColumnsInfo, bool readEndInfo)
		{
			LineNumberTable lineNumberTable = new LineNumberTable(file);
			lineNumberTable.DoRead(file, br, readColumnsInfo, readEndInfo);
			return lineNumberTable;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0003F410 File Offset: 0x0003D610
		private void DoRead(MonoSymbolFile file, MyBinaryReader br, bool includesColumns, bool includesEnds)
		{
			List<LineNumberEntry> list = new List<LineNumberEntry>();
			bool flag = false;
			bool flag2 = false;
			int num = 1;
			int num2 = 0;
			int num3 = 1;
			byte b;
			for (;;)
			{
				b = br.ReadByte();
				if (b == 0)
				{
					byte b2 = br.ReadByte();
					long num4 = br.BaseStream.Position + (long)((ulong)b2);
					b = br.ReadByte();
					if (b == 1)
					{
						break;
					}
					if (b == 64)
					{
						flag = !flag;
						flag2 = true;
					}
					else if (b < 64 || b > 127)
					{
						goto IL_7F;
					}
					br.BaseStream.Position = num4;
				}
				else
				{
					if (b < this.OpcodeBase)
					{
						switch (b)
						{
						case 1:
							list.Add(new LineNumberEntry(num3, num, -1, num2, flag));
							flag2 = false;
							continue;
						case 2:
							num2 += br.ReadLeb128();
							flag2 = true;
							continue;
						case 3:
							num += br.ReadLeb128();
							flag2 = true;
							continue;
						case 4:
							num3 = br.ReadLeb128();
							flag2 = true;
							continue;
						case 8:
							num2 += this.MaxAddressIncrement;
							flag2 = true;
							continue;
						}
						goto Block_7;
					}
					b -= this.OpcodeBase;
					num2 += (int)b / this.LineRange;
					num += this.LineBase + (int)b % this.LineRange;
					list.Add(new LineNumberEntry(num3, num, -1, num2, flag));
					flag2 = false;
				}
			}
			if (flag2)
			{
				list.Add(new LineNumberEntry(num3, num, -1, num2, flag));
			}
			this._line_numbers = list.ToArray();
			if (includesColumns)
			{
				for (int i = 0; i < this._line_numbers.Length; i++)
				{
					LineNumberEntry lineNumberEntry = this._line_numbers[i];
					if (lineNumberEntry.Row >= 0)
					{
						lineNumberEntry.Column = br.ReadLeb128();
					}
				}
			}
			if (includesEnds)
			{
				for (int j = 0; j < this._line_numbers.Length; j++)
				{
					LineNumberEntry lineNumberEntry2 = this._line_numbers[j];
					int num5 = br.ReadLeb128();
					if (num5 == 16777215)
					{
						lineNumberEntry2.EndRow = -1;
						lineNumberEntry2.EndColumn = -1;
					}
					else
					{
						lineNumberEntry2.EndRow = lineNumberEntry2.Row + num5;
						lineNumberEntry2.EndColumn = br.ReadLeb128();
					}
				}
			}
			return;
			IL_7F:
			throw new MonoSymbolFileException("Unknown extended opcode {0:x}", new object[] { b });
			Block_7:
			throw new MonoSymbolFileException("Unknown standard opcode {0:x} in LNT", new object[] { b });
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0003F66D File Offset: 0x0003D86D
		public bool GetMethodBounds(out LineNumberEntry start, out LineNumberEntry end)
		{
			if (this._line_numbers.Length > 1)
			{
				start = this._line_numbers[0];
				end = this._line_numbers[this._line_numbers.Length - 1];
				return true;
			}
			start = LineNumberEntry.Null;
			end = LineNumberEntry.Null;
			return false;
		}

		// Token: 0x04000A11 RID: 2577
		protected LineNumberEntry[] _line_numbers;

		// Token: 0x04000A12 RID: 2578
		public readonly int LineBase;

		// Token: 0x04000A13 RID: 2579
		public readonly int LineRange;

		// Token: 0x04000A14 RID: 2580
		public readonly byte OpcodeBase;

		// Token: 0x04000A15 RID: 2581
		public readonly int MaxAddressIncrement;

		// Token: 0x04000A16 RID: 2582
		public const int Default_LineBase = -1;

		// Token: 0x04000A17 RID: 2583
		public const int Default_LineRange = 8;

		// Token: 0x04000A18 RID: 2584
		public const byte Default_OpcodeBase = 9;

		// Token: 0x04000A19 RID: 2585
		public const byte DW_LNS_copy = 1;

		// Token: 0x04000A1A RID: 2586
		public const byte DW_LNS_advance_pc = 2;

		// Token: 0x04000A1B RID: 2587
		public const byte DW_LNS_advance_line = 3;

		// Token: 0x04000A1C RID: 2588
		public const byte DW_LNS_set_file = 4;

		// Token: 0x04000A1D RID: 2589
		public const byte DW_LNS_const_add_pc = 8;

		// Token: 0x04000A1E RID: 2590
		public const byte DW_LNE_end_sequence = 1;

		// Token: 0x04000A1F RID: 2591
		public const byte DW_LNE_MONO_negate_is_hidden = 64;

		// Token: 0x04000A20 RID: 2592
		internal const byte DW_LNE_MONO__extensions_start = 64;

		// Token: 0x04000A21 RID: 2593
		internal const byte DW_LNE_MONO__extensions_end = 127;
	}
}
