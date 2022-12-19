using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x0200000B RID: 11
	internal class EarthIceMerge : SpellMergeData
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000045D8 File Offset: 0x000027D8
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.frostEffectData = Catalog.GetData<EffectData>(this.frostEffectId, true);
			this.frozenEffectData = Catalog.GetData<EffectData>(this.frozenEffectId, true);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004608 File Offset: 0x00002808
		public override void Merge(bool active)
		{
			base.Merge(active);
			if (!active)
			{
				Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
				Vector3 vector2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
				bool flag = vector.magnitude > SpellCaster.throwMinHandVelocity && vector2.magnitude > SpellCaster.throwMinHandVelocity;
				if (flag)
				{
					bool flag2 = Vector3.Angle(vector, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) < 45f || Vector3.Angle(vector2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) < 45f;
					if (flag2)
					{
						bool flag3 = this.currentCharge > this.frostMinCharge && !EarthBendingController.IceActive;
						if (flag3)
						{
							EarthBendingController.IceActive = true;
							this.mana.StartCoroutine(this.IceSpikesCoroutine());
							this.currentCharge = 0f;
						}
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000474E File Offset: 0x0000294E
		private IEnumerator IceSpikesCoroutine()
		{
			Vector3 pos = Player.currentCreature.transform.position;
			EffectInstance frostEffectInstance = this.frostEffectData.Spawn(pos, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			frostEffectInstance.Play(0, false);
			foreach (Creature creature in Creature.allActive)
			{
				bool flag = creature != Player.currentCreature && !creature.isKilled;
				if (flag)
				{
					float dist = Vector3.Distance(creature.transform.position, pos);
					bool flag2 = dist < this.frostRadius;
					if (flag2)
					{
						this.mana.StartCoroutine(this.FreezeCreature(creature, this.frozenDuration));
					}
				}
				creature = null;
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			yield return new WaitForSeconds(6f);
			frostEffectInstance.Stop(0);
			EarthBendingController.IceActive = false;
			yield break;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000475D File Offset: 0x0000295D
		private IEnumerator FreezeCreature(Creature targetCreature, float duration)
		{
			EffectInstance effectInstance = this.frozenEffectData.Spawn(targetCreature.transform.position, Quaternion.identity, targetCreature.transform, null, true, null, false, Array.Empty<Type>());
			effectInstance.Play(0, false);
			targetCreature.animator.speed = 0f;
			targetCreature.locomotion.SetSpeedModifier(this, 0f, 0f, 0f, 0f, 0f);
			targetCreature.brain.Stop();
			yield return new WaitForSeconds(duration);
			bool flag = !targetCreature.isKilled;
			if (flag)
			{
				targetCreature.ragdoll.SetState(1, false);
				targetCreature.animator.speed = 1f;
				targetCreature.locomotion.ClearSpeedModifiers();
				targetCreature.brain.Load(targetCreature.brain.instance.id);
			}
			effectInstance.Despawn();
			yield break;
		}

		// Token: 0x0400008D RID: 141
		public float frostMinCharge;

		// Token: 0x0400008E RID: 142
		public string frostEffectId;

		// Token: 0x0400008F RID: 143
		public float frostRadius;

		// Token: 0x04000090 RID: 144
		public float frozenDuration;

		// Token: 0x04000091 RID: 145
		public string frozenEffectId;

		// Token: 0x04000092 RID: 146
		private EffectData frostEffectData;

		// Token: 0x04000093 RID: 147
		private EffectData frozenEffectData;
	}
}
