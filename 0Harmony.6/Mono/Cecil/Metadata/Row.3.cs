using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AF RID: 431
	internal struct Row<T1, T2, T3, T4>
	{
		// Token: 0x06000DAF RID: 3503 RVA: 0x0002F391 File Offset: 0x0002D591
		public Row(T1 col1, T2 col2, T3 col3, T4 col4)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
		}

		// Token: 0x0400064B RID: 1611
		internal T1 Col1;

		// Token: 0x0400064C RID: 1612
		internal T2 Col2;

		// Token: 0x0400064D RID: 1613
		internal T3 Col3;

		// Token: 0x0400064E RID: 1614
		internal T4 Col4;
	}
}
