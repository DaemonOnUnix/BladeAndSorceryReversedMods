using System;

namespace HarmonyLib
{
	// Token: 0x0200004E RID: 78
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyBefore : HarmonyAttribute
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00009E8C File Offset: 0x0000808C
		public HarmonyBefore(params string[] before)
		{
			this.info.before = before;
		}
	}
}
