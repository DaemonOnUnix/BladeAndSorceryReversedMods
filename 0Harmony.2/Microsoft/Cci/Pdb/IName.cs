using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F8 RID: 1016
	internal interface IName
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060015A2 RID: 5538
		int UniqueKey { get; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060015A3 RID: 5539
		int UniqueKeyIgnoringCase { get; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060015A4 RID: 5540
		string Value { get; }
	}
}
