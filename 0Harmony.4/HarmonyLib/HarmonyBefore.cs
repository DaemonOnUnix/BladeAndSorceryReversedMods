using System;

namespace HarmonyLib
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyBefore : HarmonyAttribute
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000AD08 File Offset: 0x00008F08
		public HarmonyBefore(params string[] before)
		{
			this.info.before = before;
		}
	}
}
