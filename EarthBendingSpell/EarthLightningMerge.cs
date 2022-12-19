using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x0200000C RID: 12
	internal class EarthLightningMerge : SpellMergeData
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00004783 File Offset: 0x00002983
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.stormEffectData = Catalog.GetData<EffectData>(this.stormEffectId, true);
			this.spikesCollisionEffectData = Catalog.GetData<EffectData>(this.spikesCollisionEffectId, true);
			this.stormStartEffectData = Catalog.GetData<EffectData>(this.stormStartEffectId, true);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000047C4 File Offset: 0x000029C4
		public override void Merge(bool active)
		{
			base.Merge(active);
			if (active)
			{
				bool flag = this.cloudEffectInstance != null;
				if (flag)
				{
					this.cloudEffectInstance.Despawn();
				}
				this.cloudEffectInstance = this.stormStartEffectData.Spawn(Player.currentCreature.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
				this.cloudEffectInstance.Play(0, false);
			}
			else
			{
				Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
				Vector3 vector2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
				bool flag2 = vector.magnitude > SpellCaster.throwMinHandVelocity && vector2.magnitude > SpellCaster.throwMinHandVelocity;
				if (flag2)
				{
					bool flag3 = Vector3.Angle(vector, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) < 45f || Vector3.Angle(vector2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) < 45f;
					if (flag3)
					{
						bool flag4 = this.currentCharge > this.stormMinCharge && !EarthBendingController.LightningActive;
						if (flag4)
						{
							EarthBendingController.LightningActive = true;
							this.mana.StartCoroutine(this.StormCoroutine());
							this.mana.StartCoroutine(this.DespawnEffectDelay(this.cloudEffectInstance, 15f));
							this.currentCharge = 0f;
							return;
						}
					}
				}
				this.mana.StartCoroutine(this.DespawnEffectDelay(this.cloudEffectInstance, 1f));
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000499F File Offset: 0x00002B9F
		public IEnumerator StormCoroutine()
		{
			Vector3 playerPos = Player.currentCreature.transform.position;
			foreach (Creature creature in Creature.allActive)
			{
				bool flag = creature != Player.currentCreature;
				if (flag)
				{
					bool flag2 = creature.state > 0;
					if (flag2)
					{
						float dist = Vector3.Distance(playerPos, creature.transform.position);
						bool flag3 = dist < this.stormRadius;
						if (flag3)
						{
							EffectInstance stormInst = this.stormEffectData.Spawn(creature.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
							stormInst.Play(0, false);
							foreach (ParticleSystem particleSystem in stormInst.effects[0].gameObject.GetComponentsInChildren<ParticleSystem>())
							{
								bool flag4 = particleSystem.gameObject.name == "CollisionDetector";
								if (flag4)
								{
									ElectricSpikeCollision scr = particleSystem.gameObject.AddComponent<ElectricSpikeCollision>();
									scr.part = particleSystem;
									scr.spikesCollisionEffectData = this.spikesCollisionEffectData;
									scr = null;
								}
								particleSystem = null;
							}
							ParticleSystem[] array = null;
							this.mana.StartCoroutine(this.DespawnEffectDelay(stormInst, 15f));
							yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
							stormInst = null;
						}
					}
				}
				else
				{
					EffectInstance stormInst2 = this.stormEffectData.Spawn(creature.transform.position + creature.transform.forward * 2f, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
					stormInst2.Play(0, false);
					foreach (ParticleSystem particleSystem2 in stormInst2.effects[0].gameObject.GetComponentsInChildren<ParticleSystem>())
					{
						bool flag5 = particleSystem2.gameObject.name == "CollisionDetector";
						if (flag5)
						{
							ElectricSpikeCollision scr2 = particleSystem2.gameObject.AddComponent<ElectricSpikeCollision>();
							scr2.part = particleSystem2;
							scr2.spikesCollisionEffectData = this.spikesCollisionEffectData;
							scr2 = null;
						}
						particleSystem2 = null;
					}
					ParticleSystem[] array2 = null;
					this.mana.StartCoroutine(this.DespawnEffectDelay(stormInst2, 15f));
					stormInst2 = null;
				}
				creature = null;
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			yield return new WaitForSeconds(10f);
			EarthBendingController.LightningActive = false;
			yield break;
			yield break;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000049AE File Offset: 0x00002BAE
		private IEnumerator DespawnEffectDelay(EffectInstance effect, float delay)
		{
			yield return new WaitForSeconds(delay);
			effect.Despawn();
			yield break;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000049CB File Offset: 0x00002BCB
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

		// Token: 0x04000094 RID: 148
		public float stormMinCharge;

		// Token: 0x04000095 RID: 149
		public float stormRadius;

		// Token: 0x04000096 RID: 150
		public string stormEffectId;

		// Token: 0x04000097 RID: 151
		public string stormStartEffectId;

		// Token: 0x04000098 RID: 152
		public string spikesCollisionEffectId;

		// Token: 0x04000099 RID: 153
		private EffectData stormEffectData;

		// Token: 0x0400009A RID: 154
		private EffectData spikesCollisionEffectData;

		// Token: 0x0400009B RID: 155
		private EffectData stormStartEffectData;

		// Token: 0x0400009C RID: 156
		private EffectInstance cloudEffectInstance;
	}
}
