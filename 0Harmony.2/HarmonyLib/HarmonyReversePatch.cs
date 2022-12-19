using System;

namespace HarmonyLib
{
	// Token: 0x0200004C RID: 76
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class HarmonyReversePatch : HarmonyAttribute
	{
		// Token: 0x06000185 RID: 389 RVA: 0x0000ACDB File Offset: 0x00008EDB
		public HarmonyReversePatch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			this.info.reversePatchType = new HarmonyReversePatchType?(type);
		}
	}
}
