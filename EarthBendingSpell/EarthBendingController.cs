using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000004 RID: 4
	public class EarthBendingController : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002B44 File Offset: 0x00000D44
		public void Initialize()
		{
			this.mana = Player.currentCreature.mana;
			this.rockPillarCollisionEffectData = Catalog.GetData<EffectData>(this.rockPillarCollisionEffectId, true);
			this.rockPillarPointsData = Catalog.GetData<EffectData>(this.rockPillarPointsId, true);
			this.rockPillarItemData = Catalog.GetData<ItemData>(this.rockPillarItemId, true);
			this.shatterEffectData = Catalog.GetData<EffectData>(this.shatterEffectId, true);
			this.spikeEffectData = Catalog.GetData<EffectData>(this.spikeEffectId, true);
			this.rockSummonEffectData = Catalog.GetData<EffectData>(this.rockSummonEffectId, true);
			this.punchEffectData = Catalog.GetData<EffectData>(this.punchEffectId, true);
			this.pushEffectData = Catalog.GetData<EffectData>(this.pushEffectId, true);
			this.shieldItemData = Catalog.GetData<ItemData>(this.shieldItemId, true);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002C04 File Offset: 0x00000E04
		private void Update()
		{
			this.leftVel = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			this.rightVel = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = Vector3.Dot(Vector3.down, this.mana.casterLeft.magic.forward) > 0.8f && Vector3.Dot(Vector3.down, this.mana.casterRight.magic.forward) > 0.8f;
			if (flag)
			{
				this.handsPointingDown = true;
			}
			else
			{
				this.handsPointingDown = false;
			}
			this.UpdateValues();
			bool flag2 = this.leftHandActive && this.rightHandActive;
			if (flag2)
			{
				bool flag3 = !PlayerControl.GetHand(0).gripPressed && !PlayerControl.GetHand(1).gripPressed;
				if (flag3)
				{
					this.UpdatePush();
					this.UpdateSpike();
				}
				else
				{
					bool flag4 = PlayerControl.GetHand(0).gripPressed && PlayerControl.GetHand(1).gripPressed;
					if (flag4)
					{
						this.UpdateShatter();
						this.UpdateShield();
						this.UpdateRockPillar();
					}
				}
			}
			this.UpdateRock();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002D54 File Offset: 0x00000F54
		private void UpdateValues()
		{
			bool flag = !this.leftHandActive || !this.leftHandActive;
			if (flag)
			{
				this.canSpawnShield = true;
				this.canSpawnSpikes = true;
				this.canSpawnShatter = true;
				this.canPush = true;
				bool flag2 = !this.leftHandActive;
				if (flag2)
				{
					this.canSpawnRockLeft = true;
				}
				bool flag3 = !this.rightHandActive;
				if (flag3)
				{
					this.canSpawnRockRight = true;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private void UpdateShield()
		{
			bool flag = this.wallCalled;
			if (flag)
			{
				this.wallCalled = false;
			}
			bool flag2 = this.canSpawnShield;
			if (flag2)
			{
				bool flag3 = this.handsPointingDown;
				if (flag3)
				{
					bool flag4 = (double)Mathf.Abs(Vector3.Dot(-Player.currentCreature.transform.right, this.mana.casterLeft.magic.up)) < 0.4 && (double)Mathf.Abs(Vector3.Dot(Player.currentCreature.transform.right, this.mana.casterLeft.magic.up)) < 0.4;
					if (flag4)
					{
						bool flag5 = Vector3.Dot(Vector3.up, this.leftVel) > this.shieldMinSpeed && Vector3.Dot(Vector3.up, this.rightVel) > this.shieldMinSpeed;
						if (flag5)
						{
							base.StartCoroutine(this.SpawnShieldCoroutine());
							this.wallCalled = true;
							this.canSpawnShield = false;
							this.abilityUsed = true;
						}
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002EE4 File Offset: 0x000010E4
		private void UpdatePush()
		{
			bool flag = this.pushCalled;
			if (flag)
			{
				this.pushCalled = false;
			}
			bool flag2 = Mathf.Abs(Vector3.Dot(Vector3.up, this.mana.casterLeft.magic.forward)) < 0.3f && Mathf.Abs(Vector3.Dot(Vector3.up, this.mana.casterRight.magic.forward)) < 0.3f;
			if (flag2)
			{
				bool flag3 = Vector3.Dot(Vector3.up, this.mana.casterLeft.magic.up) > 0.7f && Vector3.Dot(Vector3.up, this.mana.casterRight.magic.up) > 0.7f;
				if (flag3)
				{
					bool flag4 = Vector3.Dot(this.mana.casterLeft.magic.forward, this.leftVel) > this.pushMinSpeed && Vector3.Dot(this.mana.casterLeft.magic.forward, this.rightVel) > this.pushMinSpeed;
					if (flag4)
					{
						bool flag5 = this.canPush;
						if (flag5)
						{
							this.canPush = false;
							this.pushCalled = true;
							this.abilityUsed = true;
						}
					}
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003038 File Offset: 0x00001238
		private void UpdateSpike()
		{
			Vector3 normalized = Vector3.Slerp(Vector3.up, Player.currentCreature.transform.forward, 0.5f).normalized;
			bool flag = this.canSpawnSpikes;
			if (flag)
			{
				bool flag2 = Vector3.Dot(this.mana.casterLeft.magic.forward, normalized) > 0.7f && Vector3.Dot(this.mana.casterRight.magic.forward, normalized) > 0.7f;
				if (flag2)
				{
					bool flag3 = (double)Vector3.Dot(this.mana.casterLeft.magic.right, -Player.currentCreature.transform.right) > 0.7 && (double)Vector3.Dot(this.mana.casterRight.magic.right, -Player.currentCreature.transform.right) > 0.7;
					if (flag3)
					{
						bool flag4 = Vector3.Dot(this.mana.casterLeft.magic.forward, this.leftVel) > this.spikeMinSpeed && Vector3.Dot(this.mana.casterLeft.magic.forward, this.rightVel) > this.spikeMinSpeed;
						if (flag4)
						{
							base.StartCoroutine(this.SpikeCoroutine());
							this.canSpawnSpikes = false;
							this.abilityUsed = true;
						}
					}
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000031C4 File Offset: 0x000013C4
		private void UpdateRock()
		{
			bool flag = this.leftHandActive;
			if (flag)
			{
				bool flag2 = !PlayerControl.GetHand(1).gripPressed;
				if (flag2)
				{
					bool flag3 = this.canSpawnRockLeft;
					if (flag3)
					{
						bool flag4 = Vector3.Dot(Vector3.down, this.mana.casterLeft.magic.forward) > 0.7f;
						if (flag4)
						{
							bool flag5 = (double)Mathf.Abs(Vector3.Dot(-Player.currentCreature.transform.right, this.mana.casterLeft.magic.up)) < 0.7;
							if (flag5)
							{
								bool flag6 = Vector3.Dot(Vector3.up, this.leftVel) > this.shieldMinSpeed;
								if (flag6)
								{
									base.StartCoroutine(this.SpawnRockCoroutine(this.mana.casterLeft));
									this.canSpawnRockLeft = false;
								}
							}
						}
					}
				}
			}
			bool flag7 = this.rightHandActive;
			if (flag7)
			{
				bool flag8 = !PlayerControl.GetHand(0).gripPressed;
				if (flag8)
				{
					bool flag9 = this.canSpawnRockRight;
					if (flag9)
					{
						bool flag10 = Vector3.Dot(Vector3.down, this.mana.casterRight.magic.forward) > 0.7f;
						if (flag10)
						{
							bool flag11 = (double)Mathf.Abs(Vector3.Dot(-Player.currentCreature.transform.right, this.mana.casterRight.magic.up)) < 0.7;
							if (flag11)
							{
								bool flag12 = Vector3.Dot(Vector3.up, this.rightVel) > this.shieldMinSpeed;
								if (flag12)
								{
									base.StartCoroutine(this.SpawnRockCoroutine(this.mana.casterRight));
									this.canSpawnRockRight = false;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000033B0 File Offset: 0x000015B0
		private void UpdateShatter()
		{
			bool flag = this.shatterCalled;
			if (flag)
			{
				this.shatterCalled = false;
			}
			bool flag2 = this.canSpawnShatter;
			if (flag2)
			{
				bool flag3 = this.handsPointingDown;
				if (flag3)
				{
					bool flag4 = Vector3.Dot(Player.currentCreature.transform.right, this.mana.casterLeft.magic.right) > 0.8f && Vector3.Dot(Player.currentCreature.transform.right, this.mana.casterRight.magic.right) > 0.8f;
					if (flag4)
					{
						bool flag5 = Vector3.Dot(this.mana.casterLeft.magic.up, this.leftVel) > this.shatterMinSpeed && Vector3.Dot(this.mana.casterLeft.magic.up, this.rightVel) > this.shatterMinSpeed;
						if (flag5)
						{
							base.StartCoroutine(this.ShatterCoroutine());
							this.shatterCalled = true;
							this.canSpawnShatter = false;
							this.abilityUsed = true;
						}
					}
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000034D8 File Offset: 0x000016D8
		private void UpdateRockPillar()
		{
			bool flag = this.canSpawnPillars;
			if (flag)
			{
				bool flag2 = this.handsPointingDown;
				if (flag2)
				{
					bool flag3 = Vector3.Dot(this.mana.casterLeft.magic.forward, this.leftVel) > this.rockPillarMinSpeed && Vector3.Dot(this.mana.casterLeft.magic.forward, this.rightVel) > this.rockPillarMinSpeed;
					if (flag3)
					{
						base.StartCoroutine(this.PillarDownCoroutine());
						this.canSpawnPillars = false;
						this.abilityUsed = true;
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003575 File Offset: 0x00001775
		private IEnumerator PillarDownCoroutine()
		{
			Vector3 middlePoint = Vector3.Lerp(this.mana.casterLeft.magic.position, this.mana.casterRight.magic.position, 0.5f) + Player.currentCreature.transform.forward * 0.25f;
			RaycastHit hit;
			bool flag = Physics.Raycast(middlePoint, Vector3.down, ref hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "Default" }));
			if (flag)
			{
				EffectInstance spawnPoints = this.rockPillarPointsData.Spawn(hit.point, Player.currentCreature.transform.rotation, null, null, true, null, false, Array.Empty<Type>());
				using (IEnumerator enumerator = spawnPoints.effects[0].transform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EarthBendingController.<>c__DisplayClass74_0 CS$<>8__locals1 = new EarthBendingController.<>c__DisplayClass74_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.child = (Transform)enumerator.Current;
						this.rockPillarItemData.SpawnAsync(delegate(Item rockPillar)
						{
							rockPillar.Throw(1f, 1);
							rockPillar.transform.position = CS$<>8__locals1.child.position;
							rockPillar.transform.rotation = CS$<>8__locals1.child.rotation;
							rockPillar.rb.velocity = -Vector3.up * 2f;
							RockPillarCollision rockPillarCollision = rockPillar.gameObject.AddComponent<RockPillarCollision>();
							rockPillarCollision.effectData = CS$<>8__locals1.<>4__this.rockPillarCollisionEffectData;
							CS$<>8__locals1.<>4__this.StartCoroutine(CS$<>8__locals1.<>4__this.DespawnItemAfterTime(CS$<>8__locals1.<>4__this.rockPillarLifeTime, rockPillar));
						}, null, null, null, true, null);
						yield return new WaitForSeconds(this.rockPillarSpawnDelay);
						CS$<>8__locals1 = null;
					}
				}
				IEnumerator enumerator = null;
				yield return new WaitForSeconds(this.rockPillarLifeTime - this.rockPillarSpawnDelay * (float)spawnPoints.effects[0].transform.childCount);
				spawnPoints = null;
			}
			this.canSpawnPillars = true;
			yield break;
			yield break;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003584 File Offset: 0x00001784
		private IEnumerator DespawnItemAfterTime(float delay, Item item)
		{
			yield return new WaitForSeconds(delay);
			item.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			item.Despawn();
			yield break;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000035A1 File Offset: 0x000017A1
		private IEnumerator ShatterCoroutine()
		{
			Vector3 middlePoint = Vector3.Lerp(this.mana.casterLeft.magic.position, this.mana.casterRight.magic.position, 0.5f) + (this.leftVel.normalized + this.rightVel.normalized) * 0.25f;
			Vector3 forwards = Player.currentCreature.transform.forward;
			RaycastHit hit;
			RaycastHit hitEnd;
			bool flag = Physics.Raycast(middlePoint, Vector3.down, ref hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "Default" })) && Physics.Raycast(middlePoint + forwards * this.shatterRange, Vector3.down, ref hitEnd, 2.5f, LayerMask.GetMask(new string[] { "Default" }));
			if (flag)
			{
				EffectInstance shatter = this.shatterEffectData.Spawn(hit.point, Player.currentCreature.transform.rotation, null, null, true, null, false, Array.Empty<Type>());
				shatter.Play(0, false);
				Collider[] colliders = Physics.OverlapCapsule(hit.point, hit.point + forwards * this.shatterRange, this.shatterRadius);
				foreach (Collider collider in colliders)
				{
					bool flag2 = collider.attachedRigidbody;
					if (flag2)
					{
						bool flag3 = collider.GetComponentInParent<Creature>();
						if (flag3)
						{
							Creature creature = collider.GetComponentInParent<Creature>();
							bool flag4 = creature != Player.currentCreature;
							if (flag4)
							{
								bool flag5 = creature.state == 2;
								if (flag5)
								{
									creature.ragdoll.SetState(1, false);
								}
								base.StartCoroutine(this.AddForceInOneFrame(collider.attachedRigidbody, forwards));
							}
							creature = null;
						}
						else
						{
							bool flag6 = collider.GetComponentInParent<Item>();
							if (flag6)
							{
								Item item = collider.GetComponentInParent<Item>();
								bool flag7 = item.mainHandler;
								if (flag7)
								{
									bool flag8 = item.mainHandler.creature != Player.currentCreature;
									if (flag8)
									{
										base.StartCoroutine(this.AddForceInOneFrame(collider.attachedRigidbody, forwards));
									}
								}
								item = null;
							}
						}
					}
					collider = null;
				}
				Collider[] array = null;
				shatter = null;
				colliders = null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000035B0 File Offset: 0x000017B0
		private IEnumerator AddForceInOneFrame(Rigidbody rb, Vector3 forwards)
		{
			yield return new WaitForEndOfFrame();
			rb.AddForce(this.shatterForce * (forwards.normalized + Vector3.up), 1);
			yield break;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000035CD File Offset: 0x000017CD
		private IEnumerator SpikeCoroutine()
		{
			Vector3 middlePoint = Vector3.Lerp(this.mana.casterLeft.magic.position, this.mana.casterRight.magic.position, 0.5f) + (this.leftVel.normalized + this.rightVel.normalized) * 0.25f;
			Vector3 forwards = Player.currentCreature.transform.forward;
			RaycastHit hit;
			RaycastHit hitEnd;
			bool flag = Physics.Raycast(middlePoint, Vector3.down, ref hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "Default" })) && Physics.Raycast(middlePoint + forwards * this.spikeRange, Vector3.down, ref hitEnd, 2.5f, LayerMask.GetMask(new string[] { "Default" }));
			if (flag)
			{
				EffectInstance spikes = this.spikeEffectData.Spawn(hit.point, Player.currentCreature.transform.rotation, null, null, true, null, false, Array.Empty<Type>());
				spikes.Play(0, false);
				using (List<Creature>.Enumerator enumerator = Creature.allActive.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EarthBendingController.<>c__DisplayClass78_0 CS$<>8__locals1 = new EarthBendingController.<>c__DisplayClass78_0();
						CS$<>8__locals1.creature = enumerator.Current;
						bool flag2 = CS$<>8__locals1.creature != Player.currentCreature;
						if (flag2)
						{
							float dist = Vector3.Distance(CS$<>8__locals1.creature.transform.position, hit.point);
							bool flag3 = dist < this.spikeRange;
							if (flag3)
							{
								Vector3 dir = (CS$<>8__locals1.creature.transform.position - hit.point).normalized;
								bool flag4 = Vector3.Dot(dir, forwards) > this.spikeMinAngle;
								if (flag4)
								{
									bool flag5 = CS$<>8__locals1.creature.state > 0;
									if (flag5)
									{
										CS$<>8__locals1.creature.ragdoll.SetState(1, false);
										CollisionInstance collisionStruct = new CollisionInstance(new DamageStruct(1, this.spikeDamage), null, null);
										CS$<>8__locals1.creature.Damage(collisionStruct);
										Catalog.GetData<ItemData>("EarthBendingRockSpikes", true).SpawnAsync(delegate(Item spike)
										{
											spike.transform.position = CS$<>8__locals1.creature.transform.position;
											spike.transform.rotation = Player.currentCreature.transform.rotation;
											Transform customReference = spike.GetCustomReference("Mesh");
											customReference.localScale = new Vector3(0.15f, 0.15f, 0.15f);
											spike.transform.localEulerAngles += new Vector3(35f, 0f, 0f);
											spike.Despawn(5f);
										}, null, null, null, true, null);
										collisionStruct = null;
									}
								}
								dir = default(Vector3);
							}
						}
						CS$<>8__locals1 = null;
					}
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				spikes = null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000035DC File Offset: 0x000017DC
		private IEnumerator SpawnShieldCoroutine()
		{
			Vector3 middlePoint = Vector3.Lerp(this.mana.casterLeft.magic.position, this.mana.casterRight.magic.position, 0.5f) + Player.currentCreature.transform.forward * 0.7f;
			RaycastHit hit;
			bool flag = Physics.Raycast(middlePoint, Vector3.down, ref hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "Default" }));
			if (flag)
			{
				this.shieldItemData.SpawnAsync(delegate(Item shield)
				{
					shield.Throw(1f, 1);
					shield.StartCoroutine(this.ShieldSpawnedCoroutine(shield, hit));
				}, null, null, null, true, null);
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000035EB File Offset: 0x000017EB
		private IEnumerator ShieldSpawnedCoroutine(Item shield, RaycastHit hit)
		{
			shield.AddCustomData<EarthBendingController.ShieldCustomData>(new EarthBendingController.ShieldCustomData(this.shieldHealth));
			Vector3 spawnPoint = hit.point;
			shield.transform.position = spawnPoint;
			shield.transform.rotation = Player.currentCreature.transform.rotation;
			Animation shieldAnimation = shield.GetCustomReference("RockAnim").GetComponent<Animation>();
			shieldAnimation.Play();
			yield return new WaitForSeconds(shieldAnimation.clip.length);
			shield.colliderGroups[0].collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.Shield_OnDamageReceivedEvent);
			float startTime = Time.time;
			while (Time.time - startTime < this.shieldFreezeTime)
			{
				bool flag = this.pushCalled;
				if (flag)
				{
					Vector3 forwards = Player.currentCreature.transform.forward;
					Vector3 dir = (shield.transform.position - Player.currentCreature.transform.position).normalized;
					bool flag2 = Vector3.Dot(dir, forwards) > 0.6f;
					if (flag2)
					{
						base.StartCoroutine(this.ShieldPush(shield, this.leftVel.normalized + this.rightVel.normalized));
						break;
					}
					forwards = default(Vector3);
					dir = default(Vector3);
				}
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitUntil(() => Time.time - startTime > this.shieldFreezeTime);
			shield.colliderGroups[0].collisionHandler.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.Shield_OnDamageReceivedEvent);
			base.StartCoroutine(this.DespawnShield(shield));
			yield break;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003608 File Offset: 0x00001808
		private IEnumerator ShieldPush(Item shield, Vector3 direction)
		{
			shield.rb.constraints = 116;
			shield.rb.isKinematic = false;
			shield.rb.useGravity = true;
			EffectInstance effectInstance = this.pushEffectData.Spawn(shield.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			effectInstance.Play(0, false);
			shield.rb.AddForce(direction * this.pushForce * this.shieldPushMul, 1);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitUntil(() => shield.rb.velocity.magnitude < 8f);
			shield.rb.constraints = 0;
			shield.rb.isKinematic = true;
			shield.rb.useGravity = false;
			yield break;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003628 File Offset: 0x00001828
		private void Shield_OnDamageReceivedEvent(CollisionInstance collisionStruct)
		{
			bool flag = collisionStruct.impactVelocity.magnitude > 2f;
			if (flag)
			{
				bool flag2 = collisionStruct.sourceColliderGroup != null && collisionStruct.sourceColliderGroup == Player.currentCreature.handLeft.colliderGroup;
				bool flag3 = collisionStruct.targetColliderGroup != null && collisionStruct.targetColliderGroup == Player.currentCreature.handLeft.colliderGroup;
				bool flag4 = collisionStruct.sourceColliderGroup != null && collisionStruct.sourceColliderGroup == Player.currentCreature.handRight.colliderGroup;
				bool flag5 = collisionStruct.targetColliderGroup != null && collisionStruct.targetColliderGroup == Player.currentCreature.handRight.colliderGroup;
				Item item = null;
				bool flag6 = collisionStruct.targetCollider.GetComponentInParent<Item>();
				if (flag6)
				{
					bool flag7 = collisionStruct.targetCollider.GetComponentInParent<Item>().itemId == this.shieldItemId;
					if (flag7)
					{
						item = collisionStruct.targetCollider.GetComponentInParent<Item>();
					}
				}
				else
				{
					item = collisionStruct.sourceCollider.GetComponentInParent<Item>();
				}
				bool flag8 = flag2 || flag4;
				if (flag8)
				{
					item = collisionStruct.targetCollider.GetComponentInParent<Item>();
					Collider collider = collisionStruct.targetCollider;
				}
				bool flag9 = flag3 || flag5;
				if (flag9)
				{
					item = collisionStruct.sourceCollider.GetComponentInParent<Item>();
					Collider collider = collisionStruct.sourceCollider;
				}
				EarthBendingController.ShieldCustomData shieldCustomData;
				item.TryGetCustomData<EarthBendingController.ShieldCustomData>(ref shieldCustomData);
				int hp = shieldCustomData.hp;
				bool flag10 = item != null;
				if (flag10)
				{
					bool flag11 = hp - 1 > 0;
					if (flag11)
					{
						bool flag12 = flag2 || flag3;
						if (flag12)
						{
							base.StartCoroutine(this.ShieldPush(item, this.leftVel.normalized.normalized));
						}
						bool flag13 = flag4 || flag5;
						if (flag13)
						{
							base.StartCoroutine(this.ShieldPush(item, this.rightVel.normalized.normalized));
						}
					}
				}
				bool flag14 = Time.time - shieldCustomData.lastHit > 0.1f;
				if (flag14)
				{
					bool flag15 = hp - 1 < 1;
					if (flag15)
					{
						base.StartCoroutine(this.DespawnShield(item));
					}
					else
					{
						shieldCustomData.hp = hp - 1;
					}
					shieldCustomData.lastHit = Time.time;
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003886 File Offset: 0x00001A86
		private IEnumerator DespawnShield(Item shield)
		{
			ParticleSystem ps = shield.GetCustomReference("ShatterParticles").GetComponent<ParticleSystem>();
			ParticleSystem activePS = shield.GetCustomReference("ActiveParticles").GetComponent<ParticleSystem>();
			bool flag = !ps.isPlaying;
			if (flag)
			{
				foreach (ColliderGroup colliderGroup in shield.colliderGroups)
				{
					foreach (Collider collider in colliderGroup.colliders)
					{
						collider.enabled = false;
						collider = null;
					}
					List<Collider>.Enumerator enumerator2 = default(List<Collider>.Enumerator);
					colliderGroup = null;
				}
				List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
				foreach (Renderer meshRenderer in shield.renderers)
				{
					meshRenderer.enabled = false;
					meshRenderer = null;
				}
				List<Renderer>.Enumerator enumerator3 = default(List<Renderer>.Enumerator);
				ps.Play();
				activePS.Stop();
				yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
				bool flag2 = shield;
				if (flag2)
				{
					shield.Despawn();
				}
			}
			yield break;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000389C File Offset: 0x00001A9C
		private IEnumerator SpawnRockCoroutine(SpellCaster spellCasterSide)
		{
			EarthBendingController.<>c__DisplayClass85_0 CS$<>8__locals1 = new EarthBendingController.<>c__DisplayClass85_0();
			CS$<>8__locals1.<>4__this = this;
			Vector3 middlePoint = spellCasterSide.transform.position + Player.currentCreature.transform.forward * 0.25f;
			bool flag = Physics.Raycast(middlePoint, Vector3.down, ref CS$<>8__locals1.hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "Default" }));
			if (flag)
			{
				EarthBendingController.<>c__DisplayClass85_1 CS$<>8__locals2 = new EarthBendingController.<>c__DisplayClass85_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.spawnPoint = CS$<>8__locals2.CS$<>8__locals1.hit.point + Vector3.down;
				string randRock = this.rockItemIds[Random.Range(0, this.rockItemIds.Count)];
				Catalog.GetData<ItemData>(randRock, true).SpawnAsync(delegate(Item rock)
				{
					rock.StartCoroutine(CS$<>8__locals2.CS$<>8__locals1.<>4__this.RockSpawnedCoroutine(rock, CS$<>8__locals2.spawnPoint, CS$<>8__locals2.CS$<>8__locals1.hit));
				}, new Vector3?(CS$<>8__locals2.spawnPoint), new Quaternion?(Player.currentCreature.transform.rotation), null, false, null);
				CS$<>8__locals2 = null;
				randRock = null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000038B2 File Offset: 0x00001AB2
		private IEnumerator RockSpawnedCoroutine(Item rock, Vector3 spawnPoint, RaycastHit hit)
		{
			rock.Throw(1f, 1);
			rock.rb.mass = Random.Range(this.rockMassMinMax.x, this.rockMassMinMax.y);
			rock.transform.position = spawnPoint;
			rock.transform.rotation = Player.currentCreature.transform.rotation;
			EffectInstance rockSummonEffect = this.rockSummonEffectData.Spawn(hit.point, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			rockSummonEffect.Play(0, false);
			foreach (ColliderGroup colliderGroup in rock.colliderGroups)
			{
				foreach (Collider collider in colliderGroup.colliders)
				{
					collider.enabled = false;
					collider = null;
				}
				List<Collider>.Enumerator enumerator2 = default(List<Collider>.Enumerator);
				colliderGroup = null;
			}
			List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
			rock.rb.useGravity = false;
			while (rock.transform.position.y < hit.point.y + (Player.currentCreature.ragdoll.headPart.transform.position.y - hit.point.y) / this.rockHeightFromGround)
			{
				rock.transform.position = Vector3.MoveTowards(rock.transform.position, hit.point + new Vector3(0f, (Player.currentCreature.ragdoll.headPart.transform.position.y - hit.point.y) / this.rockHeightFromGround + 0.05f, 0f), Time.deltaTime * this.rockMoveSpeed);
				yield return new WaitForEndOfFrame();
			}
			foreach (ColliderGroup colliderGroup2 in rock.colliderGroups)
			{
				foreach (Collider collider2 in colliderGroup2.colliders)
				{
					collider2.enabled = true;
					collider2 = null;
				}
				List<Collider>.Enumerator enumerator4 = default(List<Collider>.Enumerator);
				colliderGroup2 = null;
			}
			List<ColliderGroup>.Enumerator enumerator3 = default(List<ColliderGroup>.Enumerator);
			rock.rb.velocity = Vector3.zero;
			float startTime = Time.time;
			rock.rb.angularVelocity = Random.insideUnitSphere * 2f;
			rock.colliderGroups[0].collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.Rock_OnDamageReceivedEvent);
			while (Time.time - startTime < this.rockFreezeTime)
			{
				bool flag = this.pushCalled;
				if (flag)
				{
					EffectInstance effectInstance = this.pushEffectData.Spawn(rock.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
					effectInstance.Play(0, false);
					rock.rb.AddForce((this.leftVel.normalized + this.rightVel.normalized) * this.pushForce * this.rockForceMul, 1);
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			rock.rb.useGravity = true;
			rock.colliderGroups[0].collisionHandler.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.Rock_OnDamageReceivedEvent);
			yield return null;
			yield break;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000038D8 File Offset: 0x00001AD8
		private void Rock_OnDamageReceivedEvent(CollisionInstance collisionStruct)
		{
			Debug.Log("Rock on damage");
			bool flag = collisionStruct.impactVelocity.magnitude > 1f;
			if (flag)
			{
				bool flag2 = collisionStruct.sourceColliderGroup == Player.currentCreature.handLeft.colliderGroup;
				bool flag3 = collisionStruct.targetColliderGroup == Player.currentCreature.handLeft.colliderGroup;
				bool flag4 = collisionStruct.sourceColliderGroup == Player.currentCreature.handRight.colliderGroup;
				bool flag5 = collisionStruct.targetColliderGroup == Player.currentCreature.handRight.colliderGroup;
				Item item = null;
				Collider collider = null;
				Vector3 vector = Vector3.zero;
				bool flag6 = flag2 || flag4;
				if (flag6)
				{
					item = collisionStruct.targetCollider.GetComponentInParent<Item>();
					collider = collisionStruct.targetCollider;
				}
				bool flag7 = flag3 || flag5;
				if (flag7)
				{
					item = collisionStruct.sourceCollider.GetComponentInParent<Item>();
					collider = collisionStruct.sourceCollider;
				}
				bool flag8 = item == null;
				if (!flag8)
				{
					item.rb.useGravity = true;
					EffectInstance effectInstance = Catalog.GetData<EffectData>(this.punchEffectId, true).Spawn(item.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
					effectInstance.Play(0, false);
					bool flag9 = flag2 || flag3;
					if (flag9)
					{
						vector = this.AimAssist(item.transform.position, this.leftVel.normalized, this.punchAimPrecision, this.punchAimRandomness);
					}
					bool flag10 = flag4 || flag5;
					if (flag10)
					{
						vector = this.AimAssist(item.transform.position, this.rightVel.normalized, this.punchAimPrecision, this.punchAimRandomness);
					}
					collider.attachedRigidbody.AddForce(vector * this.punchForce, 1);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003AA8 File Offset: 0x00001CA8
		private Vector3 AimAssist(Vector3 ownPosition, Vector3 ownDirection, float aimPrecision, float randomness)
		{
			Creature creature = null;
			float num = -1f;
			Vector3 vector = Vector3.zero;
			foreach (Creature creature2 in Creature.allActive)
			{
				bool flag = creature2 != Player.currentCreature && !creature2.isKilled;
				if (flag)
				{
					Vector3 normalized = (creature2.ragdoll.GetPart(1).transform.position - ownPosition).normalized;
					bool flag2 = Vector3.Dot(ownDirection, normalized) > aimPrecision;
					if (flag2)
					{
						bool flag3 = Vector3.Dot(ownDirection, normalized) > num;
						if (flag3)
						{
							num = Vector3.Dot(ownDirection, normalized);
							creature = creature2;
							vector = normalized;
						}
					}
				}
			}
			bool flag4 = creature != null;
			Vector3 vector3;
			if (flag4)
			{
				Vector3 vector2 = Random.insideUnitSphere * randomness;
				vector3 = (vector + vector2).normalized;
			}
			else
			{
				vector3 = ownDirection;
			}
			return vector3;
		}

		// Token: 0x04000032 RID: 50
		public bool leftHandActive;

		// Token: 0x04000033 RID: 51
		public bool rightHandActive;

		// Token: 0x04000034 RID: 52
		public float shieldMinSpeed;

		// Token: 0x04000035 RID: 53
		public string shieldItemId;

		// Token: 0x04000036 RID: 54
		public float shieldFreezeTime;

		// Token: 0x04000037 RID: 55
		public int shieldHealth;

		// Token: 0x04000038 RID: 56
		public float shieldPushMul;

		// Token: 0x04000039 RID: 57
		public float pushMinSpeed;

		// Token: 0x0400003A RID: 58
		public string pushEffectId;

		// Token: 0x0400003B RID: 59
		public float pushForce;

		// Token: 0x0400003C RID: 60
		public List<string> rockItemIds = new List<string>();

		// Token: 0x0400003D RID: 61
		public Vector2 rockMassMinMax;

		// Token: 0x0400003E RID: 62
		public float rockForceMul;

		// Token: 0x0400003F RID: 63
		public float rockFreezeTime;

		// Token: 0x04000040 RID: 64
		public float rockHeightFromGround;

		// Token: 0x04000041 RID: 65
		public float rockMoveSpeed;

		// Token: 0x04000042 RID: 66
		public string rockSummonEffectId;

		// Token: 0x04000043 RID: 67
		public float punchForce;

		// Token: 0x04000044 RID: 68
		public string punchEffectId;

		// Token: 0x04000045 RID: 69
		public float punchAimPrecision;

		// Token: 0x04000046 RID: 70
		public float punchAimRandomness;

		// Token: 0x04000047 RID: 71
		public string spikeEffectId;

		// Token: 0x04000048 RID: 72
		public float spikeMinSpeed;

		// Token: 0x04000049 RID: 73
		public float spikeRange;

		// Token: 0x0400004A RID: 74
		public float spikeMinAngle;

		// Token: 0x0400004B RID: 75
		public float spikeDamage;

		// Token: 0x0400004C RID: 76
		public string shatterEffectId;

		// Token: 0x0400004D RID: 77
		public float shatterMinSpeed;

		// Token: 0x0400004E RID: 78
		public float shatterRange;

		// Token: 0x0400004F RID: 79
		public float shatterRadius;

		// Token: 0x04000050 RID: 80
		public float shatterForce;

		// Token: 0x04000051 RID: 81
		private bool canSpawnShield = true;

		// Token: 0x04000052 RID: 82
		private bool canSpawnRockLeft = true;

		// Token: 0x04000053 RID: 83
		private bool canSpawnRockRight = true;

		// Token: 0x04000054 RID: 84
		private bool canSpawnSpikes = true;

		// Token: 0x04000055 RID: 85
		private bool canSpawnShatter = true;

		// Token: 0x04000056 RID: 86
		private bool canSpawnPillars = true;

		// Token: 0x04000057 RID: 87
		private bool canPush = true;

		// Token: 0x04000058 RID: 88
		public float rockPillarMinSpeed;

		// Token: 0x04000059 RID: 89
		public string rockPillarPointsId;

		// Token: 0x0400005A RID: 90
		public string rockPillarItemId;

		// Token: 0x0400005B RID: 91
		public string rockPillarCollisionEffectId;

		// Token: 0x0400005C RID: 92
		public float rockPillarLifeTime;

		// Token: 0x0400005D RID: 93
		public float rockPillarSpawnDelay;

		// Token: 0x0400005E RID: 94
		private bool pushCalled;

		// Token: 0x0400005F RID: 95
		private bool wallCalled;

		// Token: 0x04000060 RID: 96
		private bool shatterCalled;

		// Token: 0x04000061 RID: 97
		private Mana mana;

		// Token: 0x04000062 RID: 98
		private Vector3 leftVel;

		// Token: 0x04000063 RID: 99
		private Vector3 rightVel;

		// Token: 0x04000064 RID: 100
		public static bool GravActive;

		// Token: 0x04000065 RID: 101
		public static bool LightningActive;

		// Token: 0x04000066 RID: 102
		public static bool IceActive;

		// Token: 0x04000067 RID: 103
		public static bool FireActive;

		// Token: 0x04000068 RID: 104
		private bool abilityUsed;

		// Token: 0x04000069 RID: 105
		private bool handsPointingDown;

		// Token: 0x0400006A RID: 106
		private EffectData rockPillarCollisionEffectData;

		// Token: 0x0400006B RID: 107
		private EffectData rockPillarPointsData;

		// Token: 0x0400006C RID: 108
		private ItemData rockPillarItemData;

		// Token: 0x0400006D RID: 109
		private EffectData shatterEffectData;

		// Token: 0x0400006E RID: 110
		private EffectData spikeEffectData;

		// Token: 0x0400006F RID: 111
		private EffectData rockSummonEffectData;

		// Token: 0x04000070 RID: 112
		private EffectData punchEffectData;

		// Token: 0x04000071 RID: 113
		private EffectData pushEffectData;

		// Token: 0x04000072 RID: 114
		private ItemData shieldItemData;

		// Token: 0x02000013 RID: 19
		private class ShieldCustomData : ContentCustomData
		{
			// Token: 0x0600005D RID: 93 RVA: 0x0000526F File Offset: 0x0000346F
			public ShieldCustomData(int hp)
			{
				this.hp = hp;
			}

			// Token: 0x040000C0 RID: 192
			public int hp;

			// Token: 0x040000C1 RID: 193
			public float lastHit;
		}
	}
}
