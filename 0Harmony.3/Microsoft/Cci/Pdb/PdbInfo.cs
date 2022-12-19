using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000310 RID: 784
	internal class PdbInfo
	{
		// Token: 0x04000F1D RID: 3869
		public PdbFunction[] Functions;

		// Token: 0x04000F1E RID: 3870
		public Dictionary<uint, PdbTokenLine> TokenToSourceMapping;

		// Token: 0x04000F1F RID: 3871
		public string SourceServerData;

		// Token: 0x04000F20 RID: 3872
		public int Age;

		// Token: 0x04000F21 RID: 3873
		public Guid Guid;

		// Token: 0x04000F22 RID: 3874
		public byte[] SourceLinkData;
	}
}
