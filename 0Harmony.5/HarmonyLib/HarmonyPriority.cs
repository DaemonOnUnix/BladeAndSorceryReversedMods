using System;

namespace HarmonyLib
{
	// Token: 0x0200004E RID: 78
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyPriority : HarmonyAttribute
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public HarmonyPriority(int priority)
		{
			this.info.priority = priority;
		}
	}
}
