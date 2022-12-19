using System;

namespace HarmonyLib
{
	// Token: 0x0200004B RID: 75
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class HarmonyReversePatch : HarmonyAttribute
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00009E5F File Offset: 0x0000805F
		public HarmonyReversePatch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			this.info.reversePatchType = new HarmonyReversePatchType?(type);
		}
	}
}
