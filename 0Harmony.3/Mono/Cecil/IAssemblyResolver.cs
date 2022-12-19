using System;

namespace Mono.Cecil
{
	// Token: 0x0200013A RID: 314
	public interface IAssemblyResolver : IDisposable
	{
		// Token: 0x060008E2 RID: 2274
		AssemblyDefinition Resolve(AssemblyNameReference name);

		// Token: 0x060008E3 RID: 2275
		AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters);
	}
}
