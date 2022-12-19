using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace Heartbreaker
{
	// Token: 0x02000005 RID: 5
	public static class RagdollHandPoserExtension
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002FE0 File Offset: 0x000011E0
		public static void CloseHand(this RagdollHand ragdollHand, float strength)
		{
			float delta = ragdollHand.ragdoll.creature.data.ragdollData.fingerSpeed * Time.deltaTime;
			float strength2 = Mathf.Lerp(0.5f, 1f, strength);
			ragdollHand.poser.thumbCloseWeight = Mathf.MoveTowards(ragdollHand.poser.thumbCloseWeight, strength2, delta);
			ragdollHand.poser.indexCloseWeight = Mathf.MoveTowards(ragdollHand.poser.indexCloseWeight, strength2, delta);
			ragdollHand.poser.middleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.middleCloseWeight, strength2, delta);
			ragdollHand.poser.ringCloseWeight = Mathf.MoveTowards(ragdollHand.poser.ringCloseWeight, strength2, delta);
			ragdollHand.poser.littleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.littleCloseWeight, strength2, delta);
			ragdollHand.poser.UpdatePoseThumb(ragdollHand.poser.thumbCloseWeight);
			ragdollHand.poser.UpdatePoseIndex(ragdollHand.poser.indexCloseWeight);
			ragdollHand.poser.UpdatePoseMiddle(ragdollHand.poser.middleCloseWeight);
			ragdollHand.poser.UpdatePoseRing(ragdollHand.poser.ringCloseWeight);
			ragdollHand.poser.UpdatePoseLittle(ragdollHand.poser.littleCloseWeight);
		}

		// Token: 0x02000008 RID: 8
		[HarmonyPatch("ManagedUpdate")]
		[HarmonyPatch(typeof(RagdollHandPoser))]
		private static class RagdollHandPoserManagedUpdatePatch
		{
			// Token: 0x06000023 RID: 35 RVA: 0x00003614 File Offset: 0x00001814
			[HarmonyPostfix]
			private static void Postfix(RagdollHandPoser __instance)
			{
				RagdollHand ragdollHand = __instance.ragdollHand;
				Object @object;
				if (ragdollHand == null)
				{
					@object = null;
				}
				else
				{
					Ragdoll ragdoll = ragdollHand.ragdoll;
					@object = ((ragdoll != null) ? ragdoll.creature : null);
				}
				bool flag = !@object || !__instance.ragdollHand.ragdoll.creature.isPlayer;
				if (!flag)
				{
					bool flag2 = __instance.ragdollHand.grabbedHandle && __instance.ragdollHand.grabbedHandle.item && __instance.ragdollHand.grabbedHandle.item.itemId == "Heart";
					if (flag2)
					{
						__instance.ragdollHand.CloseHand(__instance.ragdollHand.playerHand.controlHand.gripAxis);
					}
				}
			}
		}
	}
}
