using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000291 RID: 657
	internal sealed class TextMap
	{
		// Token: 0x060010CA RID: 4298 RVA: 0x000362DD File Offset: 0x000344DD
		public void AddMap(TextSegment segment, int length)
		{
			this.map[(int)segment] = new Range(this.GetStart(segment), (uint)length);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x000362F8 File Offset: 0x000344F8
		public void AddMap(TextSegment segment, int length, int align)
		{
			align--;
			this.AddMap(segment, (length + align) & ~align);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0003630C File Offset: 0x0003450C
		public void AddMap(TextSegment segment, Range range)
		{
			this.map[(int)segment] = range;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0003631B File Offset: 0x0003451B
		public Range GetRange(TextSegment segment)
		{
			return this.map[(int)segment];
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0003632C File Offset: 0x0003452C
		public DataDirectory GetDataDirectory(TextSegment segment)
		{
			Range range = this.map[(int)segment];
			return new DataDirectory((range.Length == 0U) ? 0U : range.Start, range.Length);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00036362 File Offset: 0x00034562
		public uint GetRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00036378 File Offset: 0x00034578
		public uint GetNextRVA(TextSegment segment)
		{
			return this.map[(int)segment].Start + this.map[(int)segment].Length;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x000363AA File Offset: 0x000345AA
		public int GetLength(TextSegment segment)
		{
			return (int)this.map[(int)segment].Length;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x000363C0 File Offset: 0x000345C0
		private uint GetStart(TextSegment segment)
		{
			if (segment != TextSegment.ImportAddressTable)
			{
				return this.ComputeStart((int)segment);
			}
			return 8192U;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x000363DF File Offset: 0x000345DF
		private uint ComputeStart(int index)
		{
			index--;
			return this.map[index].Start + this.map[index].Length;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0003640C File Offset: 0x0003460C
		public uint GetLength()
		{
			Range range = this.map[16];
			return range.Start - 8192U + range.Length;
		}

		// Token: 0x04000634 RID: 1588
		private readonly Range[] map = new Range[17];
	}
}
