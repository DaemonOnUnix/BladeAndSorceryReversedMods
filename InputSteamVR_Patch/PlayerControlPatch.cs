using System;
using HarmonyLib;
using ThunderRoad;

namespace InputSteamVR_Patch
{
	// Token: 0x02000003 RID: 3
	public static class PlayerControlPatch
	{
		// Token: 0x02000006 RID: 6
		[HarmonyPatch("Grip")]
		[HarmonyPatch(typeof(PlayerControl))]
		private static class PlayerControlGripPatch
		{
			// Token: 0x0600000E RID: 14 RVA: 0x00002810 File Offset: 0x00000A10
			[HarmonyPrefix]
			private static bool Prefix(PlayerControl __instance, Side side, float axis)
			{
				Player local = Player.local;
				bool flag = !((local != null) ? local.creature : null);
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					bool flag3 = (double)axis > 0.0;
					if (flag3)
					{
						bool flag4 = !PlayerControl.GetHand(side).gripPressed;
						if (flag4)
						{
							PlayerControl.GetHand(side).InvokeButtonEvent(2, true);
							__instance.GrabGrip(side);
							__instance.TelekinesisGrab(side);
						}
					}
					else
					{
						bool flag5 = !PlayerControl.GetHand(side).gripPressed;
						if (flag5)
						{
							return false;
						}
						PlayerControl.GetHand(side).InvokeButtonEvent(2, false);
						__instance.UngrabGrip(side);
						__instance.TelekinesisRelease(side);
					}
					flag2 = false;
				}
				return flag2;
			}
		}
	}
}
