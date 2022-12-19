using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A5 RID: 677
	internal struct Row<T1, T2, T3, T4, T5>
	{
		// Token: 0x06001113 RID: 4371 RVA: 0x00036DD8 File Offset: 0x00034FD8
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
		}

		// Token: 0x04000687 RID: 1671
		internal T1 Col1;

		// Token: 0x04000688 RID: 1672
		internal T2 Col2;

		// Token: 0x04000689 RID: 1673
		internal T3 Col3;

		// Token: 0x0400068A RID: 1674
		internal T4 Col4;

		// Token: 0x0400068B RID: 1675
		internal T5 Col5;
	}
}
