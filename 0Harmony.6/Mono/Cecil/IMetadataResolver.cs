using System;

namespace Mono.Cecil
{
	// Token: 0x0200013B RID: 315
	public interface IMetadataResolver
	{
		// Token: 0x060008E4 RID: 2276
		TypeDefinition Resolve(TypeReference type);

		// Token: 0x060008E5 RID: 2277
		FieldDefinition Resolve(FieldReference field);

		// Token: 0x060008E6 RID: 2278
		MethodDefinition Resolve(MethodReference method);
	}
}
