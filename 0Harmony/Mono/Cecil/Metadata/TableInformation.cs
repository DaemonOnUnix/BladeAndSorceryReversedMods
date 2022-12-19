using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020002AB RID: 683
	internal struct TableInformation
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x00036FFD File Offset: 0x000351FD
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535U;
			}
		}

		// Token: 0x040006D2 RID: 1746
		public uint Offset;

		// Token: 0x040006D3 RID: 1747
		public uint Length;

		// Token: 0x040006D4 RID: 1748
		public uint RowSize;
	}
}
