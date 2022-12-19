using System;
using System.Reflection;

namespace Mono.Cecil
{
	// Token: 0x0200012B RID: 299
	public interface IReflectionImporter
	{
		// Token: 0x06000867 RID: 2151
		AssemblyNameReference ImportReference(AssemblyName reference);

		// Token: 0x06000868 RID: 2152
		TypeReference ImportReference(Type type, IGenericParameterProvider context);

		// Token: 0x06000869 RID: 2153
		FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context);

		// Token: 0x0600086A RID: 2154
		MethodReference ImportReference(MethodBase method, IGenericParameterProvider context);
	}
}
