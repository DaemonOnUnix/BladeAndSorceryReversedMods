using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000317 RID: 791
	internal class PdbTokenLine
	{
		// Token: 0x06001278 RID: 4728 RVA: 0x0003E362 File Offset: 0x0003C562
		internal PdbTokenLine(uint token, uint file_id, uint line, uint column, uint endLine, uint endColumn)
		{
			this.token = token;
			this.file_id = file_id;
			this.line = line;
			this.column = column;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		// Token: 0x04000F3D RID: 3901
		internal uint token;

		// Token: 0x04000F3E RID: 3902
		internal uint file_id;

		// Token: 0x04000F3F RID: 3903
		internal uint line;

		// Token: 0x04000F40 RID: 3904
		internal uint column;

		// Token: 0x04000F41 RID: 3905
		internal uint endLine;

		// Token: 0x04000F42 RID: 3906
		internal uint endColumn;

		// Token: 0x04000F43 RID: 3907
		internal PdbSource sourceFile;

		// Token: 0x04000F44 RID: 3908
		internal PdbTokenLine nextLine;
	}
}
