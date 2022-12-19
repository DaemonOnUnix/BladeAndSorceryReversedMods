using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AD RID: 429
	internal struct Row<T1, T2>
	{
		// Token: 0x06000DAD RID: 3501 RVA: 0x0002F36A File Offset: 0x0002D56A
		public Row(T1 col1, T2 col2)
		{
			this.Col1 = col1;
			this.Col2 = col2;
		}

		// Token: 0x04000646 RID: 1606
		internal T1 Col1;

		// Token: 0x04000647 RID: 1607
		internal T2 Col2;
	}
}
