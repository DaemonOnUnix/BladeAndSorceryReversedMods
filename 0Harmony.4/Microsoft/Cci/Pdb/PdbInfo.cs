using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000406 RID: 1030
	internal class PdbInfo
	{
		// Token: 0x04000F5B RID: 3931
		public PdbFunction[] Functions;

		// Token: 0x04000F5C RID: 3932
		public Dictionary<uint, PdbTokenLine> TokenToSourceMapping;

		// Token: 0x04000F5D RID: 3933
		public string SourceServerData;

		// Token: 0x04000F5E RID: 3934
		public int Age;

		// Token: 0x04000F5F RID: 3935
		public Guid Guid;

		// Token: 0x04000F60 RID: 3936
		public byte[] SourceLinkData;
	}
}
