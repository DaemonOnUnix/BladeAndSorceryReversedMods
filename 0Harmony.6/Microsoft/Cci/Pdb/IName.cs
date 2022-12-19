using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000302 RID: 770
	internal interface IName
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001233 RID: 4659
		int UniqueKey { get; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001234 RID: 4660
		int UniqueKeyIgnoringCase { get; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001235 RID: 4661
		string Value { get; }
	}
}
