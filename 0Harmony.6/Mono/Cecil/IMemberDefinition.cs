using System;

namespace Mono.Cecil
{
	// Token: 0x02000123 RID: 291
	public interface IMemberDefinition : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000848 RID: 2120
		// (set) Token: 0x06000849 RID: 2121
		string Name { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600084A RID: 2122
		string FullName { get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600084B RID: 2123
		// (set) Token: 0x0600084C RID: 2124
		bool IsSpecialName { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600084D RID: 2125
		// (set) Token: 0x0600084E RID: 2126
		bool IsRuntimeSpecialName { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600084F RID: 2127
		// (set) Token: 0x06000850 RID: 2128
		TypeDefinition DeclaringType { get; set; }
	}
}
