using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200040E RID: 1038
	internal sealed class PdbIteratorScope : ILocalScope
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x000462DF File Offset: 0x000444DF
		internal PdbIteratorScope(uint offset, uint length)
		{
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x000462F5 File Offset: 0x000444F5
		public uint Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x000462FD File Offset: 0x000444FD
		public uint Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x04000F83 RID: 3971
		private uint offset;

		// Token: 0x04000F84 RID: 3972
		private uint length;
	}
}
