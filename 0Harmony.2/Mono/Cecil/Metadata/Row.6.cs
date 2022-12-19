using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A7 RID: 679
	internal struct Row<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x00036E30 File Offset: 0x00035030
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

		// Token: 0x04000692 RID: 1682
		internal T1 Col1;

		// Token: 0x04000693 RID: 1683
		internal T2 Col2;

		// Token: 0x04000694 RID: 1684
		internal T3 Col3;

		// Token: 0x04000695 RID: 1685
		internal T4 Col4;

		// Token: 0x04000696 RID: 1686
		internal T5 Col5;

		// Token: 0x04000697 RID: 1687
		internal T6 Col6;

		// Token: 0x04000698 RID: 1688
		internal T7 Col7;

		// Token: 0x04000699 RID: 1689
		internal T8 Col8;

		// Token: 0x0400069A RID: 1690
		internal T9 Col9;
	}
}
