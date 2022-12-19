using System;

namespace Mono.Cecil
{
	// Token: 0x02000149 RID: 329
	internal interface IModifierType
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060009F1 RID: 2545
		TypeReference ModifierType { get; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060009F2 RID: 2546
		TypeReference ElementType { get; }
	}
}
