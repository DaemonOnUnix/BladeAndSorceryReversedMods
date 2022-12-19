using System;
using ThunderRoad;

namespace DynamicModule
{
	// Token: 0x02000004 RID: 4
	public static class KeyBind
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000217C File Offset: 0x0000037C
		public static bool CheckBind(PlayerHand playerHand, string action)
		{
			bool flag;
			if (!(action == "grip"))
			{
				if (!(action == "trigger"))
				{
					flag = action == "spellWheel" && playerHand.controlHand.secondaryUsePressed;
				}
				else
				{
					flag = playerHand.controlHand.usePressed;
				}
			}
			else
			{
				flag = playerHand.controlHand.gripPressed;
			}
			return flag;
		}
	}
}
