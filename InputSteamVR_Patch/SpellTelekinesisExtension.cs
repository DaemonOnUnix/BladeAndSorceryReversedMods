using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace InputSteamVR_Patch
{
	// Token: 0x02000005 RID: 5
	public static class SpellTelekinesisExtension
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002644 File Offset: 0x00000844
		public static void SqueezeTelekinesis(this RagdollHand ragdollHand, float strength)
		{
			float delta = ragdollHand.ragdoll.creature.data.ragdollData.fingerSpeed * Time.deltaTime;
			ragdollHand.poser.SetDefaultPose(SpellTelekinesisExtension.tkOpenPose);
			ragdollHand.poser.SetTargetPose(InputSteamVRPatchLevelModule.defaultClosePose, false, false, false, false, false);
			ragdollHand.poser.thumbCloseWeight = Mathf.MoveTowards(ragdollHand.poser.thumbCloseWeight, strength, delta);
			ragdollHand.poser.indexCloseWeight = Mathf.MoveTowards(ragdollHand.poser.indexCloseWeight, Mathf.Lerp(0f, 0.75f, Mathf.Lerp(0f, 0.8f, strength) + Mathf.Lerp(0f, 0.2f, ragdollHand.poser.indexCloseWeight)), delta);
			ragdollHand.poser.middleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.middleCloseWeight, Mathf.Lerp(0f, 0.75f, Mathf.Lerp(0f, 0.8f, strength) + Mathf.Lerp(0f, 0.2f, ragdollHand.poser.middleCloseWeight)), delta);
			ragdollHand.poser.ringCloseWeight = Mathf.MoveTowards(ragdollHand.poser.ringCloseWeight, strength, delta);
			ragdollHand.poser.littleCloseWeight = Mathf.MoveTowards(ragdollHand.poser.littleCloseWeight, strength, delta);
			ragdollHand.poser.UpdatePoseThumb(ragdollHand.poser.thumbCloseWeight);
			ragdollHand.poser.UpdatePoseIndex(ragdollHand.poser.indexCloseWeight);
			ragdollHand.poser.UpdatePoseMiddle(ragdollHand.poser.middleCloseWeight);
			ragdollHand.poser.UpdatePoseRing(ragdollHand.poser.ringCloseWeight);
			ragdollHand.poser.UpdatePoseLittle(ragdollHand.poser.littleCloseWeight);
		}

		// Token: 0x04000006 RID: 6
		public static HandPoseData tkOpenPose;

		// Token: 0x02000008 RID: 8
		[HarmonyPatch("UpdateGrip")]
		[HarmonyPatch(typeof(SpellTelekinesis))]
		private static class SpellTelekinesisUpdateGripPatch
		{
			// Token: 0x06000010 RID: 16 RVA: 0x00002980 File Offset: 0x00000B80
			[HarmonyPostfix]
			private static void Postfix(SpellTelekinesis __instance)
			{
				bool flag = !__instance.catchedHandle;
				if (!flag)
				{
					RagdollHand ragdollHand = __instance.spellCaster.ragdollHand;
					bool flag2 = !ragdollHand.ragdoll.creature.isPlayer;
					if (!flag2)
					{
						ragdollHand.SqueezeTelekinesis(ragdollHand.playerHand.controlHand.gripAxis);
					}
				}
			}
		}
	}
}
