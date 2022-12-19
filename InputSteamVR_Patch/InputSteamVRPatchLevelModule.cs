using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;
using Valve.VR;

namespace InputSteamVR_Patch
{
	// Token: 0x02000002 RID: 2
	internal class InputSteamVRPatchLevelModule : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			this.harmony = new Harmony("com.hujohner.inputsteamvr");
			this.harmony.PatchAll(Assembly.GetExecutingAssembly());
			InputSteamVRPatchLevelModule.revealBruiseRight = Catalog.GetData<EffectData>("RevealBruiseRight", true);
			InputSteamVRPatchLevelModule.revealBruiseLeft = Catalog.GetData<EffectData>("RevealBruiseLeft", true);
			InputSteamVRPatchLevelModule.defaultClosePose = Catalog.GetData<HandPoseData>("DefaultClose", true);
			SpellTelekinesisExtension.tkOpenPose = Catalog.GetData<HandPoseData>("TelekinesisOpen", true);
			Debug.Log(string.Format("[InputSteamVRPatch] Mod v{0} loaded.", Assembly.GetExecutingAssembly().GetName().Version));
			bool flag = PlayerControl.loader == 2;
			if (flag)
			{
				this.gripAxisAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("GripAxis", false);
				this.gripAxisAction.AddOnChangeListener(delegate(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float axis, float delta)
				{
					this.GripStrength(0, axis);
				}, 2);
				this.gripAxisAction.AddOnChangeListener(delegate(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float axis, float delta)
				{
					this.GripStrength(1, axis);
				}, 1);
			}
			this.AddTips();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002140 File Offset: 0x00000340
		public override void OnUnload()
		{
			bool flag = this.gripAxisAction == null;
			if (!flag)
			{
				this.gripAxisAction.RemoveOnChangeListener(delegate(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float axis, float delta)
				{
					this.GripStrength(0, axis);
				}, 2);
				this.gripAxisAction.RemoveOnChangeListener(delegate(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float axis, float delta)
				{
					this.GripStrength(1, axis);
				}, 1);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002194 File Offset: 0x00000394
		public override void Update()
		{
			bool flag = !Player.local;
			if (!flag)
			{
				bool flag2 = PlayerControl.loader == 1;
				if (flag2)
				{
					this.GripStrength(0, ((InputXR)PlayerControl.input).rightController.trigger.GetValue());
					this.GripStrength(1, ((InputXR)PlayerControl.input).leftController.trigger.GetValue());
				}
				this.HandleBruising(1);
				this.HandleBruising(0);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002218 File Offset: 0x00000418
		private bool GripStrength(Side side, float value)
		{
			PlayerControl.Hand hand = PlayerControl.GetHand(side);
			bool flag = hand == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				hand.gripAxis = value;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002248 File Offset: 0x00000448
		private void HandleBruising(Side side)
		{
			RagdollHand ragdollHand2;
			if (side != 1)
			{
				PlayerHand handRight = Player.local.handRight;
				ragdollHand2 = ((handRight != null) ? handRight.ragdollHand : null);
			}
			else
			{
				PlayerHand handLeft = Player.local.handLeft;
				ragdollHand2 = ((handLeft != null) ? handLeft.ragdollHand : null);
			}
			RagdollHand ragdollHand = ragdollHand2;
			bool flag = !ragdollHand;
			if (!flag)
			{
				float gripAxis = PlayerControl.GetHand(side).gripAxis;
				HandleRagdoll handleRagdoll;
				bool flag2;
				if (gripAxis > 0f)
				{
					handleRagdoll = ragdollHand.grabbedHandle as HandleRagdoll;
					flag2 = handleRagdoll != null;
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					Vector3 dir = ragdollHand.grabbedHandle.transform.position - ragdollHand.grip.position;
					RaycastHit hit;
					bool flag4 = Physics.Raycast(ragdollHand.grip.position, dir, ref hit);
					if (flag4)
					{
						MaterialData material = MaterialData.GetMaterial(hit.collider);
						bool flag5 = material == null || material.id != "Flesh";
						if (!flag5)
						{
							DamageStruct damageStruct;
							damageStruct..ctor(3, 0f);
							damageStruct.hitRagdollPart = handleRagdoll.ragdollPart;
							CollisionInstance collisionInstance = new CollisionInstance(damageStruct, null, null)
							{
								targetColliderGroup = handleRagdoll.ragdollPart.colliderGroup,
								hasEffect = true
							};
							Quaternion rotation = Quaternion.LookRotation(hit.normal, ragdollHand.grip.up);
							EffectInstance effectInstance = ((side == 1) ? InputSteamVRPatchLevelModule.revealBruiseLeft : InputSteamVRPatchLevelModule.revealBruiseRight).Spawn(hit.point, rotation, collisionInstance.targetColliderGroup.transform, collisionInstance, true, null, false, Array.Empty<Type>());
							effectInstance.SetIntensity(gripAxis);
							effectInstance.Play(0, false);
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023E4 File Offset: 0x000005E4
		private void AddTips()
		{
			TextData.TextGroup tips = LocalizationManager.Instance.GetGroup("Tips");
			bool flag = tips != null;
			if (flag)
			{
				int id = tips.texts.Count + 1;
				TextData.TextID tip = new TextData.TextID
				{
					id = id.ToString(),
					text = "[InputSteamVRPatch] Squeezing while holding an enemy will leave bruises."
				};
				tips.texts.Add(tip);
			}
		}

		// Token: 0x04000001 RID: 1
		private SteamVR_Action_Single gripAxisAction;

		// Token: 0x04000002 RID: 2
		private Harmony harmony;

		// Token: 0x04000003 RID: 3
		private static EffectData revealBruiseRight;

		// Token: 0x04000004 RID: 4
		private static EffectData revealBruiseLeft;

		// Token: 0x04000005 RID: 5
		public static HandPoseData defaultClosePose;
	}
}
