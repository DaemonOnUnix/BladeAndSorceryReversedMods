using System;

namespace HarmonyLib
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyAfter : HarmonyAttribute
	{
		// Token: 0x06000189 RID: 393 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public HarmonyAfter(params string[] after)
		{
			this.info.after = after;
		}
	}
}
