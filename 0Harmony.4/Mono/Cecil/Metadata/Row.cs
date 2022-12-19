using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002A2 RID: 674
	internal struct Row<T1, T2>
	{
		// Token: 0x06001110 RID: 4368 RVA: 0x00036D92 File Offset: 0x00034F92
		public Row(T1 col1, T2 col2)
		{
			this.Col1 = col1;
			this.Col2 = col2;
		}

		// Token: 0x0400067E RID: 1662
		internal T1 Col1;

		// Token: 0x0400067F RID: 1663
		internal T2 Col2;
	}
}
