using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000CC RID: 204
	internal abstract class MetadataTable
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060005B6 RID: 1462
		public abstract int Length { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001A876 File Offset: 0x00018A76
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535;
			}
		}

		// Token: 0x060005B8 RID: 1464
		public abstract void Write(TableHeapBuffer buffer);

		// Token: 0x060005B9 RID: 1465
		public abstract void Sort();
	}
}
