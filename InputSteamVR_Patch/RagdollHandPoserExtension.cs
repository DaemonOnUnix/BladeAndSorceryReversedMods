using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace InputSteamVR_Patch
{
	// Token: 0x02000004 RID: 4
	public static class RagdollHandPoserExtension
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000247C File Offset: 0x0000067C
		public static void SqueezeRagdoll(this RagdollHand ragdollHand, float strength)
		{
			float delta = ragdollHand.ragdoll.creature.data.ragdollData.fingerSpeed * Time.deltaTime;
			HandPoseData.Pose.Fingers defaultClosePoseFingers = InputSteamVRPatchLevelModule.defaultClosePose.GetCreaturePose(ragdollHand.creature).GetFingers(ragdollHand.side);
			HandPoseData.Pose.Fingers currentPoseFingers = ragdollHand.poser.targetHandPoseFingers;
			float strength2 = Mathf.Lerp(0f, 0.25f, strength);
			ragdollHand.poser.thumbCloseWeight = Mathf.MoveTowards(ragdollHand.poser.thumbCloseWeight, strength2, delta);
			ragdollHand.poser.indexCloseWeight = Mathf.MoveTowards(ragdollHand.poser.indexCloseWeight, strength2, delta);
			ragdollHand.poser.middleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.middleCloseWeight, strength2, delta);
			ragdollHand.poser.ringCloseWeight = Mathf.MoveTowards(ragdollHand.poser.ringCloseWeight, strength2, delta);
			ragdollHand.poser.littleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.littleCloseWeight, strength2, delta);
			ragdollHand.poser.UpdateFinger(ragdollHand.fingerThumb, currentPoseFingers.thumb, defaultClosePoseFingers.thumb, ragdollHand.poser.thumbCloseWeight);
			ragdollHand.poser.UpdateFinger(ragdollHand.fingerIndex, currentPoseFingers.index, defaultClosePoseFingers.index, ragdollHand.poser.indexCloseWeight);
			ragdollHand.poser.UpdateFinger(ragdollHand.fingerMiddle, currentPoseFingers.middle, defaultClosePoseFingers.middle, ragdollHand.poser.middleCloseWeight);
			ragdollHand.poser.UpdateFinger(ragdollHand.fingerRing, currentPoseFingers.ring, defaultClosePoseFingers.ring, ragdollHand.poser.ringCloseWeight);
			ragdollHand.poser.UpdateFinger(ragdollHand.fingerLittle, currentPoseFingers.little, defaultClosePoseFingers.little, ragdollHand.poser.littleCloseWeight);
		}

		// Token: 0x02000007 RID: 7
		[HarmonyPatch("ManagedUpdate")]
		[HarmonyPatch(typeof(RagdollHandPoser))]
		private static class RagdollHandPoserManagedUpdatePatch
		{
			// Token: 0x0600000F RID: 15 RVA: 0x000028C8 File Offset: 0x00000AC8
			[HarmonyPostfix]
			private static void Postfix(RagdollHandPoser __instance)
			{
				bool flag = !__instance.ragdollHand.ragdoll.creature.isPlayer || !__instance.ragdollHand.grabbedHandle;
				if (!flag)
				{
					bool flag2 = __instance.ragdollHand.grabbedHandle.telekinesisHandler;
					if (flag2)
					{
						__instance.ragdollHand.SqueezeTelekinesis(__instance.ragdollHand.playerHand.controlHand.gripAxis);
					}
					else
					{
						bool flag3 = __instance.ragdollHand.grabbedHandle is HandleRagdoll;
						if (flag3)
						{
							__instance.ragdollHand.SqueezeRagdoll(__instance.ragdollHand.playerHand.controlHand.gripAxis);
						}
					}
				}
			}
		}
	}
}
