using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000301 RID: 769
	internal interface IUsedNamespace
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001231 RID: 4657
		IName Alias { get; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001232 RID: 4658
		IName NamespaceName { get; }
	}
}
