using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200040D RID: 1037
	internal class PdbTokenLine
	{
		// Token: 0x060015E7 RID: 5607 RVA: 0x000462AA File Offset: 0x000444AA
		internal PdbTokenLine(uint token, uint file_id, uint line, uint column, uint endLine, uint endColumn)
		{
			this.token = token;
			this.file_id = file_id;
			this.line = line;
			this.column = column;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		// Token: 0x04000F7B RID: 3963
		internal uint token;

		// Token: 0x04000F7C RID: 3964
		internal uint file_id;

		// Token: 0x04000F7D RID: 3965
		internal uint line;

		// Token: 0x04000F7E RID: 3966
		internal uint column;

		// Token: 0x04000F7F RID: 3967
		internal uint endLine;

		// Token: 0x04000F80 RID: 3968
		internal uint endColumn;

		// Token: 0x04000F81 RID: 3969
		internal PdbSource sourceFile;

		// Token: 0x04000F82 RID: 3970
		internal PdbTokenLine nextLine;
	}
}
