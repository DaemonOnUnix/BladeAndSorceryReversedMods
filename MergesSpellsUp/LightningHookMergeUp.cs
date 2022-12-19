using System;
using ThunderRoad;
using UnityEngine;

namespace MergesSpellsUp
{
	// Token: 0x02000006 RID: 6
	public class LightningHookMergeUp : MonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00006EF0 File Offset: 0x000050F0
		private void Awake()
		{
			this.creature = base.GetComponent<Creature>();
			this.jointRb = new GameObject(this.creature.name + " Lightning Hook Joint RB").AddComponent<Rigidbody>();
			this.jointRb.isKinematic = true;
			this.jointRb.useGravity = false;
			this.creature.OnDespawnEvent += delegate(EventTime time)
			{
				bool flag = time > 0;
				if (!flag)
				{
					Object.Destroy(this);
				}
			};
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006F64 File Offset: 0x00005164
		public void Hook(LightningMergeUp hookingSpell)
		{
			bool flag = this.active;
			if (!flag)
			{
				this.spell = hookingSpell;
				RagdollPart part = this.creature.ragdoll.GetPart(4);
				this.creature.ragdoll.SetState(1);
				this.creature.brain.AddNoStandUpModifier(this);
				this.creature.ragdoll.AddPhysicToggleModifier(this);
				this.creature.ragdoll.SetPhysicModifier(this, 0f, 1f, -1f, -1f, null);
				this.jointRb.transform.position = part.transform.position;
				this.joint = part.rb.gameObject.AddComponent<SpringJoint>();
				this.joint.autoConfigureConnectedAnchor = false;
				this.joint.connectedBody = this.jointRb;
				this.joint.connectedAnchor = Vector3.zero;
				this.joint.anchor = Vector3.zero;
				this.joint.spring = this.spell.beamHookSpring;
				this.joint.damper = this.spell.beamHookDamper;
				this.active = true;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000070A0 File Offset: 0x000052A0
		public void Hook(LightningBeam hookingbeam)
		{
			bool flag = this.active;
			if (!flag)
			{
				this.beamSpell = hookingbeam;
				RagdollPart part = this.creature.ragdoll.GetPart(4);
				this.creature.ragdoll.SetState(1);
				this.creature.brain.AddNoStandUpModifier(this);
				this.creature.ragdoll.AddPhysicToggleModifier(this);
				this.creature.ragdoll.SetPhysicModifier(this, 0f, 1f, -1f, -1f, null);
				this.jointRb.transform.position = part.transform.position;
				this.joint = part.rb.gameObject.AddComponent<SpringJoint>();
				this.joint.autoConfigureConnectedAnchor = false;
				this.joint.connectedBody = this.jointRb;
				this.joint.connectedAnchor = Vector3.zero;
				this.joint.anchor = Vector3.zero;
				this.joint.spring = this.beamSpell.beamHookSpring;
				this.joint.damper = this.beamSpell.beamHookDamper;
				this.active = true;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000071DC File Offset: 0x000053DC
		public void Unhook()
		{
			bool flag = !this.active;
			if (!flag)
			{
				this.spell = null;
				this.active = false;
				Object.Destroy(this.joint);
				this.creature.brain.RemoveNoStandUpModifier(this);
				this.creature.ragdoll.RemovePhysicToggleModifier(this);
				this.creature.ragdoll.RemovePhysicModifier(this);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000724C File Offset: 0x0000544C
		private void Update()
		{
			bool flag = this.spell == null || !this.active || this.jointRb == null;
			if (!flag)
			{
				Vector3 position = this.jointRb.transform.position;
				this.jointRb.transform.SetPositionAndRotation(Vector3.Lerp(position, this.spell.beamHitPoint.position, Time.deltaTime * this.spell.beamHookSpeed), Quaternion.LookRotation(position - this.spell.beamStart.position));
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000072E3 File Offset: 0x000054E3
		private void OnDestroy()
		{
			Object.Destroy(this.joint);
		}

		// Token: 0x04000036 RID: 54
		public Creature creature;

		// Token: 0x04000037 RID: 55
		private SpringJoint joint;

		// Token: 0x04000038 RID: 56
		private Rigidbody jointRb;

		// Token: 0x04000039 RID: 57
		private LightningMergeUp spell;

		// Token: 0x0400003A RID: 58
		private LightningBeam beamSpell;

		// Token: 0x0400003B RID: 59
		private bool active;
	}
}
