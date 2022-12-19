using System;

namespace HarmonyLib
{
	// Token: 0x02000051 RID: 81
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyDebug : HarmonyAttribute
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000AD30 File Offset: 0x00008F30
		public HarmonyDebug()
		{
			this.info.debug = new bool?(true);
		}
	}
}
