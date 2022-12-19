using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A4 RID: 676
	internal struct Row<T1, T2, T3, T4>
	{
		// Token: 0x06001112 RID: 4370 RVA: 0x00036DB9 File Offset: 0x00034FB9
		public Row(T1 col1, T2 col2, T3 col3, T4 col4)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
		}

		// Token: 0x04000683 RID: 1667
		internal T1 Col1;

		// Token: 0x04000684 RID: 1668
		internal T2 Col2;

		// Token: 0x04000685 RID: 1669
		internal T3 Col3;

		// Token: 0x04000686 RID: 1670
		internal T4 Col4;
	}
}
