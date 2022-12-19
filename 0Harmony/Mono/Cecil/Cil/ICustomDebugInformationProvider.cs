using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002DD RID: 733
	internal interface ICustomDebugInformationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060012B0 RID: 4784
		bool HasCustomDebugInformations { get; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060012B1 RID: 4785
		Collection<CustomDebugInformation> CustomDebugInformations { get; }
	}
}
