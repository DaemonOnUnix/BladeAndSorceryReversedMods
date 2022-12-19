using System;
using System.Reflection;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000367 RID: 871
	internal interface IDetourRuntimePlatform
	{
		// Token: 0x060014A5 RID: 5285
		IntPtr GetNativeStart(MethodBase method);

		// Token: 0x060014A6 RID: 5286
		MethodInfo CreateCopy(MethodBase method);

		// Token: 0x060014A7 RID: 5287
		bool TryCreateCopy(MethodBase method, out MethodInfo dm);

		// Token: 0x060014A8 RID: 5288
		void Pin(MethodBase method);

		// Token: 0x060014A9 RID: 5289
		void Unpin(MethodBase method);

		// Token: 0x060014AA RID: 5290
		MethodBase GetDetourTarget(MethodBase from, MethodBase to);
	}
}
