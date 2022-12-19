using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020001BE RID: 446
	internal abstract class MetadataTable
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060008EE RID: 2286
		public abstract int Length { get; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0002070A File Offset: 0x0001E90A
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535;
			}
		}

		// Token: 0x060008F0 RID: 2288
		public abstract void Write(TableHeapBuffer buffer);

		// Token: 0x060008F1 RID: 2289
		public abstract void Sort();
	}
}
