using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace MergesSpellsUp
{
	// Token: 0x02000009 RID: 9
	public class MergeFireMergeUp : SpellMergeData
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00008348 File Offset: 0x00006548
		public override void Load(Mana mana)
		{
			base.Load(mana);
			this.meteorsThrown = false;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000835A File Offset: 0x0000655A
		public override void Merge(bool active)
		{
			base.Merge(active);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00008365 File Offset: 0x00006565
		public override void FireAxis(float value, Side side)
		{
			base.FireAxis(value, side);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008374 File Offset: 0x00006574
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			Vector3 from = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = !this.meteorsThrown;
			if (flag)
			{
				bool flag2 = vector.magnitude > SpellCaster.throwMinHandVelocity && from.magnitude > SpellCaster.throwMinHandVelocity;
				if (flag2)
				{
					bool flag3 = Vector3.Angle(vector, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) < this.maxAngleOfHands || Vector3.Angle(from, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) < this.maxAngleOfHands;
					if (flag3)
					{
						this.SpawnMeteors();
						this.meteorsThrown = true;
						this.mana.StartCoroutine(this.MeteorCoroutine());
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000084A0 File Offset: 0x000066A0
		public override bool CanMerge()
		{
			return !this.meteorsThrown;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000084BB File Offset: 0x000066BB
		public IEnumerator MeteorCoroutine()
		{
			this.radius = this.radiusInitial;
			while (Time.time - this.timeMeteorsSpawned < this.timeBeforeNewSpawn)
			{
				this.rotation += Time.deltaTime * 120f;
				this.radius += Time.deltaTime * (this.maxDistance / this.timeBeforeNewSpawn);
				bool flag = this.meteorsItemSpawned.Count == this.nbMeteorToSpawn;
				if (flag)
				{
					int num;
					for (int i = 0; i < this.nbMeteorToSpawn; i = num + 1)
					{
						bool flag2 = this.meteorsItemSpawned[i] != null;
						if (flag2)
						{
							this.meteorsItemSpawned[i].transform.position = (Player.local.creature.transform.position + Vector3.up).RotateCircle(Player.local.creature.transform.up, Player.local.creature.transform.forward, this.radius, this.rotation, this.nbMeteorToSpawn, i);
						}
						num = i;
					}
				}
				yield return Yielders.FixedUpdate;
			}
			bool flag3 = this.timeMeteorsSpawned <= Time.time - this.timeBeforeNewSpawn;
			if (flag3)
			{
				int num;
				for (int j = 0; j < this.nbMeteorToSpawn; j = num + 1)
				{
					bool flag4 = this.meteorsItemSpawned[j] != null;
					if (flag4)
					{
						this.meteorsItemSpawned[j].rb.velocity = Vector3.zero;
						this.meteorsItemSpawned[j].rb.AddForce(-Vector3.Cross(this.meteorsItemSpawned[j].transform.position - (Player.local.creature.transform.position + Vector3.up), Vector3.up).normalized * 20f, 2);
					}
					num = j;
				}
				this.meteorsThrown = false;
				this.rotation = 0f;
				this.radius = this.radiusInitial;
				this.meteorsItemSpawned.Clear();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000084CC File Offset: 0x000066CC
		public void SpawnMeteors()
		{
			for (int i = 0; i < this.nbMeteorToSpawn; i++)
			{
				this.meteorsItemSpawned.Add(this.ThrowMeteoroid((Player.local.creature.transform.position + Vector3.up).PosAroundCircle(Player.local.creature.transform.up, Player.local.creature.transform.forward, this.radiusInitial, this.nbMeteorToSpawn, i), Player.currentCreature, false));
			}
			this.timeMeteorsSpawned = Time.time;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00008570 File Offset: 0x00006770
		public override void Unload()
		{
			base.Unload();
			this.meteorsThrown = false;
			this.rotation = 0f;
			this.radius = this.radiusInitial;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008598 File Offset: 0x00006798
		public Item ThrowMeteoroid(Vector3 origin, Creature thrower, bool useGravity = true)
		{
			Item meteor = new Item();
			EffectData meteorEffectData = Catalog.GetData<EffectData>("Meteor", true);
			EffectData meteorExplosionEffectData = Catalog.GetData<EffectData>("MeteorExplosion", true);
			float meteorVelocity = 7f;
			float meteorExplosionDamage = 20f;
			float meteorExplosionPlayerDamage = 0f;
			float meteorExplosionRadius = 10f;
			AnimationCurve meteorIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1f);
			SpellCastCharge meteorImbueSpellData = Catalog.GetData<SpellCastCharge>("Fire", true);
			ItemMagicAreaProjectile projectile;
			ItemMagicAreaProjectile.HandlerHit <>9__1;
			ItemMagicAreaProjectile.HandlerAreaHit <>9__2;
			ItemMagicAreaProjectile.CreatureAreaHit <>9__3;
			ItemMagicAreaProjectile.Hit <>9__4;
			Catalog.GetData<ItemData>("Meteor", true).SpawnAsync(delegate(Item item)
			{
				item.disallowDespawn = true;
				item.rb.useGravity = useGravity;
				item.IgnoreRagdollCollision(thrower.ragdoll);
				ItemMagicAreaProjectile component = item.GetComponent<ItemMagicAreaProjectile>();
				bool flag = component != null;
				if (flag)
				{
					projectile = component;
					component.explosionEffectData = Catalog.GetData<EffectData>("MeteorExplosion", true);
					component.areaRadius = meteorExplosionRadius;
					ItemMagicAreaProjectile itemMagicAreaProjectile = component;
					ItemMagicAreaProjectile.HandlerHit handlerHit;
					if ((handlerHit = <>9__1) == null)
					{
						handlerHit = (<>9__1 = delegate(CollisionInstance hit, CollisionHandler handler)
						{
							bool flag2 = !handler.isItem;
							if (!flag2)
							{
								this.MeteorImbueItem(hit.targetColliderGroup);
							}
						});
					}
					itemMagicAreaProjectile.OnHandlerHit += handlerHit;
					ItemMagicAreaProjectile itemMagicAreaProjectile2 = component;
					ItemMagicAreaProjectile.HandlerAreaHit handlerAreaHit;
					if ((handlerAreaHit = <>9__2) == null)
					{
						handlerAreaHit = (<>9__2 = delegate(Collider collider, CollisionHandler handler)
						{
							bool flag2 = !handler.isItem;
							if (!flag2)
							{
								this.MeteorImbueItem(collider.GetComponentInParent<ColliderGroup>());
							}
						});
					}
					itemMagicAreaProjectile2.OnHandlerAreaHit += handlerAreaHit;
					ItemMagicAreaProjectile itemMagicAreaProjectile3 = component;
					ItemMagicAreaProjectile.CreatureAreaHit creatureAreaHit;
					if ((creatureAreaHit = <>9__3) == null)
					{
						creatureAreaHit = (<>9__3 = delegate(Collider collider, Creature creature)
						{
							creature.Damage(new CollisionInstance(new DamageStruct(4, creature.isPlayer ? meteorExplosionPlayerDamage : meteorExplosionDamage), null, null));
						});
					}
					itemMagicAreaProjectile3.OnCreatureAreaHit += creatureAreaHit;
					ItemMagicAreaProjectile itemMagicAreaProjectile4 = component;
					ItemMagicAreaProjectile.Hit hit2;
					if ((hit2 = <>9__4) == null)
					{
						hit2 = (<>9__4 = delegate(CollisionInstance collision)
						{
							this.MeteorExplosion(collision.contactPoint, meteorExplosionRadius, thrower);
						});
					}
					itemMagicAreaProjectile4.OnHit += hit2;
					component.guidance = 0;
					component.guidanceAmount = 0f;
					component.speed = meteorVelocity;
					component.effectIntensityCurve = meteorIntensityCurve;
					component.Fire(Vector3.forward, meteorEffectData, null, Player.currentCreature.ragdoll);
				}
				meteor = item;
			}, new Vector3?(origin), new Quaternion?(Quaternion.identity), null, true, null);
			return meteor;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000867E File Offset: 0x0000687E
		private void MeteorImbueItem(ColliderGroup group)
		{
			if (group != null)
			{
				Imbue imbue = group.imbue;
				if (imbue != null)
				{
					imbue.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true), group.imbue.maxEnergy * 2f);
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000086B4 File Offset: 0x000068B4
		private void MeteorExplosion(Vector3 position, float radius, Creature thrower)
		{
			HashSet<Rigidbody> rigidbodySet = new HashSet<Rigidbody>();
			HashSet<Creature> hitCreatures = new HashSet<Creature>();
			float meteorExplosionForce = 20f;
			float meteorExplosionPlayerForce = 0f;
			LayerMask explosionLayerMask = 232799233;
			foreach (Collider collider in Physics.OverlapSphere(position, radius, explosionLayerMask, 1))
			{
				bool flag = collider.attachedRigidbody && !rigidbodySet.Contains(collider.attachedRigidbody);
				if (flag)
				{
					float explosionForce = meteorExplosionForce;
					Creature componentInParent = collider.attachedRigidbody.GetComponentInParent<Creature>();
					bool flag2 = componentInParent != null && componentInParent != thrower && !componentInParent.isKilled && !componentInParent.isPlayer && !hitCreatures.Contains(componentInParent);
					if (flag2)
					{
						componentInParent.ragdoll.SetState(1);
						hitCreatures.Add(componentInParent);
					}
					bool flag3 = collider.attachedRigidbody.GetComponentInParent<Player>() != null;
					if (flag3)
					{
						explosionForce = meteorExplosionPlayerForce;
					}
					rigidbodySet.Add(collider.attachedRigidbody);
					collider.attachedRigidbody.AddExplosionForce(explosionForce, position, radius, 1f, 2);
				}
			}
		}

		// Token: 0x0400006A RID: 106
		private float maxAngleOfHands = 45f;

		// Token: 0x0400006B RID: 107
		private bool meteorsThrown = false;

		// Token: 0x0400006C RID: 108
		private float timeBeforeNewSpawn = 5f;

		// Token: 0x0400006D RID: 109
		private float timeMeteorsSpawned;

		// Token: 0x0400006E RID: 110
		public int nbMeteorToSpawn = 5;

		// Token: 0x0400006F RID: 111
		public float maxDistance = 4f;

		// Token: 0x04000070 RID: 112
		private List<Item> meteorsItemSpawned = new List<Item>();

		// Token: 0x04000071 RID: 113
		private float radiusInitial = 1.5f;

		// Token: 0x04000072 RID: 114
		private float radius = 0f;

		// Token: 0x04000073 RID: 115
		private float rotation = 0f;
	}
}
