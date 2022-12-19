using System;

namespace HarmonyLib
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyAfter : HarmonyAttribute
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00009EA0 File Offset: 0x000080A0
		public HarmonyAfter(params string[] after)
		{
			this.info.after = after;
		}
	}
}
