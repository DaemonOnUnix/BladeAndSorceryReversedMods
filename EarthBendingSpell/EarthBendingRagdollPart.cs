using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000003 RID: 3
	public class EarthBendingRagdollPart : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000027A0 File Offset: 0x000009A0
		public void Initialize()
		{
			this.creature = this.ragdollPart.ragdoll.creature;
			RagdollPart.Type type = this.ragdollPart.type;
			RagdollPart.Type type2 = 1920;
			bool flag = type2.HasFlag(type);
			if (flag)
			{
				this.creature.locomotion.speedModifiers.Add(new Locomotion.SpeedModifier(this, 0f, 0f, 0f, 0f, 0f));
			}
			RagdollPart.Type type3 = 40;
			RagdollPart.Type type4 = 80;
			bool flag2 = type3.HasFlag(type) || type4.HasFlag(type);
			if (flag2)
			{
				Side side = 0;
				bool flag3 = false;
				bool flag4 = type3.HasFlag(type);
				if (flag4)
				{
					side = 1;
					flag3 = true;
				}
				else
				{
					bool flag5 = type4.HasFlag(type);
					if (flag5)
					{
						side = 0;
						flag3 = true;
					}
				}
				bool flag6 = flag3;
				if (flag6)
				{
					bool flag7 = this.creature.equipment.GetHeldHandle(side);
					if (flag7)
					{
						foreach (RagdollHand ragdollHand in this.creature.equipment.GetHeldHandle(side).handlers)
						{
							bool flag8 = ragdollHand.creature == this.creature;
							if (flag8)
							{
								ragdollHand.UnGrab(false);
							}
						}
					}
				}
			}
			bool flag9 = type == 1 || type == 2;
			if (flag9)
			{
				this.creature.brain.Stop();
				this.creature.locomotion.speedModifiers.Add(new Locomotion.SpeedModifier(this, 0f, 0f, 0f, 0f, 0f));
				this.creature.animator.speed = 0f;
			}
			Player.currentCreature.StartCoroutine(this.ImbueCoroutine());
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000029C8 File Offset: 0x00000BC8
		public void ResetCreature()
		{
			this.ragdollPart.rb.constraints = 0;
			this.creature.locomotion.ClearSpeedModifiers();
			bool flag = !this.creature.brain.instance.isActive;
			if (flag)
			{
				this.creature.brain.instance.Start();
			}
			this.creature.animator.speed = 1f;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002A43 File Offset: 0x00000C43
		private IEnumerator ImbueCoroutine()
		{
			this.creature.OnKillEvent += new Creature.KillEvent(this.Creature_OnKillEvent);
			List<EffectInstance> effects = new List<EffectInstance>();
			foreach (Collider collider in this.ragdollPart.colliderGroup.colliders)
			{
				EffectInstance imbueEffect = Catalog.GetData<EffectData>("EarthRagdollImbue", true).Spawn(this.ragdollPart.transform, true, null, false, Array.Empty<Type>());
				imbueEffect.SetCollider(collider);
				imbueEffect.Play(0, false);
				imbueEffect.SetIntensity(1f);
				effects.Add(imbueEffect);
				imbueEffect = null;
				collider = null;
			}
			List<Collider>.Enumerator enumerator = default(List<Collider>.Enumerator);
			this.ragdollPart.rb.constraints = 112;
			float startTime = Time.time;
			yield return new WaitUntil(() => Time.time - startTime > 30f);
			this.ResetCreature();
			foreach (EffectInstance imbueEffect2 in effects)
			{
				imbueEffect2.Stop(0);
				imbueEffect2 = null;
			}
			List<EffectInstance>.Enumerator enumerator2 = default(List<EffectInstance>.Enumerator);
			Object.Destroy(this);
			yield break;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002A54 File Offset: 0x00000C54
		private void Creature_OnKillEvent(CollisionInstance collisionStruct, EventTime eventTime)
		{
			foreach (RagdollPart ragdollPart in collisionStruct.damageStruct.hitRagdollPart.ragdoll.parts)
			{
				bool flag = ragdollPart.gameObject.GetComponent<EarthBendingRagdollPart>();
				if (flag)
				{
					EarthBendingRagdollPart component = ragdollPart.gameObject.GetComponent<EarthBendingRagdollPart>();
					component.ResetCreature();
					Object.Destroy(component);
				}
			}
		}

		// Token: 0x0400002E RID: 46
		public RagdollPart ragdollPart;

		// Token: 0x0400002F RID: 47
		private Creature creature;

		// Token: 0x04000030 RID: 48
		private List<HumanBodyBones> armLeftBones = new List<HumanBodyBones> { 15, 13, 17 };

		// Token: 0x04000031 RID: 49
		private List<HumanBodyBones> armRightBones = new List<HumanBodyBones> { 16, 14, 18 };
	}
}
