using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000316 RID: 790
	internal class PdbSource
	{
		// Token: 0x06001277 RID: 4727 RVA: 0x0003E32D File Offset: 0x0003C52D
		internal PdbSource(string name, Guid doctype, Guid language, Guid vendor, Guid checksumAlgorithm, byte[] checksum)
		{
			this.name = name;
			this.doctype = doctype;
			this.language = language;
			this.vendor = vendor;
			this.checksumAlgorithm = checksumAlgorithm;
			this.checksum = checksum;
		}

		// Token: 0x04000F37 RID: 3895
		internal string name;

		// Token: 0x04000F38 RID: 3896
		internal Guid doctype;

		// Token: 0x04000F39 RID: 3897
		internal Guid language;

		// Token: 0x04000F3A RID: 3898
		internal Guid vendor;

		// Token: 0x04000F3B RID: 3899
		internal Guid checksumAlgorithm;

		// Token: 0x04000F3C RID: 3900
		internal byte[] checksum;
	}
}
