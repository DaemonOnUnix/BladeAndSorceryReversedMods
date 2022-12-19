using System;

namespace Mono.Cecil.PE
{
	// Token: 0x0200028A RID: 650
	internal struct DataDirectory
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00033B2C File Offset: 0x00031D2C
		public bool IsZero
		{
			get
			{
				return this.VirtualAddress == 0U && this.Size == 0U;
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00033B41 File Offset: 0x00031D41
		public DataDirectory(uint rva, uint size)
		{
			this.VirtualAddress = rva;
			this.Size = size;
		}

		// Token: 0x040005E6 RID: 1510
		public readonly uint VirtualAddress;

		// Token: 0x040005E7 RID: 1511
		public readonly uint Size;
	}
}
