using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Heartbreaker
{
	// Token: 0x02000006 RID: 6
	public class SpellPhase : SpellCastCharge
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00003124 File Offset: 0x00001324
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			spellCaster.ragdollHand.OnGrabEvent += new RagdollHand.GrabEvent(this.RagdollHand_OnGrabEvent);
			spellCaster.ragdollHand.OnUnGrabEvent += new RagdollHand.UnGrabEvent(this.RagdollHand_OnUnGrabEvent);
			foreach (Collider collider in spellCaster.ragdollHand.colliderGroup.colliders)
			{
				collider.enabled = false;
			}
			bool flag = SpellPhase.phaseEffect == null;
			if (flag)
			{
				SpellPhase.phaseEffect = Catalog.GetData<EffectData>("SpellPhaseLoad", true);
			}
			this.effectInstance = SpellPhase.phaseEffect.Spawn(spellCaster.ragdollHand.transform, true, null, false, Array.Empty<Type>());
			this.effectInstance.Play(0, false);
			bool flag2 = SpellPhase.phasingPose == null;
			if (flag2)
			{
				SpellPhase.phasingPose = Catalog.GetData<HandPoseData>("Phasing", true);
			}
			bool flag3 = SpellPhase.defaultOpenPose == null;
			if (flag3)
			{
				SpellPhase.defaultOpenPose = Catalog.GetData<HandPoseData>("DefaultOpen", true);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003248 File Offset: 0x00001448
		public override void Fire(bool active)
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000324C File Offset: 0x0000144C
		public override void UpdateCaster()
		{
			bool flag = HeartbreakerLevelModule.local == null || HeartbreakerLevelModule.local.noHandVibrationWhenPhasing;
			if (!flag)
			{
				float speed = 1000f;
				float amount = 0.01f;
				float deltaY = Mathf.Sin(Time.time * speed) * amount;
				bool flag2 = this.spellCaster.ragdollHand.grabbedHandle != null && this.spellCaster.ragdollHand.grabbedHandle.item != null;
				if (flag2)
				{
					this.spellCaster.ragdollHand.grabbedHandle.item.transform.Translate(new Vector3(0f, deltaY, 0f));
				}
				else
				{
					this.spellCaster.ragdollHand.transform.Translate(new Vector3(0f, deltaY, 0f));
				}
				bool flag3 = this.spellCaster.ragdollHand.poser.defaultHandPoseData == SpellPhase.defaultOpenPose && SpellPhase.phasingPose != null;
				if (flag3)
				{
					this.spellCaster.ragdollHand.poser.SetDefaultPose(SpellPhase.phasingPose);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003370 File Offset: 0x00001570
		public override void Unload()
		{
			base.Unload();
			this.spellCaster.ragdollHand.OnGrabEvent -= new RagdollHand.GrabEvent(this.RagdollHand_OnGrabEvent);
			this.spellCaster.ragdollHand.OnUnGrabEvent -= new RagdollHand.UnGrabEvent(this.RagdollHand_OnUnGrabEvent);
			this.PhaseHandle(this.spellCaster.ragdollHand.grabbedHandle, false);
			foreach (Collider collider in this.spellCaster.ragdollHand.colliderGroup.colliders)
			{
				collider.enabled = true;
			}
			bool flag = this.effectInstance != null;
			if (flag)
			{
				this.effectInstance.End(false, -1f);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003450 File Offset: 0x00001650
		private void RagdollHand_OnGrabEvent(Side side, Handle handle, float axisPosition, HandlePose orientation, EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (!flag)
			{
				this.PhaseHandle(handle, true);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003474 File Offset: 0x00001674
		private void RagdollHand_OnUnGrabEvent(Side side, Handle handle, bool throwing, EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (!flag)
			{
				this.PhaseHandle(handle, false);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003498 File Offset: 0x00001698
		private void PhaseHandle(Handle handle, bool active)
		{
			bool flag = !handle || !handle.item;
			if (!flag)
			{
				bool flag2 = !this.allowSpellMenuDict.ContainsKey(handle);
				if (flag2)
				{
					this.allowSpellMenuDict.Add(handle, handle.data.allowSpellMenu);
				}
				foreach (ColliderGroup colliderGroup in handle.item.colliderGroups)
				{
					foreach (Collider collider in colliderGroup.colliders)
					{
						collider.enabled = !active;
					}
				}
				handle.data.allowSpellMenu = active || this.allowSpellMenuDict[handle];
			}
		}

		// Token: 0x0400001C RID: 28
		public static HandPoseData defaultOpenPose;

		// Token: 0x0400001D RID: 29
		public static HandPoseData phasingPose;

		// Token: 0x0400001E RID: 30
		public static EffectData phaseEffect;

		// Token: 0x0400001F RID: 31
		private Dictionary<Handle, bool> allowSpellMenuDict = new Dictionary<Handle, bool>();

		// Token: 0x04000020 RID: 32
		private EffectInstance effectInstance;
	}
}
