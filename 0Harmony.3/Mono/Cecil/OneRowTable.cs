using System;

namespace Mono.Cecil
{
	// Token: 0x020000CD RID: 205
	internal abstract class OneRowTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00012561 File Offset: 0x00010761
		public sealed override int Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00012279 File Offset: 0x00010479
		public sealed override void Sort()
		{
		}

		// Token: 0x04000260 RID: 608
		internal TRow row;
	}
}
