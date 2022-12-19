using System;

namespace Mono.Cecil
{
	// Token: 0x0200022D RID: 557
	public interface IAssemblyResolver : IDisposable
	{
		// Token: 0x06000C25 RID: 3109
		AssemblyDefinition Resolve(AssemblyNameReference name);

		// Token: 0x06000C26 RID: 3110
		AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters);
	}
}
