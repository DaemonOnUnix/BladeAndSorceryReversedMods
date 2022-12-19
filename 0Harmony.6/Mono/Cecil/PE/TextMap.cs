using System;

namespace Mono.Cecil.PE
{
	// Token: 0x0200019C RID: 412
	internal sealed class TextMap
	{
		// Token: 0x06000D67 RID: 3431 RVA: 0x0002E8B5 File Offset: 0x0002CAB5
		public void AddMap(TextSegment segment, int length)
		{
			this.map[(int)segment] = new Range(this.GetStart(segment), (uint)length);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
		public void AddMap(TextSegment segment, int length, int align)
		{
			align--;
			this.AddMap(segment, (length + align) & ~align);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002E8E4 File Offset: 0x0002CAE4
		public void AddMap(TextSegment segment, Range range)
		{
			this.map[(int)segment] = range;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002E8F3 File Offset: 0x0002CAF3
		public Range GetRange(TextSegment segment)
		{
			return this.map[(int)segment];
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002E904 File Offset: 0x0002CB04
		public DataDirectory GetDataDirectory(TextSegment segment)
		{
			Range range = this.map[(int)segment];
			return new DataDirectory((range.Length == 0U) ? 0U : range.Start, range.Length);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0002E93A File Offset: 0x0002CB3A
		public uint GetRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002E950 File Offset: 0x0002CB50
		public uint GetNextRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start + this.map[(int)segment].Length;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0002E982 File Offset: 0x0002CB82
		public int GetLength(TextSegment segment)
		{
			return (int)this.map[(int)segment].Length;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0002E998 File Offset: 0x0002CB98
		private uint GetStart(TextSegment segment)
		{
			if (segment != TextSegment.ImportAddressTable)
			{
				return this.ComputeStart((int)segment);
			}
			return 8192U;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0002E9B7 File Offset: 0x0002CBB7
		private uint ComputeStart(int index)
		{
			index--;
			return this.map[index].Start + this.map[index].Length;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002E9E4 File Offset: 0x0002CBE4
		public uint GetLength()
		{
			Range range = this.map[16];
			return range.Start - 8192U + range.Length;
		}

		// Token: 0x040005FC RID: 1532
		private readonly Range[] map = new Range[17];
	}
}
