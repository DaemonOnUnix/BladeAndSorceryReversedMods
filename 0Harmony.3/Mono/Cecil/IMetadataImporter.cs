using System;

namespace Mono.Cecil
{
	// Token: 0x02000129 RID: 297
	public interface IMetadataImporter
	{
		// Token: 0x06000862 RID: 2146
		AssemblyNameReference ImportReference(AssemblyNameReference reference);

		// Token: 0x06000863 RID: 2147
		TypeReference ImportReference(TypeReference type, IGenericParameterProvider context);

		// Token: 0x06000864 RID: 2148
		FieldReference ImportReference(FieldReference field, IGenericParameterProvider context);

		// Token: 0x06000865 RID: 2149
		MethodReference ImportReference(MethodReference method, IGenericParameterProvider context);
	}
}
