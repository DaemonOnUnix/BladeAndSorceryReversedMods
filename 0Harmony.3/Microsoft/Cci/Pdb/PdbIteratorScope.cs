using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000318 RID: 792
	internal sealed class PdbIteratorScope : ILocalScope
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x0003E397 File Offset: 0x0003C597
		internal PdbIteratorScope(uint offset, uint length)
		{
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0003E3AD File Offset: 0x0003C5AD
		public uint Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x0003E3B5 File Offset: 0x0003C5B5
		public uint Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x04000F45 RID: 3909
		private uint offset;

		// Token: 0x04000F46 RID: 3910
		private uint length;
	}
}
