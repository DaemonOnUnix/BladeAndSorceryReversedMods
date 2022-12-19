using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000009 RID: 9
	internal class EarthGravMerge : SpellMergeData
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00004200 File Offset: 0x00002400
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.bubbleEffectData = Catalog.GetData<EffectData>(this.bubbleEffectId, true);
			this.portalEffectData = Catalog.GetData<EffectData>(this.portalEffectId, true);
			this.rockCollisionEffectData = Catalog.GetData<EffectData>(this.rockCollisionEffectId, true);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004240 File Offset: 0x00002440
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
						bool flag3 = this.currentCharge > this.bubbleMinCharge && !EarthBendingController.GravActive;
						if (flag3)
						{
							EarthBendingController.GravActive = true;
							this.mana.StartCoroutine(this.BubbleCoroutine());
							this.currentCharge = 0f;
						}
					}
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004386 File Offset: 0x00002586
		public IEnumerator BubbleCoroutine()
		{
			Vector3 centerPoint = this.mana.mergePoint.transform.position;
			EffectInstance bubbleEffect = null;
			bubbleEffect = this.bubbleEffectData.Spawn(centerPoint, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			bubbleEffect.SetIntensity(0f);
			bubbleEffect.Play(0, false);
			ParticleSystem parentParticleSystem = bubbleEffect.effects[0].gameObject.GetComponent<ParticleSystem>();
			foreach (ParticleSystem particleSystem in parentParticleSystem.gameObject.GetComponentsInChildren<ParticleSystem>())
			{
				bool flag = particleSystem.gameObject.name == "Portal";
				if (flag)
				{
					float startDelay = particleSystem.main.startDelay.constant;
					Player.currentCreature.mana.StartCoroutine(this.PlayEffectSound(startDelay, this.portalEffectData, particleSystem.transform.position, 3f));
				}
				bool flag2 = particleSystem.gameObject.name == "Rock";
				if (flag2)
				{
					RockCollision scr = particleSystem.gameObject.AddComponent<RockCollision>();
					scr.rockCollisionEffectData = this.rockCollisionEffectData;
					scr.rockExplosionForce = this.rockExplosionForce;
					scr.rockExplosionRadius = this.rockExplosionRadius;
					scr.part = particleSystem;
					scr = null;
				}
				particleSystem = null;
			}
			ParticleSystem[] array = null;
			yield return new WaitForSeconds(4.5f);
			bubbleEffect.Stop(0);
			EarthBendingController.GravActive = false;
			yield break;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004395 File Offset: 0x00002595
		public IEnumerator PlayEffectSound(float delay, EffectData effectData, Vector3 position, float despawnDelay = 0f)
		{
			yield return new WaitForSeconds(delay);
			EffectInstance effectInstance = effectData.Spawn(position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			effectInstance.Play(0, false);
			bool flag = despawnDelay != 0f;
			if (flag)
			{
				yield return new WaitForSeconds(despawnDelay);
				effectInstance.Stop(0);
			}
			yield break;
		}

		// Token: 0x0400007F RID: 127
		public string bubbleEffectId;

		// Token: 0x04000080 RID: 128
		public float bubbleMinCharge;

		// Token: 0x04000081 RID: 129
		public float rockExplosionRadius;

		// Token: 0x04000082 RID: 130
		public float rockExplosionForce;

		// Token: 0x04000083 RID: 131
		public string portalEffectId;

		// Token: 0x04000084 RID: 132
		public string rockCollisionEffectId;

		// Token: 0x04000085 RID: 133
		private EffectData bubbleEffectData;

		// Token: 0x04000086 RID: 134
		private EffectData portalEffectData;

		// Token: 0x04000087 RID: 135
		private EffectData rockCollisionEffectData;
	}
}
