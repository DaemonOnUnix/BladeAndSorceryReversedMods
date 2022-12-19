using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A6 RID: 678
	internal struct Row<T1, T2, T3, T4, T5, T6>
	{
		// Token: 0x06001114 RID: 4372 RVA: 0x00036DFF File Offset: 0x00034FFF
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
		}

		// Token: 0x0400068C RID: 1676
		internal T1 Col1;

		// Token: 0x0400068D RID: 1677
		internal T2 Col2;

		// Token: 0x0400068E RID: 1678
		internal T3 Col3;

		// Token: 0x0400068F RID: 1679
		internal T4 Col4;

		// Token: 0x04000690 RID: 1680
		internal T5 Col5;

		// Token: 0x04000691 RID: 1681
		internal T6 Col6;
	}
}
