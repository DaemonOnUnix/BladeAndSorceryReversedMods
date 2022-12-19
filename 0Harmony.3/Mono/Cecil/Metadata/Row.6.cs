using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B2 RID: 434
	internal struct Row<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002F408 File Offset: 0x0002D608
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6, T7 col7, T8 col8, T9 col9)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
			this.Col7 = col7;
			this.Col8 = col8;
			this.Col9 = col9;
		}

		// Token: 0x0400065A RID: 1626
		internal T1 Col1;

		// Token: 0x0400065B RID: 1627
		internal T2 Col2;

		// Token: 0x0400065C RID: 1628
		internal T3 Col3;

		// Token: 0x0400065D RID: 1629
		internal T4 Col4;

		// Token: 0x0400065E RID: 1630
		internal T5 Col5;

		// Token: 0x0400065F RID: 1631
		internal T6 Col6;

		// Token: 0x04000660 RID: 1632
		internal T7 Col7;

		// Token: 0x04000661 RID: 1633
		internal T8 Col8;

		// Token: 0x04000662 RID: 1634
		internal T9 Col9;
	}
}
