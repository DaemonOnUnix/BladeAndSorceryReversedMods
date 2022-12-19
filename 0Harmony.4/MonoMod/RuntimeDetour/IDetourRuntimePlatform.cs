using System;
using System.Reflection;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000466 RID: 1126
	internal interface IDetourRuntimePlatform
	{
		// Token: 0x06001850 RID: 6224
		MethodBase GetIdentifiable(MethodBase method);

		// Token: 0x06001851 RID: 6225
		IntPtr GetNativeStart(MethodBase method);

		// Token: 0x06001852 RID: 6226
		MethodInfo CreateCopy(MethodBase method);

		// Token: 0x06001853 RID: 6227
		bool TryCreateCopy(MethodBase method, out MethodInfo dm);

		// Token: 0x06001854 RID: 6228
		void Pin(MethodBase method);

		// Token: 0x06001855 RID: 6229
		void Unpin(MethodBase method);

		// Token: 0x06001856 RID: 6230
		MethodBase GetDetourTarget(MethodBase from, MethodBase to);

		// Token: 0x06001857 RID: 6231
		uint TryMemAllocScratchCloseTo(IntPtr target, out IntPtr ptr, int size);

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001858 RID: 6232
		bool OnMethodCompiledWillBeCalled { get; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06001859 RID: 6233
		// (remove) Token: 0x0600185A RID: 6234
		event OnMethodCompiledEvent OnMethodCompiled;
	}
}
