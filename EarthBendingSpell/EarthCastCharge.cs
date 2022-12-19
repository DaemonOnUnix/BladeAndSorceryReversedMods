using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000002 RID: 2
	public class EarthCastCharge : SpellCastCharge
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Init()
		{
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			base.Init();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
		private void EventManager_onPossess(Creature creature, EventTime eventTime)
		{
			bool flag = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellEarthItem").Count<ContainerData.Content>() <= 0;
			if (flag)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellEarthItem", true), null, null);
			}
			bool flag2 = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellEarthFireMerge").Count<ContainerData.Content>() <= 0;
			if (flag2)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellEarthFireMerge", true), null, null);
			}
			bool flag3 = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellEarthIceMerge").Count<ContainerData.Content>() <= 0;
			if (flag3)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellEarthIceMerge", true), null, null);
			}
			bool flag4 = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellMeteorBarrage").Count<ContainerData.Content>() <= 0;
			if (flag4)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellMeteorBarrage", true), null, null);
			}
			bool flag5 = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "LightningStorm").Count<ContainerData.Content>() <= 0;
			if (flag5)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("LightningStorm", true), null, null);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002230 File Offset: 0x00000430
		public override void Fire(bool active)
		{
			base.Fire(active);
			bool flag = !active;
			if (flag)
			{
				this.chargeEffectInstance.Despawn();
			}
			bool flag2 = !this.spellCaster.mana.gameObject.GetComponent<EarthBendingController>();
			if (flag2)
			{
				this.spellCaster.mana.gameObject.AddComponent<EarthBendingController>();
				EarthBendingController component = this.spellCaster.mana.gameObject.GetComponent<EarthBendingController>();
				component.shieldMinSpeed = this.shieldMinSpeed;
				component.shieldItemId = this.shieldItemId;
				component.shieldFreezeTime = this.shieldFreezeTime;
				component.shieldHealth = this.shieldHealth;
				component.shieldPushMul = this.shieldPushMul;
				component.pushEffectId = this.pushEffectId;
				component.pushMinSpeed = this.pushMinSpeed;
				component.pushForce = this.pushForce;
				component.rockItemIds = this.rockItemIds;
				component.rockForceMul = this.rockForceMul;
				component.rockFreezeTime = this.rockFreezeTime;
				component.rockHeightFromGround = this.rockHeightFromGround;
				component.rockMoveSpeed = this.rockMoveSpeed;
				component.rockMassMinMax = this.rockMassMinMax;
				component.rockSummonEffectId = this.rockSummonEffectId;
				component.punchForce = this.punchForce;
				component.punchEffectId = this.punchEffectId;
				component.punchAimPrecision = this.punchAimPrecision;
				component.punchAimRandomness = this.punchAimRandomness;
				component.spikeMinSpeed = this.spikeMinSpeed;
				component.spikeEffectId = this.spikeEffectId;
				component.spikeRange = this.spikeRange;
				component.spikeMinAngle = this.spikeMinAngle;
				component.spikeDamage = this.spikeDamage;
				component.shatterEffectId = this.shatterEffectId;
				component.shatterForce = this.shatterForce;
				component.shatterMinSpeed = this.shatterMinSpeed;
				component.shatterRadius = this.shatterRadius;
				component.shatterRange = this.shatterRange;
				component.rockPillarPointsId = this.rockPillarPointsId;
				component.rockPillarItemId = this.rockPillarItemId;
				component.rockPillarCollisionEffectId = this.rockPillarCollisionEffectId;
				component.rockPillarMinSpeed = this.rockPillarMinSpeed;
				component.rockPillarLifeTime = this.rockPillarLifeTime;
				component.rockPillarSpawnDelay = this.rockPillarSpawnDelay;
				component.Initialize();
			}
			EarthBendingController component2 = this.spellCaster.mana.gameObject.GetComponent<EarthBendingController>();
			bool flag3 = this.spellCaster.ragdollHand.side == 1;
			if (flag3)
			{
				component2.leftHandActive = active;
			}
			else
			{
				component2.rightHandActive = active;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000249C File Offset: 0x0000069C
		public override bool OnImbueCollisionStart(CollisionInstance collisionInstance)
		{
			base.OnImbueCollisionStart(collisionInstance);
			bool flag = collisionInstance.damageStruct.hitRagdollPart;
			if (flag)
			{
				Creature creature = collisionInstance.damageStruct.hitRagdollPart.ragdoll.creature;
				bool flag2 = creature != Player.currentCreature;
				if (flag2)
				{
					RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
					bool flag3 = !hitRagdollPart.GetComponent<EarthBendingRagdollPart>();
					if (flag3)
					{
						EarthBendingRagdollPart earthBendingRagdollPart = hitRagdollPart.gameObject.AddComponent<EarthBendingRagdollPart>();
						earthBendingRagdollPart.ragdollPart = hitRagdollPart;
						earthBendingRagdollPart.Initialize();
					}
					else
					{
						bool flag4 = collisionInstance.damageStruct.damageType == 3;
						if (flag4)
						{
							bool dismembermentAllowed = hitRagdollPart.data.bodyDamagerData.dismembermentAllowed;
							if (dismembermentAllowed)
							{
								creature.ragdoll.Slice(hitRagdollPart);
								creature.Kill();
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002580 File Offset: 0x00000780
		public override bool OnCrystalSlam(CollisionInstance collisionInstance)
		{
			this.imbue.colliderGroup.collisionHandler.item.StartCoroutine(this.RockShockWaveCoroutine(collisionInstance.contactPoint, collisionInstance.contactNormal, collisionInstance.sourceColliderGroup.transform.up, collisionInstance.impactVelocity));
			this.imbueHitGroundLastTime = Time.time;
			return true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000025E1 File Offset: 0x000007E1
		private IEnumerator RockShockWaveCoroutine(Vector3 contactPoint, Vector3 contactNormal, Vector3 contactNormalUpward, Vector3 impactVelocity)
		{
			EffectInstance effectInstance = Catalog.GetData<EffectData>(this.imbueHitGroundEffectId, true).Spawn(contactPoint, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			effectInstance.Play(0, false);
			yield return new WaitForSeconds(0.4f);
			Collider[] sphereCast = Physics.OverlapSphere(contactPoint, this.imbueHitGroundRadius);
			foreach (Collider collider in sphereCast)
			{
				bool flag = collider.attachedRigidbody;
				if (flag)
				{
					bool flag2 = collider.attachedRigidbody.gameObject.layer != GameManager.GetLayer(10) && this.imbue.colliderGroup.collisionHandler.rb != collider.attachedRigidbody;
					if (flag2)
					{
						collider.attachedRigidbody.AddExplosionForce(this.imbueHitGroundExplosionForce, contactPoint, this.imbueHitGroundRadius * 2f, this.imbueHitGroundExplosionUpwardModifier, 1);
					}
					bool flag3 = collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(10) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(13);
					if (flag3)
					{
						Creature creature = collider.GetComponentInParent<Creature>();
						bool flag4 = creature != Player.currentCreature && !creature.isKilled;
						if (flag4)
						{
							creature.ragdoll.SetState(1, false);
							collider.attachedRigidbody.AddExplosionForce(this.imbueHitGroundExplosionForce, contactPoint, this.imbueHitGroundRadius * 2f, this.imbueHitGroundExplosionUpwardModifier, 1);
						}
						creature = null;
					}
				}
				collider = null;
			}
			Collider[] array = null;
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002610 File Offset: 0x00000810
		public override bool OnCrystalUse(RagdollHand hand, bool active)
		{
			Debug.Log("crystal use");
			bool flag = !active;
			if (flag)
			{
				Debug.Log("not active");
				bool flag2 = !this.imbue.colliderGroup.collisionHandler.item;
				Rigidbody rb;
				if (flag2)
				{
					RagdollPart ragdollPart = this.imbue.colliderGroup.collisionHandler.ragdollPart;
					rb = ((ragdollPart != null) ? ragdollPart.rb : null);
				}
				else
				{
					rb = this.imbue.colliderGroup.collisionHandler.item.rb;
				}
				rb.GetPointVelocity(this.imbue.colliderGroup.imbueShoot.position);
				bool flag3 = rb.GetPointVelocity(this.imbue.colliderGroup.imbueShoot.position).magnitude > SpellCaster.throwMinHandVelocity && this.imbue.CanConsume(this.imbueCrystalUseCost);
				if (flag3)
				{
					Debug.Log("magnitude good");
					this.imbue.ConsumeInstant(this.imbueCrystalUseCost);
					Catalog.GetData<ItemData>("Rock Weapon 1", true).SpawnAsync(delegate(Item rock)
					{
						rock.transform.position = this.imbue.colliderGroup.imbueShoot.position;
						rock.transform.rotation = this.imbue.colliderGroup.imbueShoot.rotation;
						rock.IgnoreObjectCollision(this.imbue.colliderGroup.collisionHandler.item);
						rock.rb.AddForce(this.imbue.colliderGroup.imbueShoot.forward * this.imbueCrystalShootForce * rb.GetPointVelocity(this.imbue.colliderGroup.imbueShoot.position).magnitude, 1);
						rock.Throw(1f, 2);
					}, null, null, null, true, null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000001 RID: 1
		public float shieldMinSpeed;

		// Token: 0x04000002 RID: 2
		public string shieldItemId;

		// Token: 0x04000003 RID: 3
		public float shieldFreezeTime;

		// Token: 0x04000004 RID: 4
		public int shieldHealth;

		// Token: 0x04000005 RID: 5
		public float shieldPushMul;

		// Token: 0x04000006 RID: 6
		public float pushMinSpeed;

		// Token: 0x04000007 RID: 7
		public string pushEffectId;

		// Token: 0x04000008 RID: 8
		public float pushForce;

		// Token: 0x04000009 RID: 9
		public List<string> rockItemIds = new List<string>();

		// Token: 0x0400000A RID: 10
		public Vector2 rockMassMinMax;

		// Token: 0x0400000B RID: 11
		public float rockForceMul;

		// Token: 0x0400000C RID: 12
		public float rockFreezeTime;

		// Token: 0x0400000D RID: 13
		public float rockHeightFromGround;

		// Token: 0x0400000E RID: 14
		public float rockMoveSpeed;

		// Token: 0x0400000F RID: 15
		public string rockSummonEffectId;

		// Token: 0x04000010 RID: 16
		public float punchForce;

		// Token: 0x04000011 RID: 17
		public string punchEffectId;

		// Token: 0x04000012 RID: 18
		public float punchAimPrecision;

		// Token: 0x04000013 RID: 19
		public float punchAimRandomness;

		// Token: 0x04000014 RID: 20
		public string spikeEffectId;

		// Token: 0x04000015 RID: 21
		public float spikeMinSpeed;

		// Token: 0x04000016 RID: 22
		public float spikeRange;

		// Token: 0x04000017 RID: 23
		public float spikeMinAngle;

		// Token: 0x04000018 RID: 24
		public float spikeDamage;

		// Token: 0x04000019 RID: 25
		public string shatterEffectId;

		// Token: 0x0400001A RID: 26
		public float shatterMinSpeed;

		// Token: 0x0400001B RID: 27
		public float shatterRange;

		// Token: 0x0400001C RID: 28
		public float shatterRadius;

		// Token: 0x0400001D RID: 29
		public float shatterForce;

		// Token: 0x0400001E RID: 30
		public float rockPillarMinSpeed;

		// Token: 0x0400001F RID: 31
		public string rockPillarPointsId;

		// Token: 0x04000020 RID: 32
		public string rockPillarItemId;

		// Token: 0x04000021 RID: 33
		public string rockPillarCollisionEffectId;

		// Token: 0x04000022 RID: 34
		public float rockPillarLifeTime;

		// Token: 0x04000023 RID: 35
		public float rockPillarSpawnDelay;

		// Token: 0x04000024 RID: 36
		public string imbueHitGroundEffectId;

		// Token: 0x04000025 RID: 37
		public float imbueHitGroundConsumption;

		// Token: 0x04000026 RID: 38
		public float imbueHitGroundExplosionUpwardModifier;

		// Token: 0x04000027 RID: 39
		public float imbueHitGroundRechargeDelay;

		// Token: 0x04000028 RID: 40
		public float imbueHitGroundMinVelocity;

		// Token: 0x04000029 RID: 41
		public float imbueHitGroundRadius;

		// Token: 0x0400002A RID: 42
		public float imbueHitGroundExplosionForce;

		// Token: 0x0400002B RID: 43
		public float imbueCrystalUseCost;

		// Token: 0x0400002C RID: 44
		public float imbueCrystalShootForce;

		// Token: 0x0400002D RID: 45
		private float imbueHitGroundLastTime;
	}
}
