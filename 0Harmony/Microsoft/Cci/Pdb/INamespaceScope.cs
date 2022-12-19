using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F6 RID: 1014
	internal interface INamespaceScope
	{
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600159F RID: 5535
		IEnumerable<IUsedNamespace> UsedNamespaces { get; }
	}
}
