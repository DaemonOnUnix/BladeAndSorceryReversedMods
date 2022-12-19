using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B1 RID: 433
	internal struct Row<T1, T2, T3, T4, T5, T6>
	{
		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002F3D7 File Offset: 0x0002D5D7
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
		}

		// Token: 0x04000654 RID: 1620
		internal T1 Col1;

		// Token: 0x04000655 RID: 1621
		internal T2 Col2;

		// Token: 0x04000656 RID: 1622
		internal T3 Col3;

		// Token: 0x04000657 RID: 1623
		internal T4 Col4;

		// Token: 0x04000658 RID: 1624
		internal T5 Col5;

		// Token: 0x04000659 RID: 1625
		internal T6 Col6;
	}
}
