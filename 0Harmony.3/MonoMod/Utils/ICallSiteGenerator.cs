using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200034D RID: 845
	public interface ICallSiteGenerator
	{
		// Token: 0x060013C7 RID: 5063
		CallSite ToCallSite(ModuleDefinition module);
	}
}
