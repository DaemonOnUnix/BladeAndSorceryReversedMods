using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000408 RID: 1032
	internal class PdbLines
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x00045EB0 File Offset: 0x000440B0
		internal PdbLines(PdbSource file, uint count)
		{
			this.file = file;
			this.lines = new PdbLine[count];
		}

		// Token: 0x04000F66 RID: 3942
		internal PdbSource file;

		// Token: 0x04000F67 RID: 3943
		internal PdbLine[] lines;
	}
}
