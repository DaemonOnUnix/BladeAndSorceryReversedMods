using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E7 RID: 487
	internal interface ICustomDebugInformationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F43 RID: 3907
		bool HasCustomDebugInformations { get; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F44 RID: 3908
		Collection<CustomDebugInformation> CustomDebugInformations { get; }
	}
}
