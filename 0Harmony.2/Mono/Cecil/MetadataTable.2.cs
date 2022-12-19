using System;

namespace Mono.Cecil
{
	// Token: 0x020001C0 RID: 448
	internal abstract class MetadataTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00020721 File Offset: 0x0001E921
		public sealed override int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002072C File Offset: 0x0001E92C
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

		// Token: 0x060008F8 RID: 2296 RVA: 0x00020774 File Offset: 0x0001E974
		private void Grow()
		{
			TRow[] array = new TRow[this.rows.Length * 2];
			Array.Copy(this.rows, array, this.rows.Length);
			this.rows = array;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00018105 File Offset: 0x00016305
		public override void Sort()
		{
		}

		// Token: 0x04000293 RID: 659
		internal TRow[] rows = new TRow[2];

		// Token: 0x04000294 RID: 660
		internal int length;
	}
}
