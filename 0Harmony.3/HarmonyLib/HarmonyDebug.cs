using System;

namespace HarmonyLib
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyDebug : HarmonyAttribute
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00009EB4 File Offset: 0x000080B4
		public HarmonyDebug()
		{
			this.info.debug = new bool?(true);
		}
	}
}
