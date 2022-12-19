using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace MysticHands
{
	// Token: 0x02000009 RID: 9
	public class Hand
	{
		// Token: 0x060000BF RID: 191 RVA: 0x000060C4 File Offset: 0x000042C4
		public Hand(Side side, bool claw = false)
		{
			this.side = side;
			this.claw = claw;
			this.jointRB = new GameObject().AddComponent<Rigidbody>();
			this.jointRB.isKinematic = true;
			this.particleSource = new GameObject();
			this.particleSource.transform.position = Player.currentCreature.GetHand(side).transform.position;
			this.SetRB();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006154 File Offset: 0x00004354
		public Vector3 GetHandSnapPos()
		{
			return Player.currentCreature.GetTorso().transform.position + Vector3.up * 0.2f + Player.currentCreature.GetTorso().transform.forward * 0.2f;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000061B4 File Offset: 0x000043B4
		public void SetRB()
		{
			Vector3 distance = Player.currentCreature.GetHand(this.side).transform.position - Player.currentCreature.GetChest();
			Vector3 offset = distance.normalized * Mathf.Pow(distance.magnitude * 2f, 2f) * 7f;
			offset = offset.normalized * Mathf.Max(offset.magnitude, (SpellMysticHands.configScale > 1f) ? Mathf.Sqrt(SpellMysticHands.configScale) : SpellMysticHands.configScale);
			offset *= SpellMysticHands.configLengthMult;
			this.jointRB.transform.position = Player.currentCreature.GetChest() + offset;
			this.jointRB.transform.rotation = Player.currentCreature.GetHand(this.side).transform.rotation;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000062B0 File Offset: 0x000044B0
		public void Toggle(bool claw = false)
		{
			this.active = !this.active;
			this.claw = claw;
			bool flag = !this.active;
			if (flag)
			{
				this.DespawnHand();
			}
			bool flag2 = this.active;
			if (flag2)
			{
				this.SpawnHand();
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006300 File Offset: 0x00004500
		public void OnGrab()
		{
			bool flag = this.grabbedItem;
			if (flag)
			{
				Player.currentCreature.GetHand(this.side).caster.DisableSpellWheel(this);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000633B File Offset: 0x0000453B
		public void OnUnGrab()
		{
			Player.currentCreature.GetHand(this.side).caster.AllowSpellWheel(this);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000635C File Offset: 0x0000455C
		public void CheckGrip()
		{
			bool flag = Player.currentCreature.GetHand(this.side).IsGripping();
			if (flag)
			{
				bool flag2 = !this.wasGripping;
				if (flag2)
				{
					bool grabbed = this.TryGrab();
					bool flag3 = grabbed;
					if (flag3)
					{
						this.OnGrab();
					}
					else
					{
						Transform grabPoint = this.hand.transform.Find("gripTrans");
						Collider[] colliders = Physics.OverlapSphere(grabPoint.position, 0.7f * SpellMysticHands.configScale, LayerMask.GetMask(new string[] { "Default" }), 1);
						RagdollHand ragdollHand = Player.currentCreature.GetHand(this.side);
						float[] array = new float[2];
						array[0] = 1f;
						ragdollHand.PlayHapticClipOver(Utils.Curve(array), 0.3f);
						Physics.IgnoreLayerCollision(GameManager.GetLayer(21), GameManager.GetLayer(2));
						bool flag4 = colliders.Length != 0;
						if (flag4)
						{
							this.SetCollision(this.hand, false);
							this.locked = true;
							Player.fallDamage = false;
							Player.local.locomotion.SetPhysicModifier(this, new float?(0f), 1f, -1f);
							this.lockJoint = this.hand.gameObject.AddComponent<FixedJoint>();
							this.lockJoint.anchor = Vector3.zero;
							this.lockJoint.autoConfigureConnectedAnchor = false;
							this.lockJoint.connectedAnchor = this.hand.transform.position;
						}
					}
				}
				this.wasGripping = true;
			}
			else
			{
				bool flag5 = this.wasGripping;
				if (flag5)
				{
					bool flag6 = this.grabbedItem;
					if (flag6)
					{
						this.OnUnGrab();
					}
					this.TryUnGrab();
					bool flag7 = this.locked;
					if (flag7)
					{
						Player.currentCreature.GetHand(this.side).PlayHapticClipOver(Utils.Curve(new float[] { 0f, 1f }), 0.3f);
						Player.local.locomotion.rb.AddForce(Player.local.locomotion.rb.velocity * 20f, 1);
					}
					this.SetCollision(this.hand, true);
					this.locked = false;
					Player.local.locomotion.RemovePhysicModifier(this);
					Object.Destroy(this.lockJoint);
				}
				this.wasGripping = false;
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000065C4 File Offset: 0x000047C4
		private void SetCollision(Item hand, bool active)
		{
			bool flag = hand == null;
			if (!flag)
			{
				foreach (ColliderGroup cg in hand.colliderGroups)
				{
					foreach (Collider coll in cg.colliders)
					{
						coll.enabled = active;
					}
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000666C File Offset: 0x0000486C
		private Item TryGrabItem()
		{
			Transform grabPoint = this.hand.transform.Find("gripTrans");
			Item item = (from collider in Physics.OverlapSphere(grabPoint.position, 1f, -5, 1)
				orderby Vector3.Distance(collider.transform.position, grabPoint.position)
				select collider).SelectNotNull(delegate(Collider collider)
			{
				Rigidbody attachedRigidbody = collider.attachedRigidbody;
				Item foundItem = ((attachedRigidbody != null) ? attachedRigidbody.GetComponentInParent<Item>() : null);
				return (foundItem != null) ? foundItem : null;
			}).FirstOrDefault((Item foundItem) => foundItem != this.hand && foundItem != this.otherHand.hand && foundItem.handles.Any<Handle>());
			bool flag = !item;
			Item item2;
			if (flag)
			{
				item2 = null;
			}
			else
			{
				this.SetCollision(this.hand, false);
				this.hand.mainCollisionHandler.damagers.First<Damager>().data.playerDamageMultiplier = 0f;
				this.hand.RunAfter(delegate
				{
					this.SetCollision(this.hand, true);
				}, 1f);
				Holder holder = item.holder;
				if (holder != null)
				{
					holder.UnSnap(item, false, true);
				}
				item.handlers.ToList<RagdollHand>().ForEach(delegate(RagdollHand handler)
				{
					handler.UnGrab(false);
				});
				this.grabJoint = Utils.CreateGrabJoint(this.hand.rb, grabPoint, this.side, item);
				bool flag2 = this.grabJoint == null;
				if (flag2)
				{
					item2 = null;
				}
				else
				{
					item.rb.collisionDetectionMode = Catalog.gameData.collisionDetection.grabbed;
					item.rb.sleepThreshold = 0f;
					item.StopThrowing();
					item.StopFlying();
					item.SetColliderAndMeshLayer(GameManager.GetLayer(5), false);
					item.disallowDespawn = true;
					this.grabbedItem = item;
					item2 = item;
				}
			}
			return item2;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006840 File Offset: 0x00004A40
		public Creature TryGrabCreature()
		{
			Transform grabPoint = this.hand.transform.Find("gripTrans");
			Creature creature = (from foundCreature in Creature.allActive
				where Vector3.Distance(foundCreature.GetChest(), grabPoint.position) < 1f
				orderby Vector3.Distance(foundCreature.GetChest(), grabPoint.position)
				select foundCreature).FirstOrDefault((Creature foundCreature) => !foundCreature.isPlayer);
			bool flag = !creature;
			Creature creature2;
			if (flag)
			{
				creature2 = null;
			}
			else
			{
				foreach (Collider collider in this.hand.colliderGroups.First<ColliderGroup>().colliders)
				{
					creature.ragdoll.IgnoreCollision(collider, true, 0);
				}
				this.SetCollision(this.hand, false);
				this.hand.RunAfter(delegate
				{
					this.SetCollision(this.hand, true);
				}, 1f);
				this.grabbedCreature = creature;
				this.grabJoint = Utils.CreateGrabJoint(this.hand.rb, grabPoint, this.side, creature);
				creature2 = creature;
			}
			return creature2;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006994 File Offset: 0x00004B94
		public bool TryGrab()
		{
			bool flag = !this.grabbedCreature && !this.grabbedItem;
			bool flag3;
			if (flag)
			{
				Creature creature = this.TryGrabCreature();
				bool flag2 = !creature;
				flag3 = !flag2 || this.TryGrabItem() != null;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000069F4 File Offset: 0x00004BF4
		public bool TryUnGrab()
		{
			bool flag = this.grabJoint != null;
			if (flag)
			{
				Object.Destroy(this.grabJoint);
			}
			bool flag2 = this.grabbedItem;
			bool flag5;
			if (flag2)
			{
				Item item = this.grabbedItem;
				bool flag3 = this.hand.rb.velocity.magnitude > 3f;
				if (flag3)
				{
					float multiplier = 1f;
					bool flag4 = item.rb.mass < 1f;
					if (flag4)
					{
						multiplier *= item.rb.mass * 2f;
					}
					else
					{
						multiplier *= item.rb.mass;
					}
					this.grabbedItem.rb.AddForce(this.hand.rb.velocity * multiplier, 1);
				}
				item.rb.sleepThreshold = item.orgSleepThreshold;
				item.rb.collisionDetectionMode = Catalog.gameData.collisionDetection.dropped;
				item.Throw(1f, 2);
				item.IgnoreObjectCollision(this.hand);
				item.disallowDespawn = false;
				item.RunAfter(delegate
				{
					item.ResetObjectCollision();
				}, 1f);
				this.grabbedItem = null;
				flag5 = true;
			}
			else
			{
				bool flag6 = this.grabbedCreature;
				if (flag6)
				{
					bool flag7 = this.hand.rb.velocity.magnitude > 3f;
					if (flag7)
					{
						this.grabbedCreature.locomotion.rb.AddForce(this.hand.rb.velocity * 10f, 1);
					}
					Creature creature = this.grabbedCreature;
					creature.RunAfter(delegate
					{
						bool flag8 = this.grabbedCreature != creature;
						if (flag8)
						{
							foreach (Collider collider in this.hand.colliderGroups.First<ColliderGroup>().colliders)
							{
								try
								{
									bool flag9 = ((collider != null) ? collider.gameObject : null);
									if (flag9)
									{
										creature.ragdoll.IgnoreCollision(collider, false, 0);
									}
								}
								catch (NullReferenceException)
								{
								}
							}
						}
					}, 1f);
					this.grabbedCreature = null;
					flag5 = true;
				}
				else
				{
					flag5 = false;
				}
			}
			return flag5;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006C48 File Offset: 0x00004E48
		public void Update()
		{
			this.SetRB();
			bool flag = this.active;
			if (flag)
			{
				bool flag2 = this.hand != null;
				if (flag2)
				{
					this.SetPlayerJoint();
					this.SetHand();
					bool configShowTrail = SpellMysticHands.configShowTrail;
					if (configShowTrail)
					{
						this.SetEffectParams();
					}
				}
				bool flag3 = this.grabbedItem;
				if (flag3)
				{
					RagdollHand hand = Player.currentCreature.GetHand(this.side);
					bool alternateUsePressed = hand.playerHand.controlHand.alternateUsePressed;
					if (alternateUsePressed)
					{
						bool flag4 = !this.buttoning;
						if (flag4)
						{
							this.buttoning = true;
							this.grabbedItem.OnHeldAction(hand, this.grabbedItem.GetMainHandle(this.side), 2);
							this.grabbedItem.OnHeldAction(hand, this.grabbedItem.GetMainHandle(this.side), 0);
						}
					}
					else
					{
						bool flag5 = this.buttoning;
						if (flag5)
						{
							this.buttoning = false;
							this.grabbedItem.OnHeldAction(hand, this.grabbedItem.GetMainHandle(this.side), 3);
							this.grabbedItem.OnHeldAction(hand, this.grabbedItem.GetMainHandle(this.side), 1);
						}
					}
					this.grabbedItem.SetColliderAndMeshLayer(GameManager.GetLayer(5), false);
				}
			}
			this.UpdateScaleTest();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006DA8 File Offset: 0x00004FA8
		public void SetPlayerJoint()
		{
			Vector3 targetPoint = Player.currentCreature.GetChest() + (this.hand.transform.position - this.jointRB.transform.position);
			bool flag = this.locked;
			if (flag)
			{
				this.playerJoint.spring = 1000f;
				this.playerJoint.damper = 300f;
				Player.local.locomotion.rb.velocity = Vector3.Slerp(Player.local.locomotion.rb.velocity, targetPoint - Player.currentCreature.GetChest(), Time.deltaTime * 4f);
			}
			else
			{
				bool flag2 = this.playerJoint;
				if (flag2)
				{
					this.playerJoint.spring = 0f;
					this.playerJoint.damper = 0f;
				}
			}
			bool flag3 = this.playerJoint;
			if (flag3)
			{
				this.playerJoint.connectedAnchor = targetPoint;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006EB8 File Offset: 0x000050B8
		public void UpdateScaleTest()
		{
			RagdollHand thisHand = Player.currentCreature.GetHand(this.side);
			RagdollHand otherHand = Player.currentCreature.GetHand(this.side.Other());
			SpellCaster caster = otherHand.caster;
			SpellCastCharge spell = ((caster != null) ? caster.spellInstance : null) as SpellCastCharge;
			bool flag = spell != null && spell.id == "Scale";
			if (flag)
			{
				bool flag2 = Vector3.Distance(thisHand.Palm(), otherHand.Palm()) < 0.2f && otherHand.caster.isFiring && this.grabbedItem != null;
				if (flag2)
				{
					bool flag3 = !this.hasResized;
					if (flag3)
					{
						this.hasResized = true;
						Catalog.GetData<EffectData>("HandScale", true).Spawn(this.grabbedItem.transform.position, this.grabbedItem.transform.rotation, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
						Player.currentCreature.mana.gameObject.SendMessage("Supersize", this.grabbedItem);
					}
				}
				else
				{
					this.hasResized = false;
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006FE8 File Offset: 0x000051E8
		public void DespawnHand()
		{
			Player.currentCreature.mana.StartCoroutine(this.DespawnCoroutine());
			EffectInstance effectInstance = this.effect;
			if (effectInstance != null)
			{
				effectInstance.SetIntensity(0f);
			}
			EffectInstance effectInstance2 = this.effect;
			if (effectInstance2 != null)
			{
				effectInstance2.End(false, -1f);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000703C File Offset: 0x0000523C
		public void SpawnHand()
		{
			bool flag = this.hand;
			if (flag)
			{
				this.DespawnHand();
			}
			Catalog.GetData<ItemData>(this.claw ? "MysticClaw" : ((this.side == 1) ? "MysticHandLeft" : "MysticHandRight"), true).SpawnAsync(delegate(Item hand)
			{
				this.hand = hand;
				foreach (Renderer renderer in hand.renderers)
				{
					Material material = renderer.material;
					if (material != null)
					{
						material.SetColor("HandColor", SpellMysticHands.configHandColor);
					}
				}
				Transform transform = hand.transform.Find("Spot Light");
				Light light = ((transform != null) ? transform.GetComponent<Light>() : null);
				bool flag2 = light != null;
				if (flag2)
				{
					light.color = SpellMysticHands.configLightColor;
				}
				hand.transform.position = this.jointRB.position;
				hand.transform.rotation = this.jointRB.rotation;
				hand.transform.localScale = Vector3.one * SpellMysticHands.configScale;
				hand.disallowDespawn = true;
				hand.disallowRoomDespawn = true;
				hand.OnCullEvent += delegate(bool culled)
				{
					if (culled)
					{
						this.DespawnHand();
						this.active = false;
						hand = null;
					}
				};
				hand.mainCollisionHandler.OnCollisionStartEvent += delegate(CollisionInstance instance)
				{
					Player.currentCreature.GetHand(this.side).HapticTick(instance.intensity, 10f);
					this.Shockwave(instance);
				};
				this.effect = Catalog.GetData<EffectData>("HandEffect", true).Spawn(Player.currentCreature.mana.transform, false, null, false, Array.Empty<Type>());
				EffectInstance effectInstance = this.effect;
				if (effectInstance != null)
				{
					effectInstance.SetIntensity(1f);
				}
				EffectInstance effectInstance2 = this.effect;
				if (effectInstance2 != null)
				{
					effectInstance2.SetSource(this.particleSource.transform);
				}
				EffectInstance effectInstance3 = this.effect;
				if (effectInstance3 != null)
				{
					effectInstance3.SetTarget(hand.holderPoint);
				}
				bool configShowTrail = SpellMysticHands.configShowTrail;
				if (configShowTrail)
				{
					this.effect.Play(0, false);
				}
				Catalog.GetData<EffectData>("HandSpawn", true).Spawn(hand.transform.position, hand.transform.rotation, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
				this.playerJoint = Player.local.locomotion.gameObject.AddComponent<SpringJoint>();
				this.playerJoint.spring = 0f;
				this.playerJoint.damper = 0f;
				this.playerJoint.autoConfigureConnectedAnchor = false;
				this.playerJoint.anchor = Player.currentCreature.transform.InverseTransformPoint(Player.currentCreature.GetChest());
				this.joint = Utils.CreateTKJoint(this.jointRB, hand.rb);
				foreach (Collider collider in hand.colliderGroups.First<ColliderGroup>().colliders)
				{
					Hand hand2 = this.otherHand;
					bool flag3 = ((hand2 != null) ? hand2.hand : null);
					if (flag3)
					{
						foreach (Collider otherCollider in this.otherHand.hand.colliderGroups.First<ColliderGroup>().colliders)
						{
							bool flag4 = collider != null && otherCollider != null;
							if (flag4)
							{
								Physics.IgnoreCollision(collider, otherCollider);
							}
						}
					}
				}
				Player.currentCreature.mana.StartCoroutine(this.SpawnCoroutine(hand));
			}, null, null, null, true, null);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000070B4 File Offset: 0x000052B4
		public void Shockwave(CollisionInstance hit)
		{
			bool flag = hit.impactVelocity.magnitude > 13f && hit.targetColliderGroup == null && Player.currentCreature.GetHand(this.side).IsGripping() && (!this.locked & (Time.time - this.lastSmash > 0.5f));
			if (flag)
			{
				RagdollHand ragdollHand = Player.currentCreature.GetHand(this.side);
				float[] array = new float[2];
				array[0] = 1f;
				ragdollHand.PlayHapticClipOver(Utils.Curve(array), 0.5f);
				this.lastSmash = Time.time;
				Vector3 contactPoint = hit.contactPoint;
				float num = 30f;
				float num2 = 4f;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = false;
				bool flag5 = false;
				int num3 = 4;
				float num4 = 0f;
				Rigidbody[] array2 = new Rigidbody[2];
				array2[0] = this.hand.rb;
				int num5 = 1;
				Hand hand = this.otherHand;
				Rigidbody rigidbody;
				if (hand == null)
				{
					rigidbody = null;
				}
				else
				{
					Item item = hand.hand;
					rigidbody = ((item != null) ? item.rb : null);
				}
				array2[num5] = rigidbody;
				Utils.Explosion(contactPoint, num, num2, flag2, flag3, flag4, flag5, num3, num4, array2);
				Catalog.GetData<EffectData>("HandShockwave", true).Spawn(hit.contactPoint, Quaternion.LookRotation(hit.contactNormal), null, null, false, null, false, Array.Empty<Type>()).Play(0, false);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000071E3 File Offset: 0x000053E3
		public IEnumerator DespawnCoroutine()
		{
			Item oldHand = this.hand;
			this.hand = null;
			ConfigurableJoint joint = this.joint;
			SpringJoint playerJoint = this.playerJoint;
			Catalog.GetData<EffectData>("HandDespawn", true).Spawn(oldHand.transform.position, oldHand.transform.rotation, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
			this.SetCollision(oldHand, false);
			foreach (Renderer renderer in oldHand.renderers)
			{
				renderer.material.SetFloat("SpawnAmount", 1f);
				renderer = null;
			}
			List<Renderer>.Enumerator enumerator = default(List<Renderer>.Enumerator);
			float startTime = Time.time;
			Object.Destroy(playerJoint);
			while (Time.time - startTime < 0.3f)
			{
				float ratio = 1f - (Time.time - startTime) / 0.3f;
				foreach (Renderer renderer2 in oldHand.renderers)
				{
					renderer2.material.SetFloat("SpawnAmount", ratio);
					renderer2 = null;
				}
				List<Renderer>.Enumerator enumerator2 = default(List<Renderer>.Enumerator);
				yield return 0;
			}
			foreach (Renderer renderer3 in oldHand.renderers)
			{
				renderer3.material.SetFloat("SpawnAmount", 0f);
				renderer3 = null;
			}
			List<Renderer>.Enumerator enumerator3 = default(List<Renderer>.Enumerator);
			Item item = oldHand;
			if (item != null)
			{
				item.Despawn();
			}
			oldHand.enabled = true;
			bool flag = joint != null;
			if (flag)
			{
				Object.Destroy(joint, 0.1f);
			}
			bool flag2 = oldHand;
			if (flag2)
			{
				Object.Destroy(oldHand, 0.2f);
			}
			yield break;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000071F2 File Offset: 0x000053F2
		public IEnumerator SpawnCoroutine(Item hand)
		{
			foreach (Renderer renderer in hand.renderers)
			{
				renderer.material.SetFloat("SpawnAmount", 0f);
				renderer = null;
			}
			List<Renderer>.Enumerator enumerator = default(List<Renderer>.Enumerator);
			float startTime = Time.time;
			while (Time.time - startTime < 0.3f)
			{
				float ratio = (Time.time - startTime) / 0.3f;
				foreach (Renderer renderer2 in hand.renderers)
				{
					renderer2.material.SetFloat("SpawnAmount", ratio);
					renderer2 = null;
				}
				List<Renderer>.Enumerator enumerator2 = default(List<Renderer>.Enumerator);
				yield return 0;
			}
			foreach (Renderer renderer3 in hand.renderers)
			{
				renderer3.material.SetFloat("SpawnAmount", 1f);
				renderer3 = null;
			}
			List<Renderer>.Enumerator enumerator3 = default(List<Renderer>.Enumerator);
			yield break;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007208 File Offset: 0x00005408
		public void SetHand()
		{
			bool flag = !this.hand.isFlying;
			if (flag)
			{
				this.hand.Throw(1f, 2);
			}
			this.CheckGrip();
			Creature creature = this.grabbedCreature;
			bool flag2 = creature != null;
			if (flag2)
			{
				creature.TryPush(0, Vector3.zero, 2, 0);
			}
			bool flag3 = this.grabbedItem;
			if (flag3)
			{
				this.grabbedItem.SetColliderAndMeshLayer(GameManager.GetLayer(5), false);
			}
			bool flag4 = this.grabbedItem || this.grabbedCreature;
			float thumb;
			float index;
			float middle;
			float ring;
			float little;
			if (flag4)
			{
				thumb = 0.5f;
				index = 0.5f;
				middle = 0.5f;
				ring = 0.5f;
				little = 0.5f;
			}
			else
			{
				bool flag5 = this.locked;
				if (flag5)
				{
					thumb = 1f;
					index = 1f;
					middle = 1f;
					ring = 1f;
					little = 1f;
				}
				else
				{
					thumb = Player.currentCreature.GetHand(this.side).playerHand.controlHand.thumbCurl;
					index = Player.currentCreature.GetHand(this.side).playerHand.controlHand.indexCurl;
					middle = Player.currentCreature.GetHand(this.side).playerHand.controlHand.middleCurl;
					ring = Player.currentCreature.GetHand(this.side).playerHand.controlHand.ringCurl;
					little = Player.currentCreature.GetHand(this.side).playerHand.controlHand.littleCurl;
				}
			}
			Animator animator = this.hand.GetComponent<Animator>();
			float moveSpeed = 10f;
			animator.SetFloat("ThumbCurl", Mathf.Lerp(animator.GetFloat("ThumbCurl"), thumb, Time.deltaTime * moveSpeed));
			animator.SetFloat("IndexCurl", Mathf.Lerp(animator.GetFloat("IndexCurl"), (this.claw && this.side == 1) ? middle : index, Time.deltaTime * moveSpeed));
			animator.SetFloat("MiddleCurl", Mathf.Lerp(animator.GetFloat("MiddleCurl"), (this.claw && this.side == 1) ? index : middle, Time.deltaTime * moveSpeed));
			animator.SetFloat("RingCurl", Mathf.Lerp(animator.GetFloat("RingCurl"), ring, Time.deltaTime * moveSpeed));
			animator.SetFloat("LittleCurl", Mathf.Lerp(animator.GetFloat("LittleCurl"), little, Time.deltaTime * moveSpeed));
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000074B0 File Offset: 0x000056B0
		public void SetEffectParams()
		{
			this.particleSource.transform.SetPositionAndRotation(Player.currentCreature.GetChest(), Quaternion.LookRotation((float)((this.side == 1) ? (-1) : 1) * Player.currentCreature.GetTorso().transform.up));
		}

		// Token: 0x04000012 RID: 18
		public GameObject particleSource;

		// Token: 0x04000013 RID: 19
		private readonly Side side;

		// Token: 0x04000014 RID: 20
		private Item hand;

		// Token: 0x04000015 RID: 21
		private Rigidbody jointRB;

		// Token: 0x04000016 RID: 22
		private EffectInstance effect;

		// Token: 0x04000017 RID: 23
		private ConfigurableJoint joint;

		// Token: 0x04000018 RID: 24
		private ConfigurableJoint grabJoint;

		// Token: 0x04000019 RID: 25
		private SpringJoint playerJoint;

		// Token: 0x0400001A RID: 26
		private FixedJoint lockJoint;

		// Token: 0x0400001B RID: 27
		private bool locked;

		// Token: 0x0400001C RID: 28
		private Item grabbedItem;

		// Token: 0x0400001D RID: 29
		private Creature grabbedCreature;

		// Token: 0x0400001E RID: 30
		private bool wasGripping = false;

		// Token: 0x0400001F RID: 31
		public bool active = false;

		// Token: 0x04000020 RID: 32
		private bool hasResized = false;

		// Token: 0x04000021 RID: 33
		public Hand otherHand;

		// Token: 0x04000022 RID: 34
		public float lastSmash;

		// Token: 0x04000023 RID: 35
		public bool claw;

		// Token: 0x04000024 RID: 36
		private bool buttoning;
	}
}
