using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B6 RID: 438
	internal struct TableInformation
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0002F5D5 File Offset: 0x0002D7D5
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535U;
			}
		}

		// Token: 0x0400069A RID: 1690
		public uint Offset;

		// Token: 0x0400069B RID: 1691
		public uint Length;

		// Token: 0x0400069C RID: 1692
		public uint RowSize;
	}
}
