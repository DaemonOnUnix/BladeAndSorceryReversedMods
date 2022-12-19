using System;

namespace Mono.Cecil
{
	// Token: 0x020001BF RID: 447
	internal abstract class OneRowTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000183ED File Offset: 0x000165ED
		public sealed override int Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00018105 File Offset: 0x00016305
		public sealed override void Sort()
		{
		}

		// Token: 0x04000292 RID: 658
		internal TRow row;
	}
}
