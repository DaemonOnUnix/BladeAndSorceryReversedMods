using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A3 RID: 675
	internal struct Row<T1, T2, T3>
	{
		// Token: 0x06001111 RID: 4369 RVA: 0x00036DA2 File Offset: 0x00034FA2
		public Row(T1 col1, T2 col2, T3 col3)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
		}

		// Token: 0x04000680 RID: 1664
		internal T1 Col1;

		// Token: 0x04000681 RID: 1665
		internal T2 Col2;

		// Token: 0x04000682 RID: 1666
		internal T3 Col3;
	}
}
