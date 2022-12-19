using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000300 RID: 768
	internal interface INamespaceScope
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001230 RID: 4656
		IEnumerable<IUsedNamespace> UsedNamespaces { get; }
	}
}
