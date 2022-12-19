using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000301 RID: 769
	public class LineNumberEntry
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x0003E43B File Offset: 0x0003C63B
		public LineNumberEntry(int file, int row, int column, int offset)
			: this(file, row, column, offset, false)
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0003E449 File Offset: 0x0003C649
		public LineNumberEntry(int file, int row, int offset)
			: this(file, row, -1, offset, false)
		{
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0003E456 File Offset: 0x0003C656
		public LineNumberEntry(int file, int row, int column, int offset, bool is_hidden)
			: this(file, row, column, -1, -1, offset, is_hidden)
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0003E467 File Offset: 0x0003C667
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

		// Token: 0x06001351 RID: 4945 RVA: 0x0003E4A4 File Offset: 0x0003C6A4
		public override string ToString()
		{
			return string.Format("[Line {0}:{1},{2}-{3},{4}:{5}]", new object[] { this.File, this.Row, this.Column, this.EndRow, this.EndColumn, this.Offset });
		}

		// Token: 0x040009DD RID: 2525
		public readonly int Row;

		// Token: 0x040009DE RID: 2526
		public int Column;

		// Token: 0x040009DF RID: 2527
		public int EndRow;

		// Token: 0x040009E0 RID: 2528
		public int EndColumn;

		// Token: 0x040009E1 RID: 2529
		public readonly int File;

		// Token: 0x040009E2 RID: 2530
		public readonly int Offset;

		// Token: 0x040009E3 RID: 2531
		public readonly bool IsHidden;

		// Token: 0x040009E4 RID: 2532
		public static readonly LineNumberEntry Null = new LineNumberEntry(0, 0, 0, 0);

		// Token: 0x02000302 RID: 770
		public sealed class LocationComparer : IComparer<LineNumberEntry>
		{
			// Token: 0x06001353 RID: 4947 RVA: 0x0003E528 File Offset: 0x0003C728
			public int Compare(LineNumberEntry l1, LineNumberEntry l2)
			{
				if (l1.Row != l2.Row)
				{
					return l1.Row.CompareTo(l2.Row);
				}
				return l1.Column.CompareTo(l2.Column);
			}

			// Token: 0x040009E5 RID: 2533
			public static readonly LineNumberEntry.LocationComparer Default = new LineNumberEntry.LocationComparer();
		}
	}
}
