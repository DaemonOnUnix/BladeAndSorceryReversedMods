using System;

namespace Mono.Cecil
{
	// Token: 0x0200022E RID: 558
	public interface IMetadataResolver
	{
		// Token: 0x06000C27 RID: 3111
		TypeDefinition Resolve(TypeReference type);

		// Token: 0x06000C28 RID: 3112
		FieldDefinition Resolve(FieldReference field);

		// Token: 0x06000C29 RID: 3113
		MethodDefinition Resolve(MethodReference method);
	}
}
