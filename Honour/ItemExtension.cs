using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace Honour
{
	// Token: 0x02000002 RID: 2
	public static class ItemExtension
	{
		// Token: 0x02000005 RID: 5
		[HarmonyPatch("StopThrowing")]
		[HarmonyPatch(typeof(Item))]
		private static class ItemStopThrowingPatch
		{
			// Token: 0x06000008 RID: 8 RVA: 0x00002EA4 File Offset: 0x000010A4
			[HarmonyPrefix]
			private static bool Prefix(Item __instance)
			{
				bool flag = !__instance.isThrowed || !__instance.forceThrown;
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					__instance.isThrowed = false;
					__instance.SetColliderAndMeshLayer(GameManager.GetLayer(6), false);
					__instance.rb.collisionDetectionMode = Catalog.gameData.collisionDetection.dropped;
					bool flag3 = Item.allThrowed.Contains(__instance);
					if (flag3)
					{
						Item.allThrowed.Remove(__instance);
					}
					__instance.lastInteractionTime = Time.time;
					flag2 = false;
				}
				return flag2;
			}
		}
	}
}
