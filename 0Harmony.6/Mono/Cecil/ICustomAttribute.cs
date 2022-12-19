using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000104 RID: 260
	internal interface ICustomAttribute
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060006FA RID: 1786
		TypeReference AttributeType { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060006FB RID: 1787
		bool HasFields { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060006FC RID: 1788
		bool HasProperties { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060006FD RID: 1789
		bool HasConstructorArguments { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060006FE RID: 1790
		Collection<CustomAttributeNamedArgument> Fields { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060006FF RID: 1791
		Collection<CustomAttributeNamedArgument> Properties { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000700 RID: 1792
		Collection<CustomAttributeArgument> ConstructorArguments { get; }
	}
}
