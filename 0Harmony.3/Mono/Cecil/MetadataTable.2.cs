using System;

namespace Mono.Cecil
{
	// Token: 0x020000CE RID: 206
	internal abstract class MetadataTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001A88D File Offset: 0x00018A8D
		public sealed override int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001A898 File Offset: 0x00018A98
		public int AddRow(TRow row)
		{
			if (this.rows.Length == this.length)
			{
				this.Grow();
			}
			TRow[] array = this.rows;
			int num = this.length;
			this.length = num + 1;
			array[num] = row;
			return this.length;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001A8E0 File Offset: 0x00018AE0
		private void Grow()
		{
			TRow[] array = new TRow[this.rows.Length * 2];
			Array.Copy(this.rows, array, this.rows.Length);
			this.rows = array;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00012279 File Offset: 0x00010479
		public override void Sort()
		{
		}

		// Token: 0x04000261 RID: 609
		internal TRow[] rows = new TRow[2];

		// Token: 0x04000262 RID: 610
		internal int length;
	}
}
