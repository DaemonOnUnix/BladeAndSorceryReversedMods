using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000447 RID: 1095
	public interface ICallSiteGenerator
	{
		// Token: 0x0600174E RID: 5966
		CallSite ToCallSite(ModuleDefinition module);
	}
}
