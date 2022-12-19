using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001F6 RID: 502
	internal interface ICustomAttribute
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000A32 RID: 2610
		TypeReference AttributeType { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A33 RID: 2611
		bool HasFields { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000A34 RID: 2612
		bool HasProperties { get; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000A35 RID: 2613
		bool HasConstructorArguments { get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A36 RID: 2614
		Collection<CustomAttributeNamedArgument> Fields { get; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A37 RID: 2615
		Collection<CustomAttributeNamedArgument> Properties { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A38 RID: 2616
		Collection<CustomAttributeArgument> ConstructorArguments { get; }
	}
}
