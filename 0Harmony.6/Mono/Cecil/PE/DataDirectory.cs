using System;

namespace Mono.Cecil.PE
{
	// Token: 0x02000195 RID: 405
	internal struct DataDirectory
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0002C14C File Offset: 0x0002A34C
		public bool IsZero
		{
			get
			{
				return this.VirtualAddress == 0U && this.Size == 0U;
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0002C161 File Offset: 0x0002A361
		public DataDirectory(uint rva, uint size)
		{
			this.VirtualAddress = rva;
			this.Size = size;
		}

		// Token: 0x040005AF RID: 1455
		public readonly uint VirtualAddress;

		// Token: 0x040005B0 RID: 1456
		public readonly uint Size;
	}
}
