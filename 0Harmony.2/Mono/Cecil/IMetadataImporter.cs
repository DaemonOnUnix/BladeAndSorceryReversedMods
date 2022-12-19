using System;

namespace Mono.Cecil
{
	// Token: 0x0200021C RID: 540
	public interface IMetadataImporter
	{
		// Token: 0x06000BA7 RID: 2983
		AssemblyNameReference ImportReference(AssemblyNameReference reference);

		// Token: 0x06000BA8 RID: 2984
		TypeReference ImportReference(TypeReference type, IGenericParameterProvider context);

		// Token: 0x06000BA9 RID: 2985
		FieldReference ImportReference(FieldReference field, IGenericParameterProvider context);

		// Token: 0x06000BAA RID: 2986
		MethodReference ImportReference(MethodReference method, IGenericParameterProvider context);
	}
}
