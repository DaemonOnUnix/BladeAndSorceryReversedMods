using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F7 RID: 1015
	internal interface IUsedNamespace
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060015A0 RID: 5536
		IName Alias { get; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060015A1 RID: 5537
		IName NamespaceName { get; }
	}
}
