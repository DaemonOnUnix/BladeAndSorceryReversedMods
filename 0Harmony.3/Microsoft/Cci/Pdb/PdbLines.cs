using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000312 RID: 786
	internal class PdbLines
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0003DF68 File Offset: 0x0003C168
		internal PdbLines(PdbSource file, uint count)
		{
			this.file = file;
			this.lines = new PdbLine[count];
		}

		// Token: 0x04000F28 RID: 3880
		internal PdbSource file;

		// Token: 0x04000F29 RID: 3881
		internal PdbLine[] lines;
	}
}
