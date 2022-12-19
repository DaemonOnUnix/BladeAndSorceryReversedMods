using System;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x0200000A RID: 10
	internal class SpellMergeIceFire : SpellMergeData
	{
		// Token: 0x06000033 RID: 51 RVA: 0x0000358C File Offset: 0x0000178C
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			bool flag = this.beamEffectId != null && this.beamEffectId != "";
			if (flag)
			{
				this.effectData = Catalog.GetData<EffectData>(this.beamEffectId, true);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035D4 File Offset: 0x000017D4
		public override void Update()
		{
			base.Update();
			bool flag = this.currentCharge > this.minCharge && !this.playing;
			if (flag)
			{
				this.currentCharge = 0f;
				this.Fire();
			}
			else
			{
				bool flag2 = this.playing;
				if (flag2)
				{
					this.currentCharge = 0f;
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003634 File Offset: 0x00001834
		public async void Fire()
		{
			this.playing = true;
			this.effectInstance = this.effectData.Spawn(this.mana.mergePoint.position, this.mana.mergePoint.rotation, null, null, true, null, false, Array.Empty<Type>());
			this.effectInstance.Play(0, false);
			await Task.Delay(2300);
			Collider[] hit = Physics.OverlapCapsule(this.mana.mergePoint.position, this.mana.mergePoint.position + this.mana.mergePoint.forward * 8f, 0.7f);
			foreach (Collider collider in hit)
			{
				if (collider.attachedRigidbody)
				{
					if (collider.GetComponentInParent<Creature>())
					{
						Creature creature = collider.GetComponentInParent<Creature>();
						if (!creature.isPlayer)
						{
							if (!creature.isKilled)
							{
								creature.ragdoll.SetState(2);
								collider.attachedRigidbody.AddForce(this.mana.mergePoint.forward * 30f, 1);
								CollisionInstance l_Dmg = new CollisionInstance(new DamageStruct(4, this.damage), null, null);
								creature.Damage(l_Dmg);
								l_Dmg = null;
							}
						}
						creature = null;
					}
				}
				collider = null;
			}
			Collider[] array = null;
			await Task.Delay(2300);
			this.effectInstance.Stop(0);
			this.playing = false;
		}

		// Token: 0x04000022 RID: 34
		public float minCharge;

		// Token: 0x04000023 RID: 35
		public string beamEffectId;

		// Token: 0x04000024 RID: 36
		public float damage;

		// Token: 0x04000025 RID: 37
		public float force;

		// Token: 0x04000026 RID: 38
		private EffectData effectData;

		// Token: 0x04000027 RID: 39
		private EffectInstance effectInstance;

		// Token: 0x04000028 RID: 40
		public bool playing;
	}
}
