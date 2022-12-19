using System;
using System.Reflection;

namespace Mono.Cecil
{
	// Token: 0x0200021E RID: 542
	public interface IReflectionImporter
	{
		// Token: 0x06000BAC RID: 2988
		AssemblyNameReference ImportReference(AssemblyName reference);

		// Token: 0x06000BAD RID: 2989
		TypeReference ImportReference(Type type, IGenericParameterProvider context);

		// Token: 0x06000BAE RID: 2990
		FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context);

		// Token: 0x06000BAF RID: 2991
		MethodReference ImportReference(MethodBase method, IGenericParameterProvider context);
	}
}
