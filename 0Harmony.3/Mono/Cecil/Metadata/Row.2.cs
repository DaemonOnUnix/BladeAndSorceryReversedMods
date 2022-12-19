using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AE RID: 430
	internal struct Row<T1, T2, T3>
	{
		// Token: 0x06000DAE RID: 3502 RVA: 0x0002F37A File Offset: 0x0002D57A
		public Row(T1 col1, T2 col2, T3 col3)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
		}

		// Token: 0x04000648 RID: 1608
		internal T1 Col1;

		// Token: 0x04000649 RID: 1609
		internal T2 Col2;

		// Token: 0x0400064A RID: 1610
		internal T3 Col3;
	}
}
