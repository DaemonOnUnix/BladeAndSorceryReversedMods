using System;
using System.Reflection;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000382 RID: 898
	internal class DetourRuntimeNETCorePlatform : DetourRuntimeNETPlatform
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x00012279 File Offset: 0x00010479
		protected override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}
	}
}
