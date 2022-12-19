using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200025E RID: 606
	internal interface ISecurityDeclarationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000EBA RID: 3770
		bool HasSecurityDeclarations { get; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000EBB RID: 3771
		Collection<SecurityDeclaration> SecurityDeclarations { get; }
	}
}
