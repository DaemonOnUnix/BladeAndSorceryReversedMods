using System;

namespace HarmonyLib
{
	// Token: 0x0200004D RID: 77
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyPriority : HarmonyAttribute
	{
		// Token: 0x06000177 RID: 375 RVA: 0x00009E78 File Offset: 0x00008078
		public HarmonyPriority(int priority)
		{
			this.info.priority = priority;
		}
	}
}
