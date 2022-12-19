using System;

namespace Mono.Cecil
{
	// Token: 0x0200023D RID: 573
	internal interface IModifierType
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D3A RID: 3386
		TypeReference ModifierType { get; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000D3B RID: 3387
		TypeReference ElementType { get; }
	}
}
