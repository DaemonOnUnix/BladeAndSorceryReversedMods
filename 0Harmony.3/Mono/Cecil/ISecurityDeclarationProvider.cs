using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200016A RID: 362
	internal interface ISecurityDeclarationProvider : IMetadataTokenProvider
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B70 RID: 2928
		bool HasSecurityDeclarations { get; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B71 RID: 2929
		Collection<SecurityDeclaration> SecurityDeclarations { get; }
	}
}
