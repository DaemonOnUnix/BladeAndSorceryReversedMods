using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020B RID: 523
	public class LineNumberEntry
	{
		// Token: 0x06000FDD RID: 4061 RVA: 0x000364EF File Offset: 0x000346EF
		public LineNumberEntry(int file, int row, int column, int offset)
			: this(file, row, column, offset, false)
		{
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000364FD File Offset: 0x000346FD
		public LineNumberEntry(int file, int row, int offset)
			: this(file, row, -1, offset, false)
		{
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0003650A File Offset: 0x0003470A
		public LineNumberEntry(int file, int row, int column, int offset, bool is_hidden)
			: this(file, row, column, -1, -1, offset, is_hidden)
		{
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003651B File Offset: 0x0003471B
		public LineNumberEntry(int file, int row, int column, int end_row, int end_column, int offset, bool is_hidden)
		{
			this.File = file;
			this.Row = row;
			this.Column = column;
			this.EndRow = end_row;
			this.EndColumn = end_column;
			this.Offset = offset;
			this.IsHidden = is_hidden;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00036558 File Offset: 0x00034758
		public override string ToString()
		{
			return string.Format("[Line {0}:{1},{2}-{3},{4}:{5}]", new object[] { this.File, this.Row, this.Column, this.EndRow, this.EndColumn, this.Offset });
		}

		// Token: 0x0400099E RID: 2462
		public readonly int Row;

		// Token: 0x0400099F RID: 2463
		public int Column;

		// Token: 0x040009A0 RID: 2464
		public int EndRow;

		// Token: 0x040009A1 RID: 2465
		public int EndColumn;

		// Token: 0x040009A2 RID: 2466
		public readonly int File;

		// Token: 0x040009A3 RID: 2467
		public readonly int Offset;

		// Token: 0x040009A4 RID: 2468
		public readonly bool IsHidden;

		// Token: 0x040009A5 RID: 2469
		public static readonly LineNumberEntry Null = new LineNumberEntry(0, 0, 0, 0);

		// Token: 0x0200020C RID: 524
		public sealed class LocationComparer : IComparer<LineNumberEntry>
		{
			// Token: 0x06000FE3 RID: 4067 RVA: 0x000365DC File Offset: 0x000347DC
			public int Compare(LineNumberEntry l1, LineNumberEntry l2)
			{
				if (l1.Row != l2.Row)
				{
					return l1.Row.CompareTo(l2.Row);
				}
				return l1.Column.CompareTo(l2.Column);
			}

			// Token: 0x040009A6 RID: 2470
			public static readonly LineNumberEntry.LocationComparer Default = new LineNumberEntry.LocationComparer();
		}
	}
}
