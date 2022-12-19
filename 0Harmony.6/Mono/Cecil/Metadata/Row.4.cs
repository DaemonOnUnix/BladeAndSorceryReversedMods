using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B0 RID: 432
	internal struct Row<T1, T2, T3, T4, T5>
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002F3B0 File Offset: 0x0002D5B0
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
		}

		// Token: 0x0400064F RID: 1615
		internal T1 Col1;

		// Token: 0x04000650 RID: 1616
		internal T2 Col2;

		// Token: 0x04000651 RID: 1617
		internal T3 Col3;

		// Token: 0x04000652 RID: 1618
		internal T4 Col4;

		// Token: 0x04000653 RID: 1619
		internal T5 Col5;
	}
}
