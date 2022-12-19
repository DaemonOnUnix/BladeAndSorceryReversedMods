using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x0200040C RID: 1036
	internal class PdbSource
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x00046275 File Offset: 0x00044475
		internal PdbSource(string name, Guid doctype, Guid language, Guid vendor, Guid checksumAlgorithm, byte[] checksum)
		{
			this.name = name;
			this.doctype = doctype;
			this.language = language;
			this.vendor = vendor;
			this.checksumAlgorithm = checksumAlgorithm;
			this.checksum = checksum;
		}

		// Token: 0x04000F75 RID: 3957
		internal string name;

		// Token: 0x04000F76 RID: 3958
		internal Guid doctype;

		// Token: 0x04000F77 RID: 3959
		internal Guid language;

		// Token: 0x04000F78 RID: 3960
		internal Guid vendor;

		// Token: 0x04000F79 RID: 3961
		internal Guid checksumAlgorithm;

		// Token: 0x04000F7A RID: 3962
		internal byte[] checksum;
	}
}
